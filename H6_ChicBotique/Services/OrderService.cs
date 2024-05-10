using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Repositories;

namespace H6_ChicBotique.Services
{
    public interface IOrderService
    {
        Task<List<OrderAndPaymentResponse>> GetAllOrders();

        Task<OrderAndPaymentResponse> GetOrderById(int orderId);
        Task<List<OrderAndPaymentResponse>> GetOrdersByAccountId(Guid acc_Id);
    
        Task<OrderAndPaymentResponse> CreateOrder(OrderAndPaymentRequest newOrder);
       
    }
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
      

        private readonly IShippingDetailsRepository _shippingDetailsRepository;
        // private readonly IStockHandlerService _stockHandlerService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IStockHandlerService _stockHandlerService;

        public OrderService(IOrderRepository orderRepository,
             IShippingDetailsRepository shippingDetailsRepository, IPaymentRepository paymentRepository,IStockHandlerService stockHandlerService)//IStockHandlerService stockHandlerService
        {
            _orderRepository = orderRepository;
           
            _shippingDetailsRepository = shippingDetailsRepository;
            _paymentRepository=paymentRepository;
            _stockHandlerService=stockHandlerService;
        }


        public async Task<List<OrderAndPaymentResponse>> GetAllOrders()
        {
            List<Order> orders = await _orderRepository.SelectAllOrders();

            if (orders != null)
            {
                return orders.Select(order => MapOrderToOrderResponse(order)).ToList();

            }

            return null;
        }



        public async Task<OrderAndPaymentResponse> GetOrderById(int orderId)
        {
            Order order = await _orderRepository.SelectOrderById(orderId);

            if (order != null)


            {

                return MapOrderToOrderResponse(order);
            }
            return null;
        }
        public async Task<OrderAndPaymentResponse> CreateOrder(OrderAndPaymentRequest newOrderRequest) //Add basketid to the controller and service call
        {
            Order newOrder = MapOrderRequestToOrder(newOrderRequest);
            

            Order insertOrder = await _orderRepository.CreateNewOrder(newOrder);
            
          //await _paymentRepository.CreatePayment(newPayment); //make sure to check and subscribe on paypals payment success notfications before inserting a payment cause it can fail or not have succeeded yet
            //await _stockHandlerService.ReservationSuccess(basketId); 

            if (insertOrder != null)
            {

                await _stockHandlerService.ReservationSuccess(newOrderRequest.ClientBasketId); 
                Order orderWithIncludes = await _orderRepository.SelectOrderById(insertOrder.Id);


                return MapOrderToOrderResponse(orderWithIncludes);


            }
            return null;
        }
        private Payment MapPaymentRequest(OrderAndPaymentRequest newOrder)
        {
            return new Payment()
            {
                Status = newOrder.Status,
                TransactionId = newOrder.TransactionId,
                Amount = newOrder.Amount
            };
        }



        private Order MapOrderRequestToOrder(OrderAndPaymentRequest newOrder)
        {
            return new Order()
            {
                OrderDate = DateTime.Now,
                AccountInfoId =newOrder.AccountInfoId,

                OrderDetails = newOrder.OrderDetails.Select(x => new OrderDetails
                {
                    ProductId = x.ProductId,
                    ProductTitle = x.ProductTitle,
                    ProductPrice = x.ProductPrice,
                    Quantity = x.Quantity

                }).ToList(),

                ShippingDetails = new ShippingDetails()

                {

                    Address=newOrder.shippingDetails.Address,
                    City=newOrder.shippingDetails.City,
                    Country=newOrder.shippingDetails.Country,
                    PostalCode=newOrder.shippingDetails.PostalCode,
                    Phone=newOrder.shippingDetails.Phone

                },
                Payment =new Payment()
                {
                    Status = newOrder.Status,
                    TransactionId = newOrder.TransactionId,
                    Amount = newOrder.Amount,
                    PaymentMethod = newOrder.PaymentMethod,
                    TimePaid = newOrder.TimePaid
                }

            };
        }

        private OrderAndPaymentResponse MapOrderToOrderResponse(Order order)
        {
            return new OrderAndPaymentResponse
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                AccountInfoId = order.AccountInfoId,
                Status=order.Payment.Status,
                TransactionId =order.Payment.TransactionId,
                PaymentMethod=order.Payment.PaymentMethod,
                Amount=order.Payment.Amount,
                Account = new AccountInfoResponse()
                {
                    Id = order.AccountInfo.Id,
                    CreatedDate = order.AccountInfo.CreatedDate,
                    UserId = order.AccountInfo.UserId
                  
                },
                ShippingDetails= new ShippingDetailsResponse()
                {
                    OrderId = order.Id,
                    Id= order.ShippingDetails.Id,
                    Address =order.ShippingDetails.Address,
                    City    =   order.ShippingDetails.City,
                    Country = order.ShippingDetails.Country,
                    PostalCode = order.ShippingDetails.PostalCode,
                    Phone = order.ShippingDetails.Phone


                },
                OrderDetails = order.OrderDetails.Select(orderDetail => new OrderDetailsResponse
                {
                    Id = order.Id,
                    ProductId = orderDetail.ProductId,
                    ProductTitle = orderDetail.ProductTitle,
                    ProductPrice = orderDetail.ProductPrice,
                    Quantity = orderDetail.Quantity

                }).ToList()

            };


        }



        public async Task<List<OrderAndPaymentResponse>> GetOrdersByAccountId(Guid acc_Id)
        {
            List<Order> orders = await _orderRepository.SelectOrdersByAccountInfoId(acc_Id);

            if (orders != null)
            {
                List<OrderAndPaymentResponse> responses = orders.Select(x => MapOrderToOrderResponse(x)).ToList();
                return responses;
            }
            return null;
        }
    }
}
