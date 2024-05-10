
using H6_ChicBotique.Database;
using H6_ChicBotique.Database.Entities;
using Microsoft.EntityFrameworkCore;


namespace H6_ChicBotique.Repositories
//Should be Select,Create and update , delete
{
    public interface IAccountInfoRepository  //Interface which declares the methods
    {
        Task<List<AccountInfo>> SelectAll(); //For getting all AccountInfo Details
        Task<AccountInfo> Create(AccountInfo AccountInfo); //For creating an AccountInfo        
        Task<AccountInfo> SelectById(Guid Id); //For getting AccountInfo by specific Id        
        Task<AccountInfo> Update(AccountInfo Account); // For updating the AccountInfo entity
        Task<AccountInfo> Delete(Guid Account_Id); //Deleting the AccountInfo entity from the database
        Task<Guid> SelectGuidByUserId(int userId);  //Getting the GuidId by the userId
    }

    public class AccountInfoRepository : IAccountInfoRepository  // This class is inheriting interfacae AccountInfoRepository and implement the interfaces
    {
        private readonly ChicBotiqueDatabaseContext _context;         //making an instance of the class ChicBotiqueDatabaseContext

        public AccountInfoRepository(ChicBotiqueDatabaseContext context)         //dependency injection with parameter 
        {
            _context = context;
        }

     //Getting All AccountInfo details including HomeAddress
        public async Task<List<AccountInfo>> SelectAll()
        {

            return await _context.AccountInfo.Include(u => u.User)
                .Include(h => h.HomeAddress).ToListAsync();

        }

        //With this method one AccountInfo's info can be added
        public async Task<AccountInfo> Create(AccountInfo Account)
        {
            _context.AccountInfo.Add(Account);
            await _context.SaveChangesAsync();
            return Account;
        }

        //This method will get one specific AccountInfo info whoose AccountId has been given 
        public async Task<AccountInfo> SelectById(Guid accId)
        {
            return await _context.AccountInfo.Include(s => s.HomeAddress).Include(u => u.User).FirstOrDefaultAsync(u => u.Id == accId);
        }

        // For updating the  AccountInfo entity
        public async Task<AccountInfo> Update(AccountInfo Account)
        {
            AccountInfo updateAccount = await _context.AccountInfo
                .FirstOrDefaultAsync(a => a.Id == Account.Id);

            if (Account != null)
            {
                

                _context.Update(updateAccount);
                await _context.SaveChangesAsync();
            }
           
            return Account;
        }



        //This method will remove all the details of one AccountInfo Entity by AccountID
        public async Task<AccountInfo> Delete(Guid Account_Id)
        {
            AccountInfo deleteAccount = await _context.AccountInfo
                .FirstOrDefaultAsync(u => u.Id == Account_Id);

            if (deleteAccount != null)
            {
                _context.AccountInfo.Remove(deleteAccount);
                await _context.SaveChangesAsync();
            }
            return deleteAccount;
        }

        //This method is developed for getting the GuidId by the UserId
        public async Task<Guid> SelectGuidByUserId(int userId)
        {
            var acc = await _context.AccountInfo.SingleOrDefaultAsync(a => a.UserId == userId);

            if (acc == null)
            {
                return Guid.Empty; // User not found
            }

            return acc.Id;
        }
    }
}
