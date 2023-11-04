using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System.Net;

namespace Shopping.Aggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public ShoppingController(
            ICatalogService catalogService,
            IBasketService basketService,
            IOrderService orderService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
            _orderService = orderService;   
        }

        [HttpGet("{username}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string username)
        {
            // Get basket by username
            var basket = await _basketService.GetBasket(username);

            // interate basket items and consumes products with basket item product id member
            // map product related member into basket item model dto with extended column
            foreach (var basketItem in basket.Items)
            {
                var product = await _catalogService.GetCatalog(basketItem.ProductId);
                basketItem.ProductName = product.Name;
                basketItem.Category = product.Category;
                basketItem.Summary = product.Summary;
                basketItem.Description = product.Description;
                basketItem.ImageFile = product.ImageFile;   
            }

            // get orders information
            var orders = await _orderService.GetOrdersByUsername(username);

            var shoppingModel = new ShoppingModel
            {
                UserName = username,
                BasketWithProducts = basket,
                Orders = orders,
            };

            return shoppingModel;
        }



    }
}
