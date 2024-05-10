using H6_ChicBotique.Database;
using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.Helpers;
using H6_ChicBotique.Repositories;
using Microsoft.EntityFrameworkCore;

namespace H6_ChicBotiqueTestProject.RepositoryTests
{
    public class PaymentRepositoryTests
    {
        private readonly DbContextOptions<ChicBotiqueDatabaseContext> _options;
        private readonly ChicBotiqueDatabaseContext _context;
        private readonly PaymentRepository _paymentRepository;
        public string salt = PasswordHelpers.GenerateSalt();
        public PaymentRepositoryTests()
        {
            // Setting up in-memory database options
            _options = new DbContextOptionsBuilder<ChicBotiqueDatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ChicBotique")
                .Options;

            // Creating an instance of the context and repository
            _context = new(_options);
            _paymentRepository = new(_context);
        }
    
        //Test for CreatePayment method of Repository
        [Fact]
        public async void InsertNewPayment_ShouldFailToAddPayment_WhenPaymentIdAlreadyExists()
        {
            // Arrange 
            await _context.Database.EnsureDeletedAsync();

            // Creating a Payment with an existing Id
            Payment payment = new Payment
            {
                Id= 1,  
                Status="Paid",
                PaymentMethod="CreditCard",
                TransactionId="5377882222",
              
                Amount=200
               
            };

            _context.Payment.Add(payment);
            await _context.SaveChangesAsync();

            // Act
            async Task action() => await _paymentRepository.CreatePayment(payment);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }
        
    }
}
