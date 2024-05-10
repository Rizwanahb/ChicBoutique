using H6_ChicBotique.Database;
using H6_ChicBotique.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace H6_ChicBotique.Repositories
{
    public interface IPasswordEntityRepository  //Interface which declares the methods
    {
        Task<List<PasswordEntity>> SelectAll(); //For getting all PasswordEntity Details
        Task<PasswordEntity> CreatePassword(PasswordEntity PasswordEntity); //For creating an PasswordEntity

        Task<PasswordEntity> SelectByUserId(int PasswordEntityId); //For getting PasswordEntity by specific Id

        Task<PasswordEntity> UpdatePassword(PasswordEntity PasswordEntity); // For updating the PasswordEntity entity

    }
    public class PasswordEntityRepository: IPasswordEntityRepository // This class is inheriting interfacae AccountInfoRepository and implement the interfaces
    {
        private readonly ChicBotiqueDatabaseContext _context;  //making an instance of the class ChicBotiqueDatabaseContext

        public PasswordEntityRepository(ChicBotiqueDatabaseContext context)   //dependency injection with parameter 
        {
            _context = context;
        }
        //**implementing the methods of IAuthorRepository interface**// 

        //This method will get all of the PasswordEntitys information 
        public async Task<List<PasswordEntity>> SelectAll()
        {

            return await _context.PasswordEntity.ToListAsync();

        }
        //With this method one PasswordEntity's info can be added
        public async Task<PasswordEntity> CreatePassword(PasswordEntity PasswordEntity)
        {
            _context.PasswordEntity.Add(PasswordEntity);
            await _context.SaveChangesAsync();
            return PasswordEntity;
        }
        //This method will get one specific PasswordEntity info whoose PasswordEntityId has been given 
        public async Task<PasswordEntity> SelectByUserId(int UserId)
        {

            return await _context.PasswordEntity.FirstOrDefaultAsync(u => u.UserId == UserId);

        }
        //Using this method existing PasswordEntity info can be updated by giving specific PasswordEntityId
        public async Task<PasswordEntity> UpdatePassword(PasswordEntity PasswordEntity)
        {
            PasswordEntity updatePasswordEntity = await _context.PasswordEntity
                .FirstOrDefaultAsync(a => a.PasswordId == PasswordEntity.PasswordId);

            if (PasswordEntity != null)
            {
                _context.Entry(updatePasswordEntity).CurrentValues.SetValues(PasswordEntity);  //update properties without the navigation properties(AccountInfo)
                await _context.SaveChangesAsync();
            }
            
            return PasswordEntity;
        }
    }

}
