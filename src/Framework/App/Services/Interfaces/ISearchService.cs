using Framework.App.Models.ViewModels;
using Framework.Core.Helpers.Pagination;

namespace Framework.App.Services.Interfaces;

public interface ISearchService
{
    Task<SearchData> SearchAsync(ListParams param, long[] categoryIds, long[] variationIds, long[] optionIds);
}