namespace Framework.App.Models.Dtos;

public class CategoryGroupDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    
    // public long? ParentCategoryId { get; set; }

    public List<CategoryGroupDto> Children { get; set; }
}
public class CategorySelectDto
{
    public long Id { get; set; }
    public string Label { get; set; }
    public string Data { get; set; }
    public string Icon { get; set; } // only primeNG icon
    
    // public long? ParentCategoryId { get; set; }

    public List<CategorySelectDto> Children { get; set; }
}