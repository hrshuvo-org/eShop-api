namespace Framework.Core.Models.Dtos;

public class UserLoginDto
{
    public string Username { get; set; }
    public string Token { get; set; }
    public string Displayname { get; set; }
}

public class AppUserDetailsDto
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public int Status { get; set; }

    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedTime { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
}

public class UserWithRoleDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public int Status { get; set; }

    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedTime { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;

    public List<RoleDto> Roles { get; set; }
}