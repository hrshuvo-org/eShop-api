using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Framework.Core.Models.Entities;

public class AppUser : IdentityUser<long>, IBaseEntity<long>
{
    #region Default

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override long Id { get; set; }

    public string Name { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime UpdatedTime { get; set; }
    public int Status { get; set; }

    #endregion

    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;

    public ICollection<AppUserRole> UserRoles { get; set; } = [];
}