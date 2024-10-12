using Framework.Core.Models.Entities;

namespace Framework.App.Models.Entities;

public class Photo : BaseEntity<long>
{
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public string PublicId { get; set; }

    public long ProductItemId { get; set; }
}