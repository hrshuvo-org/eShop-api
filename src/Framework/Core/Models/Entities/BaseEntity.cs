namespace Framework.Core.Models.Entities;

public class BaseEntity<T>: IBaseEntity<T>
{
    public T Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedTime { get; set; } = DateTime.UtcNow;
    public int Status { get; set; }
}