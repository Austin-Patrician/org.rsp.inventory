using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using org.rsp.database.Expressions;
using org.rsp.database.Table;
using org.rsp.entity.Common;
using org.rsp.entity.Model;
using org.rsp.entity.Request;
using org.rsp.entity.Response;
using org.rsp.entity.service;
using org.rsp.management.Wrapper;
using Microsoft.AspNetCore.Mvc;


namespace org.rsp.management.Manager;

public class RecordManager : IRecordManager, ITransient
{
    private readonly ILogger<RecordManager> _logger;
    private readonly IRepositoryWrapper _wrapper;
    private readonly IMapper _mapper;

    private readonly SemaphoreSlim locker = new(1, 1);

    public RecordManager(ILogger<RecordManager> logger, IRepositoryWrapper wrapper, IMapper mapper)
    {
        _logger = logger;
        _wrapper = wrapper;
        _mapper = mapper;
    }


    public async Task UpdateRecordAsync(UpdateRecordRequest request)
    {
        var record =await _wrapper.RecordRepository.FindByCondition(_=>_.RecordId==request.RecordId).FirstOrDefaultAsync();
        if (record is null)
            return;
        
        if (!string.IsNullOrEmpty(request.Use) && !string.Equals(request.Use,record.Use,StringComparison.OrdinalIgnoreCase))
        {
            record.Use = request.Use;
            _wrapper.RecordRepository.Update(record);
            await _wrapper.SaveChangeAsync();
        }
    }

    /// <summary>
    /// 查询全部
    /// </summary>
    /// <param name="request"></param>
    public async Task<QueryRecordResponse> QueryRecordAsync(QueryRecordRequest request)
    {
        QueryRecordResponse response = new();

        try
        {
            Expression<Func<Record, bool>> expression = ExpressionExtension.True<Record>();

            if (request.Direction != null)
            {
                expression = expression.And(p => p.Direction == request.Direction);
            }

            expression = expression.And(p => p.IsDeleted == false);

            var records = await _wrapper.RecordRepository
                .FindByCondition(expression)
                .Include(_ => _.StoreHouse)
                .Include(_ => _.Goods).OrderByDescending(_ => _.TradeTime)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize).ToListAsync();

            if (records.Any())
            {
                foreach (var record in records)
                {
                    var recordModel = _mapper.Map<RecordModel>(record);
                    recordModel.DirectionName = record.Direction == 1 ? "出库" : "入库";
                    response.RecordModels.Add(recordModel);
                }
            }

            var totalCount = await _wrapper.RecordRepository
                .FindByCondition(expression).CountAsync();
            response.TotalCount = totalCount;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return response;
    }

    public async Task<byte[]> ExportRecord()
    {
        return null;
    }


    /// <summary>
    /// 添加入库/出库
    /// </summary>
    /// <param name="request"></param>
    public async Task<AddRecordResponse> AddRecordAsync(AddWareHouseRecordRequest request)
    {
        await locker.WaitAsync();
        var response = new AddRecordResponse();
        IDbContextTransaction transaction = null;
        try
        {
            //开启事务
            transaction = await _wrapper.StartTransactionAsync();
            
            //先查询该仓库是否有这个商品
            var everGoods = await _wrapper.GoodsRepository.FindByCondition(_ =>
                _.GoodsId == request.GoodsId && _.StoreHouseId == request.StoreHouseId).FirstOrDefaultAsync();
            //分流水方向
            if (request.Direction == 0)
            {
                //入库,如果有这个数据，则直接添加数量
                if (everGoods != null)
                {
                    //数据库有则直接添加数量；
                    everGoods.Number += request.Quantity;
                    everGoods.UpdateTime = DateTime.UtcNow;
                    everGoods.UpdateBy = request.CreateBy;
                    _wrapper.GoodsRepository.Update(everGoods);
                }
                else
                {
                    //没有的话则新建一条产品信息
                    var newGoods = await _wrapper.GoodsRepository.FindByCondition(_ =>
                        _.GoodsId == request.GoodsId).FirstOrDefaultAsync();

                    var goods = new Goods
                    {
                        GoodsName= newGoods!.GoodsName,
                        StoreHouseId = request.StoreHouseId,
                        GoodsCategoryId = newGoods.GoodsCategoryId,
                        IsDeleted = false,
                        CreateTime = DateTime.UtcNow,
                        CreateBy = request.CreateBy,
                        UpdateBy = request.CreateBy,
                        UpdateTime = DateTime.UtcNow,
                        Number = request.Quantity
                    };
                    
                    _wrapper.GoodsRepository.Create(goods);
                }
            }
            else if(request.Direction==1)
            {
                //出库 直接减数量
                if (everGoods != null)
                {
                    everGoods.Number -= request.Quantity;
                    everGoods.UpdateTime = DateTime.UtcNow;
                    everGoods.UpdateBy = request.CreateBy;
                    _wrapper.GoodsRepository.Update(everGoods);
                }
                else
                {
                    response.Message = "该仓库没有该产品信息，请重新选择";
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            else
            {
                response.Message = "请输入有效的出入库方向";
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            var addRecord = _mapper.Map<Record>(request);
            _wrapper.RecordRepository.Create(addRecord);
            
            await _wrapper.SaveChangeAsync();

            await transaction.CommitAsync();

            return response;
        }
        catch (Exception e) when (e.Message.Contains(""))
        {
            _logger.LogError($"AddWareHouseRecordAsync error: {e.Message}");
            if (transaction != null) await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            locker.Release();
        }
    }

    

    /// <summary>
    /// 更新record
    /// </summary>
    /// <param name="ids"></param>
    public async Task BatchDelRecordAsync(List<int> ids)
    {
        if (!ids.Any())
            return;

        //logic delete
        var bindInSql = SqlBindHelper.BindInSql(ids);
        await _wrapper.ExecuteSqlRaw(
            $"update [teest].[dbo].[Records] set IsDeleted =1 where RecordId in ({bindInSql}) ");
    }
}