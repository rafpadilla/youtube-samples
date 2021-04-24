using DapperSample.Core.Entities;
using DapperSample.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DapperSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _productRepository;

        public ProductsController(ILogger<ProductsController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var products = await _productRepository.GetAll();
            return Ok(products);
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var products = await _productRepository.GetById(id);
            return Ok(products);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(Product product)
        {
            _ = await _productRepository.Create(product);
            return Ok();
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Product product)
        {
            var currentProduct = await _productRepository.GetById(product.Id);
            if (currentProduct == null)
            {
                return BadRequest("Product to update not found");
            }

            _ = await _productRepository.Update(product);
            return Ok();
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var currentProduct = await _productRepository.GetById(id);
            if (currentProduct == null)
            {
                return BadRequest("Product to delete not found");
            }

            _ = await _productRepository.Delete(id);
            return Ok();
        }
    }
}
