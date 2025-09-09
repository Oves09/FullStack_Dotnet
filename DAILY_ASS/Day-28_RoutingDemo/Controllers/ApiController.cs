using Microsoft.AspNetCore.Mvc;

namespace Day_28RoutingDemo.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet("products")]
        public IActionResult GetProducts()
        {
            var products = new[]
            {
                new { Id = 1, Name = "Product 1", Price = 9.99 },
                new { Id = 2, Name = "Product 2", Price = 19.99 }
            };
            return Ok(products);
        }

        [HttpGet("products/{id:int}")]
        public IActionResult GetProduct(int id)
        {
            var product = new { Id = id, Name = $"Product {id}", Price = id == 1 ? 9.99 : 19.99 };
            return Ok(product);
        }

        // get status
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            var status = new { Message = "API is running", Timestamp = DateTime.UtcNow };
            return Ok(status);
        }

        // get version
        [HttpGet("version")]
        public IActionResult GetVersion()
        {
            var version = new { Version = "1.0.0", ReleaseDate = DateTime.UtcNow, };
            return Ok(version);
        }

        public IActionResult Version(string version)
        {
            var apiVersion = new
            {
                Version = version,
                SupportedFeatures = new[] { "Feature1", "Feature2" }
            };

            return Ok(apiVersion);
        }



    }
}