namespace Framework.Core.Models;

public class EntityStatus
{
    public static int Deleted => -404;
    public static int Inactive => -1;
    public static int Active => 0;
}