using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Data;
using EcommerceAPI.Models;

namespace EcommerceAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly AppDbContext _context;

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult AddToCart(CartItem item)
    {
        _context.CartItems.Add(item);
        _context.SaveChanges();
        return Ok(item);
    }

    [HttpGet("{userId}")]
    public IActionResult GetCart(int userId)
    {
        var cart = _context.CartItems
            .Where(x => x.UserId == userId)
            .ToList();

        return Ok(cart);
    }
}
