namespace Framework.Core.Models.Entities;

public interface IBaseEntity<T>
{
    public T Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime UpdatedTime { get; set; }
    public int Status { get; set; }
    
}