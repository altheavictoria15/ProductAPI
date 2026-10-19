using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProductAPI.DTOs;
using ProductAPI.Exceptions;
using ProductAPI.Models;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly List<Product> products = new()
        {
            new Product
            {
                ProductId = 1,
                Code = "P001",
                Name = "Bread",
                Description = "Freshly baked bread",
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

        // GET: api/products?page=1&pageSize=10
        [HttpGet]
        public IActionResult GetProducts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
            {
                var problem = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid pagination settings",
                    Detail = "Page must be at least 1, and pageSize must be between 1 and 100.",
                    Instance = Request.Path
                };

                problem.Extensions["traceId"] =
                    HttpContext.TraceIdentifier;

                return BadRequest(problem);
            }

            var totalItems = products.Count;
            var totalPages = (int)Math.Ceiling(
                totalItems / (double)pageSize);

            var pagedProducts = products
                .OrderBy(product => product.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new PagedResponse<Product>
            {
                Data = pagedProducts,
                Meta = new PaginationMeta
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    TotalPages = totalPages
                }
            };

            return Ok(response);
        }

        // GET: api/products/1
        [HttpGet("{id:int}")]
        public IActionResult GetProduct(int id)
        {
            var product = products.FirstOrDefault(
                p => p.ProductId == id);

            if (product == null)
            {
                throw new NotFoundException(
                    $"We couldn't find a product with ID {id}. " +
                    "Check the product ID and try again.");
            }

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public IActionResult CreateProduct(
            [FromBody] CreateProductRequest request)
        {
            var duplicateCode = products.Any(
                p => string.Equals(
                    p.Code,
                    request.Code,
                    StringComparison.OrdinalIgnoreCase));

            if (duplicateCode)
            {
                throw new ConflictException(
                    $"The product code '{request.Code}' is already in use. " +
                    "Please choose a different code.");
            }

            var newId = products.Count == 0
                ? 1
                : products.Max(p => p.ProductId) + 1;

            var product = new Product
            {
                ProductId = newId,
                Code = request.Code.Trim(),
                Name = request.Name.Trim(),
                Description = request.Description.Trim(),
                Price = request.Price
            };

            products.Add(product);

            return CreatedAtAction(
                nameof(GetProduct),
                new { id = product.ProductId },
                product);
        }

        // PUT: api/products/1
        [HttpPut("{id:int}")]
        public IActionResult UpdateProduct(
            int id,
            [FromBody] UpdateProductRequest request)
        {
            var product = products.FirstOrDefault(
                p => p.ProductId == id);

            if (product == null)
            {
                throw new NotFoundException(
                    $"We couldn't update product ID {id} because it doesn't exist. " +
                    "Check the product ID and try again.");
            }

            var duplicateCode = products.Any(
                p => p.ProductId != id &&
                     string.Equals(
                         p.Code,
                         request.Code,
                         StringComparison.OrdinalIgnoreCase));

            if (duplicateCode)
            {
                throw new ConflictException(
                    $"The product code '{request.Code}' belongs to another product. " +
                    "Please choose a different code.");
            }

            product.Code = request.Code.Trim();
            product.Name = request.Name.Trim();
            product.Description = request.Description.Trim();
            product.Price = request.Price;

            return Ok(product);
        }

        // DELETE: api/products/1
        [HttpDelete("{id:int}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = products.FirstOrDefault(
                p => p.ProductId == id);

            if (product == null)
            {
                throw new NotFoundException(
                    $"We couldn't delete product ID {id} because it doesn't exist. " +
                    "Check the product ID and try again.");
            }

            products.Remove(product);

            return NoContent();
        }
    }
}
