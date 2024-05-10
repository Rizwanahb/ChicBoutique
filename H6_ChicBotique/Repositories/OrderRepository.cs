using H6_ChicBotique.Database;
using H6_ChicBotique.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace H6_ChicBotique.Repositories
{
    public interface IOrderRepository     //Interface which declares the methods
    {
        Task<List<Order>> SelectAllOrders();  //For getting all Orders with Order Details 
        Task<Order> SelectOrderById(int orderId); //For getting Order by specific Id
        Task<List<Order>> SelectOrdersByAccountInfoId(Guid AccountInfoId); ////For getting Orders by specific unique AccountInfoId
        Task<Order> CreateNewOrder(Order orderId); //Creating a new user entity
    }
    // Implementation of IOrderRepository interface in OrderRepository class
    public class OrderRepository:IOrderRepository
    {
        private readonly ChicBotiqueDatabaseContext _context; // Instance of ChicBotiqueDatabaseContext class

        public OrderRepository(ChicBotiqueDatabaseContext context) // Constructor with dependency injection
        {
            _context = context;
        }

        // Implementation of SelectAll method
        public async Task<List<Order>> SelectAllOrders()
        {
            try
            {
                // Retrieve all Orders from the database including AccountInfo, OrderDetails,
                // Payment and ShippingDetails
                return await _context.Order
                         .Include(o => o.AccountInfo)
                         .Include(o => o.OrderDetails)
                         .Include(s => s.ShippingDetails)
                         .Include(p => p.Payment)

                          .ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
        // Retrieve a specific user based on AccountId
        public async Task<List<Order>> SelectOrdersByAccountInfoId(Guid AccountInfoId)
        {
            try
            {
                return await _context.Order
                    .Include(o => o.AccountInfo)
                         .Include(o => o.OrderDetails).ThenInclude(x => x.Product).Where(c => c.AccountInfoId== AccountInfoId)
                         .Include(s => s.ShippingDetails)
                          .ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
        // Implementation of SelectOrderById method
        public async Task<Order> SelectOrderById(int orderId)
        {
            try
            {
                return await _context.Order
                    .Include(a => a.OrderDetails).ThenInclude(a => a.Product)
                    .Include(c => c.AccountInfo).ThenInclude(s => s.HomeAddress)
                    .Include(s => s.ShippingDetails)
                    .Include(p => p.Payment)
                    .FirstOrDefaultAsync(order => order.Id == orderId);
            }
            catch (Exception)
            {
                return null;
            }
        }
        //Implementation of Create method for creating a new entity in the Order table
        public async Task<Order> CreateNewOrder(Order order)
        {
            try
            {
                _context.Order.Add(order);
                await _context.SaveChangesAsync();
                return order;

            }
            catch (Exception err)
            {

                Console.Write(err.Message);
                return null;
            }



        }
    
      

    }
}
