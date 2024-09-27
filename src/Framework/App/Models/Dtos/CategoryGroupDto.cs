namespace Framework.App.Models.Dtos;

public class CategoryGroupDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    
    // public long? ParentCategoryId { get; set; }

    public List<CategoryGroupDto> Children { get; set; }
}