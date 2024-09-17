using Framework.Core.Models.Entities;

namespace Framework.App.Models.Entities;

public class VariationOption:BaseEntity<long>
{
    public long VariationId { get; set; }
    public string Variation { get; set; }

    public string Value { get; set; }
}