using System.ComponentModel.DataAnnotations;
using org.rsp.entity.Common;

namespace org.rsp.entity.Request;

public class AddUserRequest: BaseFiled
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required]
    public string UserName { get; set; }
    
    /// <summary>
    /// 密码
    /// </summary>
    [Required]
    public string PassWord { get; set; }
    
    /// <summary>
    /// 手机号
    /// </summary>
    [Required]
    public string Phone { get; set; }
    
    /// <summary>
    /// 邮箱地址
    /// </summary>
    public string Email { get; set; }
}