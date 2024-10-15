using Framework.App.Models.Dtos;
using Framework.App.Models.Entities;
using Framework.Core.Helpers.Pagination;

namespace Framework.App.Models.ViewModels;

public class SearchData
{
    public Pagination<ProductItem> Result { get; set; }
    public List<Category> Categories { get; set; }
    public List<VariationDto> Variations { get; set; }
}