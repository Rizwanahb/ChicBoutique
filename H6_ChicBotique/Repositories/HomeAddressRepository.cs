using H6_ChicBotique.Database;
using H6_ChicBotique.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace H6_ChicBotique.Repositories
{
    public interface IHomeAddressRepository //Interface which declares the methods
    {
        Task<List<HomeAddress>> SelectAll(); //For getting all HomeAddresses Details
        Task<HomeAddress> Create(HomeAddress HomeAddress); //For creating a HomeAddress

        Task<HomeAddress> SelectById(int Id); //For getting a specific HomeAddressEnitity by specific Id

        Task<HomeAddress> Update(HomeAddress HomeAddress);  // For updating the HomeAddressEnitity entity

    }
    public class HomeAddressRepository:IHomeAddressRepository
    {
        private readonly ChicBotiqueDatabaseContext _context;  //making an instance of the class ChicBotiqueDatabaseContext

        public HomeAddressRepository(ChicBotiqueDatabaseContext context)   //dependency injection with parameter 
        {
            _context = context;
        }
        public async Task<List<HomeAddress>> SelectAll()
        {

            return await _context.HomeAddress.Include(e => e.AccountInfo)
                .ToListAsync();

        }

        //With this method one HomeAddress's info can be added
        public async Task<HomeAddress> Create(HomeAddress HomeAddress)
        {
            _context.HomeAddress.Add(HomeAddress);
            await _context.SaveChangesAsync();
            return HomeAddress;
        }

        //This method will get one specific HomeAddress info whoose HomeAddressId has been given 
        public async Task<HomeAddress> SelectById(int Id)
        {
            return await _context.HomeAddress.Include(a => a.AccountInfo).FirstOrDefaultAsync(u => u.Id == Id);
        }

        // For updating the HomeAddressEnitity entity
        public async Task<HomeAddress> Update(HomeAddress HomeAddress)
        {
            HomeAddress updateHomeAddress = await _context.HomeAddress
                .FirstOrDefaultAsync(a => a.Id == HomeAddress.Id);

            if (HomeAddress != null)
            {

                _context.Entry(updateHomeAddress).CurrentValues.SetValues(HomeAddress);  //update properties without the navigation properties(AccountInfo)
                await _context.SaveChangesAsync();
            }
           
            return updateHomeAddress;
        }


    }
}
