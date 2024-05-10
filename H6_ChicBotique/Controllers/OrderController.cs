using H6_ChicBotique.DTOs;
using H6_ChicBotique.Services;
using Microsoft.AspNetCore.Mvc;

namespace H6_ChicBotique.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                List<OrderAndPaymentResponse> orderResponses = await _orderService.GetAllOrders();
                if (orderResponses == null)
                {
                    return Problem("Got no data, not even an empty list, this is unexpected");
                }
                if (orderResponses.Count == 0)
                {
                    return NoContent();
                }
                return Ok(orderResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);

            }


        }
        // https://localhost:5001/api/Product/derp
        [HttpGet("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderById([FromRoute] int orderId)
        {
            try
            {
                OrderAndPaymentResponse orderResponses = await _orderService.GetOrderById(orderId);

                if (orderResponses == null)
                {
                    return NotFound();
                }

                return Ok(orderResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }
        // https://localhost:5001/api/Product/derp

        [HttpGet("User/{accountInfoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrdersByAccountInfoId([FromRoute] Guid accountInfoId)
        {
            try
            {
                List<OrderAndPaymentResponse> orderResponse = await _orderService.GetOrdersByAccountId(accountInfoId);
                if (orderResponse == null)
                {
                    return Problem("Got no data, not even an empty list, this is unexpected");
                }
                if (orderResponse.Count == 0)
                {
                    return NoContent();
                }
                return Ok(orderResponse);
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
        public async Task<IActionResult> Create([FromBody] OrderAndPaymentRequest newOrderRequest)
        {
            try
            {


                OrderAndPaymentResponse orderResponse = await _orderService.CreateOrder(newOrderRequest);

                if (orderResponse == null)
                {
                    return NotFound();
                }

                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

    }
}
