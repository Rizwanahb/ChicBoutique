using H6_ChicBotique.DTOs;
using H6_ChicBotique.Repositories;

namespace H6_ChicBotique.Services
{
    public interface IStockHandlerService
    {


        Task<int> GetAvailableStock(int productId);
        Task<bool> ReserveStock(ReserveStockRequest reserveStock);
        Task<bool> ReservationSuccess(string clientBasketId);

    }
    public class StockHandlerService : IStockHandlerService
    {
      

        private readonly IServiceProvider _serviceProvider;
        public Dictionary<string, List<Tuple<int, int>>> ClientStockEntries = new(); // save an entry with the client unique id that has been set in cookie with the productid and the amount of wanted stock

        public StockHandlerService(IServiceProvider serviceProvider)
        {

            _serviceProvider=serviceProvider;
        }

        public async Task<int> GetAvailableStock(int productId) //find stock and reservations calculate available remainder
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var transientService = scope.ServiceProvider.GetRequiredService<IProductRepository>();

                var product = await transientService.SelectProductById(productId); // get the product such that you can check the actual stored stock
                List<Tuple<int, int>> productEntries = new List<Tuple<int, int>>(); //create a list of tuples to represent the values of existing tuples in the clientstockentries that hold the same product
                foreach (var clientEntries in ClientStockEntries.Values) //loop the clientstockentries values to find the values that contain the same product and append their tuples to the previously instantiated list
                {
                    var productTuple = clientEntries.FirstOrDefault(tuple => tuple.Item1 == productId);
                    if (productTuple!=null)
                    {
                        productEntries.Add(productTuple);
                    }
                }
                var alreadyReserved = productEntries.Select(e => e.Item2).Sum(); //get the sum of reserved stock that all clients have in their entries
                return product.Stock-alreadyReserved;
            }

        }
        public async Task<bool> ReserveStock(ReserveStockRequest reserveStock) //To reserve the stock for the clients requested products and registers the basket for tracking
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var transientService = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                    var product = await transientService.SelectProductById(reserveStock.productId); // get the product such that you can check the actual stored stock

                    if ((product.Stock - (await GetAvailableStock(reserveStock.productId))+ reserveStock.amountToReserve) >= 0) // check whether or not the reserved amount plus the amount to reserve is available in the total stock of the product meaning that the remainder should be 0 or more.
                    {
                        List<Tuple<int, int>> tuples; //create an empty list of tuples which is used later to instantiate a new list within the clientstockentries for the specific client
                        var clientListDoesNotExist = !ClientStockEntries.TryGetValue(reserveStock.clientBasketId, out tuples); // check whether or not the client already has a list of entries and if they do put it into the tuples list
                        if (clientListDoesNotExist) //create new values for the client
                        {
                            tuples = new List<Tuple<int, int>>();
                            tuples.Add(new(reserveStock.productId, reserveStock.amountToReserve));
                            ClientStockEntries.Add(reserveStock.clientBasketId, tuples);
                        }
                        else //find already existing values and replace them with the new values
                        {
                            var existingTuple = tuples.FirstOrDefault(tuple => tuple.Item1 ==   reserveStock.productId);
                            tuples.Remove(existingTuple);
                            tuples.Add(new(reserveStock.productId, reserveStock.amountToReserve));
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> ReservationSuccess(string clientBasketId) //this method updates the actual stock in the database and removes the clients basket session from tracking
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var transientService = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                var entryList = ClientStockEntries[clientBasketId];
                foreach (var entry in entryList)
                {
                    var productId = entry.Item1;
                    var product = await transientService.SelectProductById(productId);
                    product.Stock -= entry.Item2;
                    await transientService.UpdateExistingProduct(productId, product);
                }
                return true;
            }
        }
    }
}
