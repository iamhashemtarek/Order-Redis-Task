namespace Order_Redis_Task.DTOs;

public record CreateOrderRequest(
    string CustomerName,
    string Product,
    decimal Amount
);