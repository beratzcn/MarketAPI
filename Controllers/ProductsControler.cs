namespace MarketAPI.Controllers
{
    using MarketAPI.Models;
    using MarketAPI.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;

        public ProductsController(
            IRepository<Product> productRepository,
            IRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _productRepository.GetAll();
            return Ok(products);
        }


        [HttpGet("categories")]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            var categories = _categoryRepository.GetAll();
            return Ok(categories);
        }


        [HttpGet("{id}", Name = "GetProduct")]
        public ActionResult<Product> Get(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        [HttpPost("categories")]
        public ActionResult<Category> PostCategory([FromBody] Category category)
        { 
            _categoryRepository.Add(category);
            return CreatedAtRoute("GetCategory", new { id = category.Id }, category);
        }

        
        [HttpPost]
        public ActionResult<Category> Post([FromBody] Product product, int categoryId)
        {
            var category = _categoryRepository.GetById(categoryId);
            var existingProductWithSameName = _productRepository.GetAll()
                .FirstOrDefault(p => p.Name == product.Name && p.CategoryId != category.Id);
            
            if (category == null)
            {
                return NotFound("Category not found.");
            }
            else if (product.Name == "string" || product.Name == "")
            {
                return BadRequest("Product name cannot be null.");
            }
            else if (product.StockQuantity <= 20 )
            {
                return BadRequest("Stock quantity must be at least 20.");
            }
            else if (existingProductWithSameName != null)
            {
                return BadRequest("You cannot add same product to a different category.");
            }
            else
                
                product.Category = category;

                _productRepository.Add(product);

                return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }
        
        
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            var existingProduct = _productRepository.GetById(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.categoryName=product.categoryName;

            _productRepository.Update(existingProduct);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepository.Delete(id);
            return NoContent();
        }

        
        [HttpGet("stock")]
        public ActionResult<IEnumerable<Product>> GetProductsByStock([FromQuery] int minStock, [FromQuery] int maxStock)
        {
            if (minStock < 0 || maxStock < 0 || minStock > maxStock)
            {
                return BadRequest("Invalid stock range.");
            }

            var products = _productRepository.GetAll()
                .Where(p => p.StockQuantity >= minStock && p.StockQuantity <= maxStock)
                .ToList();

            if (products.Count == 0)
            {
                return NotFound("No products were found within the specified stock range.");
            }

            return Ok(products);
        }

        
        [HttpGet("name")]
        public ActionResult<IEnumerable<Product>> GetProductsByName([FromQuery] string name)
        {
            var products = _productRepository.GetAll()
                .Where(p => p.Name == name)
                .ToList();
            
            if (products.Count == 0)
            {
                return NotFound("No products were found with " + name + " letter.");
            }
            return Ok(products);
        }


        [HttpGet("description")]
        public ActionResult<IEnumerable<Product>> GetProductsByDescription([FromQuery] string description)
        {
            var products = _productRepository.GetAll()
                .Where(p => p.Description == description)
                .ToList();
            if(products.Count == 0)
            {
                return NotFound("No products were found with " + description + " description.");
            }
            return Ok(products);
        }


        [HttpGet("category")]
        public ActionResult<IEnumerable<Product>> GetProductsByCategory([FromQuery] string category)
        {
            var products = _productRepository.GetAll()
                .Where(p => p.categoryName == category)
                .ToList();
            if (products.Count == 0)
            {
                return NotFound("No products were found with " + category + " description.");
            }
            return Ok(products);
        }
    }

}
