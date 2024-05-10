
using H6_ChicBotique.Database;
using H6_ChicBotique.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace H6_ChicBotique.Repositories
{
    //Functioname in respository should be Select,Create and update , delete
    //Creating Interface of IUserRepository
    public interface IUserRepository //Interface which declares only the methods
    {
        Task<List<User>> SelectAll();     //For getting all User Details 
        Task<User> SelectByEmail(string email); //For getting User by specific unique Email
        Task<User> SelectById(int userId); //For getting User by specific Id
        Task<User> Create(User user);//Creating a new user entity
        Task<User> Update(int userId, User user); //For Updating the User entity
        Task<User> UpdateWithHomeAddress(User user); //For Updating the User entity with homeaddress
        Task<User> Delete(int userId); //For Deleting the User Entity from the table
        Task<User> SelectUserWithHomeAddress(int userId);



    }
    // Implementation of IUserRepository interface in UserRepository class
    public class UserRepository : IUserRepository
    {
        private readonly ChicBotiqueDatabaseContext _context; // Instance of ChicBotiqueDatabaseContext class

        // Constructor with dependency injection
        public UserRepository(ChicBotiqueDatabaseContext context)
        {
            _context = context;
        }

        // Implementation of SelectAll method
        public async Task<List<User>> SelectAll()
        {
            // Retrieve all users from the database
            return await _context.User.Include(h => h.AccountInfo.HomeAddress).ToListAsync();
        }

        // Implementation of SelectById method
        public async Task<User> SelectById(int userId)
        {
            // Retrieve a specific user based on user ID
            return await _context.User.Include(u=>u.AccountInfo).FirstOrDefaultAsync(u => u.Id == userId);
        }

        // Implementation of SelectByEmail method
        public async Task<User> SelectByEmail(string email)
        {
            // Retrieve a specific user based on email address and also include user account information
            
             return await _context.User.Include(a => a.AccountInfo).ThenInclude(h => h.HomeAddress).FirstOrDefaultAsync(u => u.Email == email);
        }


        //Implementation of Create method for creating a new entity in the user table
        public async Task<User> Create(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        //Get user with homeaddress
        public async Task<User> SelectUserWithHomeAddress(int userId)
        {
            return await _context.User
                .Include(u => u.AccountInfo)
                .ThenInclude(ai => ai.HomeAddress)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }


        //Using this method existing user info can be updated by giving specific userId
        public async Task<User> UpdateWithHomeAddress(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }



         public async Task<User> Update(int user_Id, User user)
               {
                   User updateUser = await _context.User
                       .FirstOrDefaultAsync(a => a.Id == user_Id);

                   if (updateUser != null)
                   {
                       updateUser.Email = user.Email;
                       updateUser.FirstName = user.FirstName;
                       updateUser.LastName = user.LastName;
                       updateUser.Role = user.Role;
                       // Update AccountInfo if it exists
                       if (updateUser.AccountInfo != null)
                       {
                           updateUser.AccountInfo.HomeAddress.Address = user.AccountInfo.HomeAddress.Address;
                           updateUser.AccountInfo.HomeAddress.City = user.AccountInfo.HomeAddress.City;
                           updateUser.AccountInfo.HomeAddress.PostalCode = user.AccountInfo.HomeAddress.PostalCode;
                           updateUser.AccountInfo.HomeAddress.Country = user.AccountInfo.HomeAddress.Country;
                           updateUser.AccountInfo.HomeAddress.TelePhone = user.AccountInfo.HomeAddress.TelePhone;
                       }

                       // _context.Entry(updateUser).CurrentValues.SetValues(user);
                       await _context.SaveChangesAsync();
                   }
                   return updateUser;
               }
        public async Task<User> Delete(int user_id)
        {
            /* User obj = new User()
             {
                 Id = user_id
             };
             _context.Entry(obj).State = EntityState.Deleted;
             _context.SaveChanges();
             return obj;*/
            var user = _context.User.Include(u => u.AccountInfo).SingleOrDefault(u => u.Id == user_id);

            if (user != null)
            {

                if (user.AccountInfo != null)
                {

                    user.AccountInfo.UserId = null;

                }
                try
                {
                    _context.User.Remove(user);
                    await _context.SaveChangesAsync(); // Corrected method name
                    return user;
                }
                catch (DbUpdateException ex)
                {
                    // Log or handle the inner exception
                    var innerException = ex.InnerException;
                    // Handle the exception or log the details
                    throw; // Re-throw the exception if needed
                }
            }

            return null;
        }
    }
}