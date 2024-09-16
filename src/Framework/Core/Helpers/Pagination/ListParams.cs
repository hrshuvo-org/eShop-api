namespace Framework.Core.Helpers.Pagination;

public class ListParams : PaginationParams
{
    public string Query { get; set; }
    public int? Status { get; set; }
    public bool WithDeleted { get; set; }
}