using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Services;

namespace ECommerceApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;

        public CartController(ICartService cartService, IAuthService authService)
        {
            _cartService = cartService;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            var sessionId = HttpContext.Session.Id;
            var cart = await _cartService.GetCartAsync(sessionId);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var sessionId = HttpContext.Session.Id;
            await _cartService.UpdateQuantityAsync(sessionId, productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var sessionId = HttpContext.Session.Id;
            await _cartService.RemoveFromCartAsync(sessionId, productId);
            TempData["Message"] = "Item removed from cart.";
            return RedirectToAction("Index");
        }

        // Dynamic routing based on user state
        [Route("Checkout")]
        public async Task<IActionResult> Checkout()
        {
            var sessionId = HttpContext.Session.Id;
            var isLoggedIn = await _authService.IsUserLoggedInAsync(sessionId);
            
            if (!isLoggedIn)
            {
                TempData["ReturnUrl"] = "/Checkout";
                return RedirectToAction("Login", "Account");
            }

            var cart = await _cartService.GetCartAsync(sessionId);
            if (!cart.Items.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            return View(cart);
        }

        [HttpPost]
        [Route("Checkout/Process")]
        public async Task<IActionResult> ProcessCheckout()
        {
            var sessionId = HttpContext.Session.Id;
            var isLoggedIn = await _authService.IsUserLoggedInAsync(sessionId);
            
            if (!isLoggedIn)
            {
                return RedirectToAction("Login", "Account");
            }

            // Process checkout logic here
            await _cartService.ClearCartAsync(sessionId);
            TempData["Success"] = "Order placed successfully!";
            return RedirectToAction("OrderConfirmation");
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}