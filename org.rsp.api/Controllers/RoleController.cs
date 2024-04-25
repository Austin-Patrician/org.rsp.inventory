using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<QueryAllRolesResponse> QueryAllRoles(QueryAllRolesRequest request)
    {
        try
        {
            return await _manager.QueryAllRolesAsync(request);
        }
        catch (Exception e)
        {
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.QueryAllRoles.");
            throw ;
        }
    }
    
    
    [HttpPost]
    [AllowAnonymous]
    public async Task AddRole(AddRoleRequest request)
    {
        try
        {
            await _manager.AddRoleAsync(request);
        }
        catch (Exception e)
        {
            _logger.LogError(
                $"Error in org.huage.Service.RoleController.RoleController.AddRole.");
            throw ;
        }
    }
    
}
