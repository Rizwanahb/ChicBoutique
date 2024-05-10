using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Helpers;
using H6_ChicBotique.Repositories;
using H6_ChicBotique.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace H6_ChicBotiqueTestProject.ServiceTests
{
    public class OrderServiceTests
    {
        private readonly OrderService _orderService;
        private readonly Mock<IOrderRepository> _mockOrderRepository = new();
        private readonly Mock<IShippingDetailsRepository> _mockShippingDetailsRepository = new();
        private readonly Mock<IPaymentRepository> _mockPaymentRepository = new();
        private readonly Mock<IStockHandlerService> _mockStockHandlerService = new();

        Guid acc1id = Guid.NewGuid();
        Guid acc2id = Guid.NewGuid();

        public OrderServiceTests()
        {
            // Initializing the OrderService with the mock order repository
            _orderService = new OrderService(_mockOrderRepository.Object, _mockShippingDetailsRepository.Object,
                _mockPaymentRepository.Object, _mockStockHandlerService.Object);
        }
        //Test for GetAllOrders method of service
        [Fact]
        public async void GetAllOrders_ShouldReturnListOfOrderResponses_WhenOrderExist()
        {
            // Arrange
            // Creating a list of order
            List<Order> orders = new List<Order>();
            List<OrderDetails> orderDetails = new List<OrderDetails>();
            orderDetails.Add(new OrderDetails
            {
                ProductId =1,
                ProductTitle = "asd",
                ProductPrice = 145,
                Quantity=1

            });
            ShippingDetails shippingDetails = new()
            {
                Address="zxc",
                City="cph",
                Country="dk",
                PostalCode="2200",
                Phone="12345678"

            };
           
            Payment payment = new()
            {
                Id=1,
                Status="paid",
                Amount=255,
                PaymentMethod="credit",
                TimePaid=DateTime.UtcNow,
                TransactionId="1234564321"
            };
            AccountInfo accountInfo = new()
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,
                
            };
            orders.Add(new Order
            {
                OrderDate = DateTime.Now,
                AccountInfoId =acc1id,
                Payment=payment,
                AccountInfo=accountInfo,
                ShippingDetails=shippingDetails,
                OrderDetails = orderDetails

            });
            // Setting up the mock accountInfo repository to return the list of accountInfo
            _mockOrderRepository
                .Setup(x => x.SelectAllOrders())
                .ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetAllOrders();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.IsType<List<OrderAndPaymentResponse>>(result);
        }
        [Fact]
        public async void GetOrderById_ShouldReturnOrderResponse_WhenOrderExists()
        {
            // Arrange
            int orderId = 1;
            List<OrderDetails> orderDetails = new List<OrderDetails>();
            orderDetails.Add(new OrderDetails
            {
                ProductId =1,
                ProductTitle = "asd",
                ProductPrice = 145,
                Quantity=1

            });
            ShippingDetails shippingDetails = new()
            {
                Address="zxc",
                City="cph",
                Country="dk",
                PostalCode="2200",
                Phone="12345678"

            };

            Payment payment = new()
            {
                Id=1,
                Status="paid",
                Amount=255,
                PaymentMethod="credit",
                TimePaid=DateTime.UtcNow,
                TransactionId="1234564321"
            };
            AccountInfo accountInfo = new()
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,

            };
            Order order = new Order
            {
                Id=orderId,
                OrderDate = DateTime.Now,
                AccountInfoId =acc1id,
                Payment=payment,
                AccountInfo=accountInfo,
                ShippingDetails=shippingDetails,
                OrderDetails = orderDetails

            };
            _mockOrderRepository
                .Setup(x => x.SelectOrderById(It.IsAny<int>()))
                .ReturnsAsync(order);

            // Act
            var result = await _orderService.GetOrderById(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrderAndPaymentResponse>(result);
            Assert.Equal(order.Id, result.Id);
            Assert.Equal(order.AccountInfoId, result.AccountInfoId);
            Assert.Equal(order.Payment.TransactionId, result.TransactionId);

        }
        [Fact]
        public async void CreateOrder_ShouldReturnOrderResponse_WhenCreateIsSuccess()
        {
            // Arrange
            // Creating a new order request
            List<OrderDetailsRequest> orderDetailsRequest = new List<OrderDetailsRequest>();
            orderDetailsRequest.Add(new OrderDetailsRequest
            {
                ProductId =1,
                ProductTitle = "asd",
                ProductPrice = 145,
                Quantity=1

            });
            ShippingDetailsRequest shippingDetailsRequest = new()
            {
                Address="zxc",
                City="cph",
                Country="dk",
                PostalCode="2200",
                Phone="12345678"
            };
            OrderAndPaymentRequest newOrderRequest = new OrderAndPaymentRequest
            {
                OrderDate =DateTime.Now,
                AccountInfoId =acc1id,
                ClientBasketId="a520b23c-7065-4f8f-845e-6719071e8599",
                Status ="Success",
                TransactionId="3254123654785",
                PaymentMethod="card",
                Amount=100,
                TimePaid=DateTime.Now,
                OrderDetails=orderDetailsRequest,
                shippingDetails=shippingDetailsRequest,




            };
            _mockStockHandlerService.Setup(x => x.ReservationSuccess(It.IsAny<string>()))
                .ReturnsAsync(true);
            // Creating a created order with existing values
            AccountInfo newAccountInfo = new()
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,
                UserId =1

            };
            List<OrderDetails> orderDetails = new List<OrderDetails>();
            orderDetails.Add(new OrderDetails
            {
                ProductId =1,
                ProductTitle = "asd",
                ProductPrice = 145,
                Quantity=1

            });
            ShippingDetails newShippingDetails = new()
            {
                Id=1,
                Address="zxc",
                City="cph",
                Country="dk",
                PostalCode="2200",
                Phone="12345678"

            };
            Payment newPayment = new()
            {
                Id=1,
                Status="paid",
                Amount=255,
                PaymentMethod="credit",
                TimePaid=DateTime.UtcNow,
                TransactionId="1234564321"
            };
            int orderId = 1;
            Order createdOrder = new Order
            {
                Id = orderId,
                OrderDate =DateTime.Now,
                AccountInfoId =acc1id,
                AccountInfo=newAccountInfo,
                ShippingDetails=newShippingDetails,
                Payment=newPayment,
                OrderDetails=orderDetails
            };

            // Setting up the mock order repository to return the created order
            _mockOrderRepository
                .Setup(x => x.CreateNewOrder(It.IsAny<Order>()))
                .ReturnsAsync(createdOrder);
            ShippingDetailsResponse shippingDetailsResponse = new()
            {
                Id=1,
                OrderId=1,
                Address="zxc",
                City="cph",
                Country="dk",
                PostalCode="2200",
                Phone="12345678"

            };


            AccountInfoResponse accountInfoResponse = new()
            {
                Id = acc1id,
                CreatedDate = DateTime.UtcNow,

            };

            List<OrderDetailsResponse> orderDetailsResponse = new List<OrderDetailsResponse>();

            orderDetailsResponse.Add(new OrderDetailsResponse
            {
                ProductId =1,
                ProductTitle = "asd",
                ProductPrice = 145,
                Quantity=1

            });



            var ClientBasketId = "a520b23c-7065-4f8f-845e-6719071e8599";
           
            OrderAndPaymentResponse newOrderResponse = new OrderAndPaymentResponse
            {
                Id=orderId,
                OrderDate =DateTime.Now,
                AccountInfoId =acc1id,

                Status ="Success",
                TransactionId="3254123654785",
                PaymentMethod="card",
                Amount=100,
                OrderDetails=orderDetailsResponse,
                ShippingDetails=shippingDetailsResponse,
                Account=accountInfoResponse




            };
            
            // Act
            var result = await _orderService.CreateOrder(newOrderRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OrderAndPaymentResponse>(result);
            Assert.Equal(orderId, result.Id);
            Assert.Equal(newOrderResponse.AccountInfoId, result.AccountInfoId);
        }


        


    }

}


