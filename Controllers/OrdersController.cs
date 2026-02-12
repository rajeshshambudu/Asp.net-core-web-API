using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Data;
using EcommerceAPI.Models;

namespace EcommerceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult CreateOrder(int userId)
    {
        var cart = _context.CartItems
            .Where(x => x.UserId == userId)
            .ToList();

        var total = cart.Sum(x =>
            _context.Products.First(p => p.Id == x.ProductId).Price * x.Quantity);

        var order = new Order
        {
            UserId = userId,
            TotalAmount = total
        };

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cart);
        _context.SaveChanges();

        return Ok(order);
    }
}
