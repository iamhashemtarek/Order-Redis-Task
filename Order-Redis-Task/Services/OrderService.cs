using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order_Redis_Task.Data;
using Order_Redis_Task.DTOs;
using Order_Redis_Task.Models;
using Order_Redis_Task.Utilities;

namespace Order_Redis_Task.Services;

public class OrderService(
    AppDbContext context,
    ICacheService cache,
    ILogger<OrderService> logger) : IOrderService
{
    public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
    {
        var order = new Order
        {
            OrderId = Guid.NewGuid(),
            CustomerName = request.CustomerName,
            Product = request.Product,
            Amount = request.Amount,
            CreatedAt = DateTime.UtcNow
        };

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        logger.LogInformation("Order created: {OrderId}", order.OrderId);

        return MapToResponse(order);
    }
    public async Task<OrderResponse?> GetOrderByIdAsync(Guid id)
    {
        var cacheKey = CacheUtitilty.GetOrderCacheKey(id);

        var cachedOrder = await cache.GetAsync<OrderResponse>(cacheKey);
        if (cachedOrder != null)
        {
            logger.LogInformation("Order retrieved from cache: {OrderId}", id);
            return cachedOrder;
        }

        var order = await context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null)
        {
            logger.LogWarning("Order not found: {OrderId}", id);
            return null;
        }

        var response = MapToResponse(order);

        await cache.SetAsync(cacheKey, response, CacheUtitilty.CacheTtl);
        logger.LogInformation("Order retrieved from database and cached: {OrderId}", id);

        return response;
    }
    public async Task<List<OrderResponse>> GetAllOrdersAsync()
    {
        var orders = await context.Orders
            .AsNoTracking()
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        logger.LogInformation("Retrieved {Count} orders", orders.Count);

        return orders.Select(MapToResponse).ToList();
    }
    public async Task<bool> DeleteOrderAsync(Guid id)
    {
        var order = await context.Orders.FindAsync(id);
        if (order == null)
        {
            logger.LogWarning("Order not found for deletion: {OrderId}", id);
            return false;
        }

        context.Orders.Remove(order);
        await context.SaveChangesAsync();

        var cacheKey = CacheUtitilty.GetOrderCacheKey(id);
        await cache.RemoveAsync(cacheKey);

        logger.LogInformation("Order deleted: {OrderId}", id);
        return true;
    }
    private static OrderResponse MapToResponse(Order order)
    {
        return new OrderResponse
        (
            order.OrderId,
            order.CustomerName,
            order.Product,
            order.Amount,
            order.CreatedAt
        );
    }
}