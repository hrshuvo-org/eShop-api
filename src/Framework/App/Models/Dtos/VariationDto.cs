namespace Framework.App.Models.Dtos;

public class VariationDto
{
    public long Id { get; set; }
    public string Name { get; set; }

    public List<VariationOptionDto> VariationOptions { get; set; } = [];

}

public class VariationOptionDto
{
    public long Id { get; set; }
    public string Value { get; set; }
    
    public long VariationId { get; set; }
}