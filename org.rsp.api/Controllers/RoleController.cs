using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using org.rsp.api.Auth;
using org.rsp.entity.Request;
using org.rsp.entity.Response;
using org.rsp.entity.service;

namespace org.rsp.api.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class RoleController
{
    private readonly IRoleManager _manager;

    private readonly ILogger<RoleController> _logger;
    
    public RoleController( ILogger<RoleController> logger, IRoleManager manager)
    {
        _logger = logger;
        _manager = manager;
    }
    
    
    [HttpPost]
    [Yes(Roles = "Administrator,Regular")]
    public async Task<QueryAllRolesResponse> QueryAllRoles(QueryAllRolesRequest request)
    {
        try
        {
            return await _manager.QueryAllRolesAsync(request);
        }
        catch (Exception _)
        {
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.QueryAllRoles.");
            throw ;
        }
    }
    
    [HttpPost]
    [Yes(Roles = "Administrator")]
    public async Task<QueryAllRolesResponse> QueryUserNotHasRole(QueryUserNotHasRoleRequest request)
    {
        try
        {
            return await _manager.QueryUserNotHasRoleAsync(request);
        }
        catch (Exception _)
        {
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.QueryUserNotHasRole.");
            throw ;
        }
    }
    
    [HttpPost]
    [Yes(Roles = "Administrator")]
    public async Task<QueryAllRolesResponse> QueryUserHasRole(QueryUserNotHasRoleRequest request)
    {
        try
        {
            return await _manager.QueryUserHaveRoleAsync(request);
        }
        catch (Exception _)
        {
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.QueryUserHasRole.");
            throw ;
        }
    }

    [HttpPost]
    [Yes(Roles = "Administrator")]
    public async Task UpdateRole(UpdateRoleRequest request)
    {
        try
        {
            await _manager.UpdateRoleAsync(request);
        }
        catch (Exception e)
        {
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.UpdateRole.");
            throw;
        }
    }
    
    
    [HttpPost]
    [Yes(Roles = "Administrator")]
    public async Task AddRole(AddRoleRequest request)
    {
        try
        {
            await _manager.AddRoleAsync(request);
        }
        catch (Exception _)
        {
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.AddRole.");
            throw ;
        }
    }
    
    
    [HttpPost]
    [Yes(Roles = "Administrator")]
    public async Task BatchDeleteRole(List<int> ids)
    {
        try
        {
            await _manager.BatchDeleteRoleAsync(ids);
        }
        catch (Exception _)
        {
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.BatchDeleteRole.");
            throw ;
        }
    }
}
