using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order_Redis_Task.DTOs;
using Order_Redis_Task.Services;

namespace Order_Redis_Task.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(
    IOrderService orderService,
    ILogger<OrdersController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        try
        {
            var response = await orderService.CreateOrderAsync(request);
            return CreatedAtAction(nameof(GetOrder), new { id = response.OrderId }, response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating order");
            return StatusCode(500, "An error occurred while creating the order");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetOrder(Guid id)
    {
        try
        {
            var order = await orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting order: {OrderId}", id);
            return StatusCode(500, "An error occurred while retrieving the order");
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetAllOrders()
    {
        try
        {
            var orders = await orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all orders");
            return StatusCode(500, "An error occurred while retrieving orders");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        try
        {
            var deleted = await orderService.DeleteOrderAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting order: {OrderId}", id);
            return StatusCode(500, "An error occurred while deleting the order");
        }
    }
}
