using Framework.Core.Models.Entities;

namespace Framework.App.Models.Entities;

public class Review : BaseEntity<long>
{
    public long ProductId { get; set; }
    
    public int Rating { get; set; }

    public List<ReviewReply> ReplyList { get; set; }
}