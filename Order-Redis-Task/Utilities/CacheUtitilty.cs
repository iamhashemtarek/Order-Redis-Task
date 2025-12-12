namespace Order_Redis_Task.Utilities;

public static class CacheUtitilty
{
    public static string GetOrderCacheKey(Guid orderId) => $"order:{orderId}";
    public static TimeSpan CacheTtl => TimeSpan.FromMinutes(5);

}
