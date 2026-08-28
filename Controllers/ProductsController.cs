using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static List<Product> products = new List<Product>
        {
            new Product
            {
                ProductId = 1,
                Code = "P001",
                Name = "Bread",
                Description = "Baked bread",
                Price = 200.00m
            },

            new Product
            {
                ProductId = 2,
                Code = "P002",
                Name = "Milk",
                Description = "Fresh milk",
                Price = 350.00m
            },

            new Product
            {
                ProductId = 3,
                Code = "P003",
                Name = "Rice",
                Description = "Jasmine rice",
                Price = 500.00m
            },

            new Product
            {
                ProductId = 4,
                Code = "P004",
                Name = "Eggs",
                Description = "Fresh eggs",
                Price = 250.00m
            },

            new Product
            {
                ProductId = 5,
                Code = "P005",
                Name = "Fish",
                Description = "Fresh fish",
                Price = 600.00m
            }
        };

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            product.ProductId = products.Count + 1;

            products.Add(product);

            return CreatedAtAction(
                nameof(GetProduct),
                new { id = product.ProductId },
                product
            );
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product updatedProduct)
        {
            var product = products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            product.Code = updatedProduct.Code;
            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            products.Remove(product);

            return NoContent();
        }
    }
}
