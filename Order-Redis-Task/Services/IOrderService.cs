using Order_Redis_Task.DTOs;

namespace Order_Redis_Task.Services;

public interface IOrderService
{
    Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request);
    Task<OrderResponse?> GetOrderByIdAsync(Guid id);
    Task<List<OrderResponse>> GetAllOrdersAsync();
    Task<bool> DeleteOrderAsync(Guid id);
}