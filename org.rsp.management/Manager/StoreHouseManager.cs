﻿using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Crypto;
using org.rsp.database.Expressions;
using org.rsp.database.Extensions;
using org.rsp.database.Table;
using org.rsp.entity.Common;
using org.rsp.entity.Request;
using org.rsp.entity.Response;
using org.rsp.entity.service;
using org.rsp.management.Wrapper;

namespace org.rsp.management.Manager;

public class StoreHouseManager : IStoreHouseManager, ITransient
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly ILogger<StoreHouseManager> _logger;

    private readonly IMapper _mapper;

    public StoreHouseManager(ILogger<StoreHouseManager> logger, IMapper mapper, IRepositoryWrapper wrapper)
    {
        _logger = logger;
        _mapper = mapper;
        _wrapper = wrapper;
    }

    public async Task AddStoreHouseAsync(AddStoreHouseRequest request)
    {
        try
        {
            //the unique storehouse is storehouseName & Location is different.
            var storeHouse = await _wrapper.StoreHouseRepository
                .FindByCondition(_ =>
                    string.Equals(_.StoreHouseName, request.StoreHouseName) &&
                    string.Equals(_.Location, request.Location)).FirstOrDefaultAsync();

            if (storeHouse is not null)
                return;

            request.UpdateBy = request.CreateBy;
            var addEntity = _mapper.Map<StoreHouse>(request);
    
            _wrapper.StoreHouseRepository.Create(addEntity);
            await _wrapper.SaveChangeAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"AddStoreHouseAsync error: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// 更新仓库
    /// </summary>
    /// <param name="request"></param>
    public async Task UpdateStoreHouseAsync(UpdateStoreHouseRequest request)
    {
        try
        {
            var entity = await _wrapper.StoreHouseRepository
                .FindByCondition(_ => _.StoreHouseId == request.StoreHouseId).FirstOrDefaultAsync();

            if(entity is null)
                return;
            
            if (!string.IsNullOrEmpty(request.StoreHouseName) && !string.Equals(request.StoreHouseName,entity.StoreHouseName))
            {
                //为空则不更新
                entity.StoreHouseName = request.StoreHouseName;
            }

            if ( !string.Equals(request.Remark,entity.Remark))
            {
                //remark can be null.
                entity.Remark = request.Remark;
            }
            
            if (!string.IsNullOrEmpty(request.Location) && !string.Equals(request.Location,entity.Location))
            {
                entity.Location = request.Location;
            }
            
            entity.UpdateBy = request.UpdateBy;
            entity.UpdateTime=DateTime.Now;
            _wrapper.StoreHouseRepository.Update(entity);
            await _wrapper.SaveChangeAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"UpdateStoreHouseAsync error: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// 批量删除仓库
    /// </summary>
    /// <param name="ids"></param>
    public async Task BatchDeleteStoreHouseAsync(List<int> ids)
    {
        try
        {
            if (!ids.Any())
                return;

            var delList = await _wrapper.StoreHouseRepository.FindByCondition(_ => ids.Contains(_.StoreHouseId))
                .ToListAsync();
            foreach (var storeHouse in delList)
            {
                storeHouse.IsDeleted = true;
                _wrapper.StoreHouseRepository.Update(storeHouse);
            }

            await _wrapper.SaveChangeAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"BatchDeleteStoreHouseAsync error: {e.Message}");
            throw;
        }
    }

    /// <summary>
    /// 根据条件查询仓库
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<QueryStoreHouseResponse> QueryStoreHouseAsync(QueryStoreHouseRequest request)
    {
        var response = new QueryStoreHouseResponse();
        try
        {
            //后续增加redis 缓存
            Expression<Func<StoreHouse, bool>> expression = ExpressionExtension.True<StoreHouse>();
            if (!string.IsNullOrEmpty(request.StoreHouseName))
            {
                expression = expression.And(p => p.StoreHouseName.Contains(request.StoreHouseName.Trim()));
            }

            if (!string.IsNullOrEmpty(request.Location))
            {
                expression = expression.And(p => p.Location != null && p.Location.Contains(request.Location.Trim()));
            }

            expression = expression.And(p => p.IsDeleted == false);
            
            var storeHouses = await _wrapper.StoreHouseRepository.FindByCondition(expression)
                .OrderByDescending(_ => _.UpdateTime)
                .Skip((request.PageNumber-1)*request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            if (storeHouses.Any())
            {
                response.StoreHouses = storeHouses;
            }

            response.TotalCount = await _wrapper.StoreHouseRepository.FindByCondition(expression).CountAsync();
            
        }
        catch (Exception e)
        {
            _logger.LogError($"QueryStoreHouseAsync error: {e.Message}");
            throw;
        }

        return response;
    }
    
}