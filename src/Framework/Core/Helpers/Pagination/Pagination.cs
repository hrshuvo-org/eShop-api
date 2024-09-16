namespace Framework.Core.Helpers.Pagination;

public class Pagination<T> where T : class
{
    public Pagination(int pageIndex, int pageSize, int count, int total, IReadOnlyList<T> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
        TotalPage = total;
        Data = data;
    }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public int TotalPage { get; set; }
    public IReadOnlyList<T> Data { get; set; }
}