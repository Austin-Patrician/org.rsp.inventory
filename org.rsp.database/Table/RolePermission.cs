﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace org.rsp.database.Table;

[Table("role_permission")]
public class RolePermission : BaseTable
{
    public int Id { get; set; }
    
    /// <summary>
    /// 角色id
    /// </summary>
    public int RoleId { get; set; }
    
    /// <summary>
    /// 权限id
    /// </summary>
    public int PermissionId { get; set; }
}