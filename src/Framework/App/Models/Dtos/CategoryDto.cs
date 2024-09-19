namespace Framework.App.Models.Dtos;

public class CategoryDto
{
    public long? Id { get; set; }
    public string Name { get; set; }
    public long? ParentCategoryId { get; set; }
    public int Status { get; set; }

    public List<VariationDto> Variations { get; set; } = [];
}