namespace Order_Redis_Task.DTOs;

public record OrderResponse(
    Guid OrderId,
    string CustomerName,
    string Product,
    decimal Amount,
    DateTime CreatedAt
);
