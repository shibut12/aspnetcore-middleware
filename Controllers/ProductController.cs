using aspnetcore_middleware.Models;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_middleware.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController:ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Product product = new Product()
            {
                Id = 1,
                Name = "product 1",
                Quantity = 1,
                Price = 10
            };

            return Ok(product);
        }

        [HttpPost]
        public IActionResult Post(Product product){
            return Ok(product.Name);
        }
    }
}