using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using org.rsp.database.Table;
using org.rsp.entity.Common;
using org.rsp.entity.Request;
using org.rsp.entity.service;
using org.rsp.management.Wrapper;

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


    /// <summary>
    /// 查询全部
    /// </summary>
    /// <param name="request"></param>
    public async Task<List<Record>> QueryRecordAsync(QueryRecordConditionRequest request)
    {
        var response = new List<Record>();

        if (request.Direction != null)
        {
            response = await _wrapper.RecordRepository
                .FindByCondition(_ => _.Direction == request.Direction)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize).ToListAsync();
        }
        else
        {
            response = await _wrapper.RecordRepository
                .FindAll()
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize).ToListAsync();
        }

        return response;
    }


    /// <summary>
    /// 添加入库/出库
    /// </summary>
    /// <param name="request"></param>
    public async Task AddWareHouseRecordAsync(AddWareHouseRecordRequest request)
    {
        await locker.WaitAsync();

        IDbContextTransaction transaction = null;
        try
        {
            if (request.Records.Any())
            {
                //开启事务
                transaction = await _wrapper.StartTransactionAsync();

                foreach (var record in request.Records)
                {
                    //先查询
                    var everGoods = await _wrapper.GoodsRepository.FindByCondition(_ =>
                        _.GoodsName == record.GoodsName && _.StoreHouseId == record.StoreHouseId).FirstOrDefaultAsync();
                    //分流水方向
                    if (record.Direction == 0)
                    {
                        //入库
                        if (everGoods != null)
                        {
                            //数据库有则直接添加数量；
                            everGoods.Number += record.Quantity;
                            _wrapper.GoodsRepository.Update(everGoods);
                        }
                        else
                        {
                            //没有的话则新建一条产品信息
                            var newGoods = await _wrapper.GoodsRepository.FindByCondition(_ =>
                                _.GoodsId == record.GoodsId).FirstOrDefaultAsync();

                            newGoods.GoodsId = 0;
                            newGoods!.StoreHouseId = record.StoreHouseId;
                            newGoods.Number = record.Quantity;

                            _wrapper.GoodsRepository.Create(newGoods);
                        }
                    }
                    else
                    {
                        //出库 直接减数量
                        if (everGoods != null)
                        {
                            everGoods.Number -= record.Quantity;
                            _wrapper.GoodsRepository.Update(everGoods);
                        }
                    }

                    var addRecord = _mapper.Map<Record>(record);
                    _wrapper.RecordRepository.Create(addRecord);
                }

                await _wrapper.SaveChangeAsync();

                await transaction.CommitAsync();
            }
        }
        catch (Exception e)
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
    public async Task DelRecordAsync(List<int> ids)
    {
        if (!ids.Any())
            return;

        //logic delete
        var bindInSql = SqlBindHelper.BindInSql(ids);
        await _wrapper.ExecuteSqlRaw($"update [teest].[dbo].[Records] set IsDeleted =1 where RecordId in ({bindInSql}) " );
    }
}