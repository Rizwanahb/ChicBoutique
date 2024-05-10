using H6_ChicBotique.Controllers;
using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Helpers;
using H6_ChicBotique.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;


namespace H6_ChicBotiqueTestProject.ControllerTests
{

    /// Unit tests for the "AccountInfoController" class.     
    public class OrderControllerTests
    {
        private readonly OrderController _orderController;
        private readonly Mock<IOrderService> _mockOrderService = new();
        Guid acc1id = Guid.NewGuid();
        Guid acc2id = Guid.NewGuid();
        public OrderControllerTests()
            {
            // Initialize the HomeAddressController with a mocked IHomeAddressService
            _orderController = new OrderController(_mockOrderService.Object);
            }

            /// Verifies that GetAll returns StatusCode 200 when categories exist.       
       [Fact]
       public async void GetAll_ShouldReturnStatusCode200_WhenOrderExist()
       {
                // Arrange
            List<OrderAndPaymentResponse> order = new List<OrderAndPaymentResponse>
            {
                new OrderAndPaymentResponse 
                {
                    OrderDate = DateTime.Now,
                    AccountInfoId =acc1id,
                    Status="success",
                    PaymentMethod="card",
                    Amount=100,
                    


                },
               
            };

            _mockOrderService.Setup(x => x.GetAllOrders()).ReturnsAsync(order);

                // Act
                var result = await _orderController.GetAllOrders();

                // Assert
                var statusCodeResult = (IStatusCodeActionResult)result;
                Assert.Equal(200, statusCodeResult.StatusCode);
       }


        /// Verifies that GetById returns StatusCode 200 when order exists

        [Fact]
        public async void GetOrderById_ShouldReturnStatusCode200_WhenOrderExist()
        {

            // Arrange
         
            List<OrderDetailsResponse> orderDetails = new List<OrderDetailsResponse>();
            int orderId = 1;
            orderDetails.Add(new OrderDetailsResponse
            {
                ProductId =1,
                ProductTitle = "asd",
                ProductPrice = 145,
                Quantity=1

            });
            ShippingDetailsResponse shippingDetails = new()
            {
                Address="zxc",
                City="cph",
                Country="dk",
                PostalCode="2200",
                Phone="12345678"

            };

            
            AccountInfoResponse accountInfo = new()
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,

            };
            OrderAndPaymentResponse order = new OrderAndPaymentResponse
            {
                Id=orderId,
                OrderDate = DateTime.Now,
                AccountInfoId =acc1id,
                Status="paid",
                Amount=255,
                PaymentMethod="credit",
                
                TransactionId="1234564321",
                Account=accountInfo,
                ShippingDetails=shippingDetails,
                OrderDetails = orderDetails

            };
            _mockOrderService
                  .Setup(x => x.GetOrderById(It.IsAny<int>()))
                  .ReturnsAsync(order);

            // Act
            var result = await _orderController.GetOrderById(1);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);


        }
        [Fact]
        public async void Create_ShouldReturnStatusCode200_WhenOrderIsSuccessfullyCreated()
        {
            // Arrange
            int orderId = 1;
          
            List<OrderDetailsRequest> orderDetails = new List<OrderDetailsRequest>();
            orderDetails.Add(new OrderDetailsRequest
            {
                ProductId =1,
                ProductTitle = "asd",
                ProductPrice = 145,
                Quantity=1

            });
            List<OrderDetailsResponse> orderDetailsResponse = new List<OrderDetailsResponse>();
            orderDetailsResponse.Add(new OrderDetailsResponse
            {
                ProductId =1,
                ProductTitle = "asd",
                ProductPrice = 145,
                Quantity=1

            });
            ShippingDetailsRequest shippingDetails = new()
            {
                Address="zxc",
                City="cph",
                Country="dk",
                PostalCode="2200",
                Phone="12345678"

            };


            ShippingDetailsResponse shippingDetailsResponse = new()
            {
                Address="zxc",
                City="cph",
                Country="dk",
                PostalCode="2200",
                Phone="12345678"

            };

            OrderAndPaymentRequest newProduct = new OrderAndPaymentRequest
            {
             
                OrderDate = DateTime.Now,
                AccountInfoId =acc1id,
                Status="paid",
                Amount=255,
                PaymentMethod="credit",
                ClientBasketId="a520b23c-7065-4f8f-845e-6719071e8599",
                TimePaid   = DateTime.UtcNow,
                TransactionId="1234564321",

                shippingDetails=shippingDetails,
                OrderDetails = orderDetails

            };
            AccountInfoResponse newAccountInfo = new()
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,

            };
            // Setup the mock OrderService to return a order response
            OrderAndPaymentResponse orderResponse = new OrderAndPaymentResponse
            {
                Id=orderId,
                OrderDate = DateTime.Now,
                AccountInfoId =acc1id,
                Status="paid",
                Amount=255,
                PaymentMethod="credit",

                TransactionId="1234564321",
                Account=newAccountInfo,
                ShippingDetails=shippingDetailsResponse,
                OrderDetails = orderDetailsResponse

            };
            _mockOrderService.Setup(x => x.CreateOrder(It.IsAny<OrderAndPaymentRequest>())).ReturnsAsync(orderResponse);

            // Act
            var result = await _orderController.Create(newProduct);

            // Assert
            var statusCodeResult = (IStatusCodeActionResult)result;
            Assert.Equal(200, statusCodeResult.StatusCode);
        }
    }
}

