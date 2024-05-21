using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using org.rsp.database.Expressions;
using org.rsp.database.Table;
using org.rsp.entity.Common;
using org.rsp.entity.Helper;
using org.rsp.entity.Model;
using org.rsp.entity.Request;
using org.rsp.entity.Response;
using org.rsp.entity.service;
using org.rsp.management.Tools;
using org.rsp.management.Wrapper;

namespace org.rsp.management.Manager;

public class RoleManager : IRoleManager, ITransient
{
    private readonly IRepositoryWrapper _wrapper;
    private readonly ILogger<RoleManager> _logger;
    private readonly IMapper _mapper;

    public RoleManager(ILogger<RoleManager> logger, IMapper mapper,
        IRepositoryWrapper wrapper)
    {
        _logger = logger;
        _mapper = mapper;
        _wrapper = wrapper;
    }

    public async Task BatchDeleteRoleAsync(List<int> ids)
    {
        if (!ids.Any())
            return;

        //查询每一个角色是否有对应的active user，否则不可以删除
        var delList = await _wrapper.Role.FindByCondition(_ => ids.Contains(_.Id)).ToListAsync();
        if (!delList.Any())
            return;

        foreach (var role in delList)
        {
            var userRole = await _wrapper.UserRole.FindByCondition(_ => _.RoleId == role.Id && _.IsDeleted == false)
                .ToListAsync();
            if (!userRole.Any())
            {
                continue;
            }

            var count = await _wrapper.User
                .FindByCondition(_ => userRole.Select(_ => _.UserId).Contains(_.Id) && _.IsDeleted == false)
                .CountAsync();
            if (count > 0)
            {
                //如果有用户,则跳过删除这条
                continue;
            }

            role.IsDeleted = true;
            _wrapper.Role.Update(role);
        }

        await _wrapper.SaveChangeAsync();
    }

    public async Task UpdateRoleAsync(UpdateRoleRequest request)
    {
        var role = await _wrapper.Role.FindByCondition(_ => _.Id == request.Id).FirstOrDefaultAsync();

        if (role is null)
            return;

        //更新需要注意名字唯一
        if (!string.IsNullOrEmpty(request.Name) && !string.Equals(request.Name, role.Name))
        {
            var count = await _wrapper.Role.FindByCondition(_ => _.Name == request.Name).CountAsync();
            if (count > 0)
            {
                _logger.LogWarning("Role Name can't duplicated.");
                return;
            }

            role.Name = request.Name;
        }

        if (!string.Equals(request.Remark, role.Remark))
        {
            role.Remark = request.Remark;
        }

        role.UpdateTime = DateTime.Now;
        role.UpdateBy = request.UpdateBy;
        _wrapper.Role.Update(role);

        await _wrapper.SaveChangeAsync();
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
                return;

            request.UpdateBy = request.CreateBy;
            var role = _mapper.Map<Role>(request);
            _wrapper.Role.Create(role);
            await _wrapper.SaveChangeAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"AddRoleAsync error. {e.Message}");
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
#if DEBUG
            Console.WriteLine("This is QueryAllRolesAsync model.");
#endif
            Expression<Func<Role, bool>> expression = ExpressionExtension.True<Role>();
            if (!string.IsNullOrEmpty(request.RoleName))
            {
                expression = expression.And(p => p.Name.Contains(request.RoleName));
            }

            expression = expression.And(p => p.IsDeleted == false);

            //查询数据库
            var roles = await _wrapper.Role.FindByCondition(expression).OrderByDescending(_ => _.UpdateTime)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize).ToListAsync();

            if (roles.Any())
            {
                response.RoleModels = _mapper.Map<List<RoleModel>>(roles);
            }

            response.TotalCount = await _wrapper.Role.FindByCondition(expression).CountAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"QueryAllRolesAsync error: {e.Message}");
            throw;
        }

        return response;
    }

    public async Task<QueryAllRolesResponse> QueryUserHaveRoleAsync(QueryUserNotHasRoleRequest request)
    {
        var response = new QueryAllRolesResponse();

        try
        {
            //查询中间表
            var roles = await _wrapper.UserRole.FindByCondition(_ => _.UserId == request.userId)
                .OrderByDescending(_ => _.UpdateTime).Select(_ => _.RoleId).ToListAsync();

            if (!roles.Any())
                return response;

            var hasRole = await _wrapper.Role.FindByCondition(_ => roles.Contains(_.Id)).ToListAsync();

            response.RoleModels = _mapper.Map<List<RoleModel>>(hasRole);
        }
        catch (Exception e)
        {
            _logger.LogError($"QueryUserHaveRoleAsync error: {e.Message}");
            throw;
        }

        return response;
    }


    /// <summary>
    /// 查询用户没有的角色
    /// </summary>
    /// <param name="userId"></param>
    public async Task<QueryAllRolesResponse> QueryUserNotHasRoleAsync(QueryUserNotHasRoleRequest request)
    {
        var response = new QueryAllRolesResponse();

        try
        {
            //查询中间表
            var roles = await _wrapper.UserRole.FindByCondition(_ => _.UserId == request.userId)
                .OrderByDescending(_ => _.UpdateTime).Select(_ => _.RoleId).ToListAsync();
            //所有的role
            var rolesList = await _wrapper.Role.FindByCondition(_ => _.IsDeleted == false).ToListAsync();
            var valueFirst = rolesList.Select(_ => _.Id);

            var enumerable = valueFirst as int[] ?? valueFirst.ToArray();
            //获取该用户没有的角色名字
            var except = enumerable.Union(roles).Except(enumerable.Intersect(roles)).ToArray();

            var realRoles = rolesList.Where(_ => except.Contains(_.Id)).ToList();
            response.RoleModels = _mapper.Map<List<RoleModel>>(realRoles);
        }
        catch (Exception e)
        {
            _logger.LogError($"QueryAllRolesAsync error: {e.Message}");
            throw;
        }

        return response;
    }
}