using Framework.Core.Models.Entities;

namespace Framework.App.Models.Entities;

public class Category : BaseEntity<long>
{
    public long ParentCategoryId { get; set; }
    public string ParentCategory { get; set; }
}