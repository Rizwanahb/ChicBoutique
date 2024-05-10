using H6_ChicBotique.DTOs;
using Microsoft.AspNetCore.Mvc;
using H6_ChicBotique.Services;

namespace H6_ChicBotique.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService; //Creating an instance of IProductService
        private readonly IStockHandlerService _stockHandlerService;
        public ProductController(IProductService productService, IStockHandlerService stockHandlerService) ///Dependency injection of IProductService
        {
            _productService = productService;
            _stockHandlerService=stockHandlerService;
        }

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //For getting all Product details
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<ProductResponse> productResponses = await _productService.GetAllProducts(); ///calling GetAllProducts method
                                                                                                  //by the instance of IProductService
                if (productResponses == null)
                {
                    return Problem("Got no data, not even an empty list, this is unexpected");
                }
                if (productResponses.Count == 0)
                {
                    return NoContent();
                }
                return Ok(productResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);

            }


        }

        [HttpGet("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //Get one specific entity's details by productId
        public async Task<IActionResult> GetById([FromRoute] int productId)
        {
            // Get a product by ID
            try
            {
                ProductResponse productResponse = await _productService.GetProductById(productId); //calling GetProductById method
                                                                                                   //by the instance of IProductService

                if (productResponse == null)
                {
                    return NotFound();
                }

                return Ok(productResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("Category/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetProductsByCategoryId([FromRoute] int categoryId)
        {
            // Get products by category ID
            try
            {
                List<ProductResponse> productResponse = await _productService.GetProductsByCategoryId(categoryId); //calling GetProductsByCategoryId method
                                                                                                                   //by the instance of IProductService

                if (productResponse == null)
                {
                    return Problem("Got no data, not even an empty list, this is unexpected");
                }

                if (productResponse.Count == 0)
                {
                    return NoContent();
                }

                return Ok(productResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ProductRequest newProduct)
        {
            //Creating Product by Admin with the ProductRequest
            try
            {
                ProductResponse productResponse = await _productService.CreateProduct(newProduct); //calling ProductCategory method
                                                                                                   //by the instance of IProductService

                if (productResponse == null)
                {
                    return NotFound();
                }

                return Ok(productResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] int productId, [FromBody] ProductRequest updateProduct)
        {
            // Update an existing product by categoryrequest and categoryId
            try
            {
                ProductResponse productResponse = await _productService.UpdateProduct(productId, updateProduct); //calling updateProduct method
                                                                                                                 //by the instance of IProductService for updating the Product entity

                if (productResponse == null)
                {
                    return NotFound();
                }

                return Ok(productResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int productId)
        {
            // Delete a product by ID
            try
            {
                ProductResponse productResponse = await _productService.DeleteProduct(productId); //calling deleteProduct method
                                                                                                  //by the instance of IProductservice for deleting the product entity

                if (productResponse == null)
                {
                    return NotFound();
                }

                return Ok(productResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpGet("stock/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductStockbyId([FromRoute] int productId)
        {
            try
            {

                var getProductAvailableStock = await _stockHandlerService.GetAvailableStock(productId);

               /* if (getProductAvailableStock <1)
                {
                    return NotFound();
                }*/

                return Ok(getProductAvailableStock);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }
        [HttpPost("ReserveStock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReserveStock([FromBody] ReserveStockRequest reserveStock)
        {
            try
            {
                bool result = await _stockHandlerService.ReserveStock(reserveStock);

                if (result == false)
                {
                    return Ok(false);
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }
        [HttpPut("ReservationSuccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReservationSuccess([FromBody] string clientBasketId)
        {
            try
            {
                bool result = await _stockHandlerService.ReservationSuccess(clientBasketId);

                if (result == false)
                {
                    return Problem("Failure");
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }
    }
}
