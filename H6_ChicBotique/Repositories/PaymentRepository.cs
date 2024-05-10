using H6_ChicBotique.Database;
using H6_ChicBotique.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace H6_ChicBotique.Repositories
{
    public interface IPaymentRepository //Interface which declares the methods
    {
        Task<Payment> CreatePayment(Payment payment);  //Creating a new Payment entity
    }
    public class PaymentRepository:IPaymentRepository
    {
        private readonly ChicBotiqueDatabaseContext _context;  //making an instance of the class ChicBotiqueDatabaseContext

        public PaymentRepository(ChicBotiqueDatabaseContext context) //dependency injection with parameter
        {
            _context = context;
        }
        //With this method clients Payment's info can be added
        public async Task<Payment> CreatePayment(Payment payment)
        {
            _context.Payment.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }
    }
}
