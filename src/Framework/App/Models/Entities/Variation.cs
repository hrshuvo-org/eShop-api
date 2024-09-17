using Framework.Core.Models.Entities;

namespace Framework.App.Models.Entities;

public class Variation : BaseEntity<long>
{
    public long CategoryId { get; set; }
    public string Category { get; set; }
}