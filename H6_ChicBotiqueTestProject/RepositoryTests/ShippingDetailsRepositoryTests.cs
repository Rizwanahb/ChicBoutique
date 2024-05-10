using H6_ChicBotique.Database;
using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.Helpers;
using H6_ChicBotique.Repositories;
using Microsoft.EntityFrameworkCore;

namespace H6_ChicBotiqueTestProject.RepositoryTests
{
    public class ShippingDetailsRepositoryTests
    {
        private readonly DbContextOptions<ChicBotiqueDatabaseContext> _options;
        private readonly ChicBotiqueDatabaseContext _context;
        private readonly ShippingDetailsRepository _shippingDetailsRepository;
        public string salt = PasswordHelpers.GenerateSalt();
        public ShippingDetailsRepositoryTests()
        {
            // Setting up in-memory database options
            _options = new DbContextOptionsBuilder<ChicBotiqueDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ChicBotique")
                .Options;

            // Creating an instance of the context and repository
            _context = new(_options);
            _shippingDetailsRepository = new(_context);
        }
        // Test case for SelectAllShippingDetails method
        [Fact]
        public async void SelectAllShippingDetails_ShouldReturnListOfShippingDetails_WhenShippingDetailsExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Adding ShippingDetails to the in-memory database

            _context.ShippingDetails.Add(new ShippingDetails
            {
             
                Id = 1,
                OrderId=1,
                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                Phone = "+228415799"

            });
            _context.ShippingDetails.Add(new ShippingDetails
            {
                Id = 2,
                OrderId=2,
                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                Phone = "+228415799"
            });


            _context.Order.Add(new Order
            {

                Id = 1,
                OrderDate = DateTime.Now,
              

            });

            _context.Order.Add(new Order
            {

                Id = 2,
                OrderDate = DateTime.Now,


            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _shippingDetailsRepository.SelectAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ShippingDetails>>(result);
            Assert.Equal(2, result.Count);
        }

        // Test case for SelectShippingDetailsById method
        [Fact]
        public async void SelectShippingDetailsById_ShouldReturnShippingDetails_WhenShippingDetailsExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            int shippingDetailsId = 1;
            // Adding a HomeAddress to the in-memory database
            _context.ShippingDetails.Add(new ShippingDetails
            {
                Id = 1,
                OrderId=1,
                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                Phone = "+228415799"
            });
            _context.Order.Add(new Order
            {
                Id = 1,
                OrderDate = DateTime.Now,

            });


            await _context.SaveChangesAsync();

            // Act
            var result = await _shippingDetailsRepository.SelectById(shippingDetailsId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ShippingDetails>(result);
            Assert.Equal(shippingDetailsId, result.Id);
        }
        [Fact]
        public async void InsertNewShippingDetails_ShouldFailToAddShippingDetails_WhenShippingDetailsIdAlreadyExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Creating a HomeAddress with an existing Id
            ShippingDetails shippingDetails = new ShippingDetails
            {
                Id = 1,
                OrderId=1,
                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                Phone = "+228415799"
            };

            _context.ShippingDetails.Add(shippingDetails);
            await _context.SaveChangesAsync();

            // Act
            async Task action() => await _shippingDetailsRepository.Create(shippingDetails);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        // Test case for UpdateExistingShippingDetails method
        [Fact]
        public async void UpdateExistingShippingDetails_ShouldChangeValuesOnShippingDetails()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();
            int shippingDetailsId = 1;

            // Creating a new ShippingDetails and updating it
            ShippingDetails newShippingDetails = new ShippingDetails
            {
                Id = 1,
                OrderId=1,
                Address = "Husum",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                Phone = "+228415799"


            };
       


            _context.ShippingDetails.Add(newShippingDetails);
            await _context.SaveChangesAsync();

            ShippingDetails updateShippingDetails = new ShippingDetails
            {
                Id = 1,
                OrderId=1,
                Address = "cph",
                City = "Copenhagen",
                PostalCode = "2200",
                Country = "Danmark",
                Phone = "+228415799"
            };
            //AccountInfo updateaccountInfo = new AccountInfo
            //{
            //    Id = acc1id,
            //    CreatedDate = DateTime.UtcNow,
            //    UserId=1,


            //};

            // Act
            var result = await _shippingDetailsRepository.Update(updateShippingDetails);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ShippingDetails>(result);
            Assert.Equal(shippingDetailsId, result.Id);
            Assert.Equal(updateShippingDetails.Address, result.Address);
        }

    }
}
