using Framework.Core.Models.Entities;

namespace Framework.App.Models.Entities;

public class ReviewReply : BaseEntity<long>
{
    public long ReviewId { get; set; }
}