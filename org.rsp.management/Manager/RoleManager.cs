using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using org.rsp.database.Table;
using org.rsp.entity.Helper;
using org.rsp.entity.Model;
using org.rsp.entity.Request;
using org.rsp.entity.Response;
using org.rsp.entity.service;
using org.rsp.management.Tools;
using org.rsp.management.Wrapper;

namespace org.rsp.management.Manager;

public class RoleManager : IRoleManager,ITransient
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly ILogger<RoleManager> _logger;
    private readonly IMapper _mapper;

    public RoleManager( ILogger<RoleManager> logger, IMapper mapper,
        IRepositoryWrapper wrapper)
    {
        _logger = logger;
        _mapper = mapper;
        _wrapper = wrapper;
    }
    
    /// <summary>
    /// 新增角色
    /// </summary>
    /// <param name="request"></param>
    public async Task AddRoleAsync(AddRoleRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                return;
            }

            var role = _mapper.Map<Role>(request);
            _wrapper.Role.Create(role);
            await _wrapper.SaveChangeAsync();
        }
        catch (Exception _)
        {
            _logger.LogError("AddRoleAsync error.");
            throw;
        }
    }
    
    
    /// <summary>
    /// 查询所有的角色
    /// </summary>
    /// <returns></returns>
    public async Task<QueryAllRolesResponse> QueryAllRolesAsync(QueryAllRolesRequest request)
    {
        var response = new QueryAllRolesResponse();

        try
        {
            //查询数据库
            var roles =await _wrapper.Role.FindAll().Where(_ => _.IsDeleted == false)
                .Skip((request.PageNumber-1) * request.PageSize).Take(request.PageNumber).ToListAsync();
            var data = _mapper.Map<List<RoleModel>>(roles);
            response.RoleModels = data;
            
        }
        catch (Exception e)
        {
            _logger.LogError($"QueryAllRolesAsync error: {e.Message}");
            throw;
        }

        return response;
    }
}