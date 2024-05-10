using Microsoft.AspNetCore.Mvc;
using H6_ChicBotique.Authorization;
using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Repositories;
using H6_ChicBotique.Helpers;
using Microsoft.EntityFrameworkCore;

namespace H6_ChicBotique.Services
{
    // Interface definition for user service
    public interface IUserService
    {
        // Task<List<UserResponse>> GetAllAsync(); // Method to retrieve all users as UserResponse objects
        Task<List<UserResponse>> GetAll(); // Method to retrieve all users as UserResponse objects
        Task<UserResponse> GetById(int UserId); // Method to retrieve a user by ID as a UserResponse object
        Task<PasswordEntityResponse> GetPasswordByUserId(int UserId);
        Task<UserResponse> GetIdByEmail(string email); // Method to retrieve a user by email as a UserResponse object
        Task<LoginResponse> Authenticate(LoginRequest login); // Method to authenticate a user based on the provided login credentials.
        Task<UserResponse> Register(UserRegisterRequest newUser);//To register a user
        Task<GuestResponse> Register_Guest(GuestRequest newGuest);//To create a user as Guest withour having password
        Task<UserResponse> Update(int UserId, UserUpdateRequest updateUser);//To update userprofile        
        Task<bool> UpdatePassword(PasswordEntityRequest passwordEntityRequest);//To change the password
    }

    // Implementation of IUserService interface in UserService class
    public class UserService : IUserService
    {
        // creating instances of Interfaces
        private readonly IUserRepository _userRepository;
        private readonly IPasswordEntityRepository _passwordEntityRepository;
        private readonly IHomeAddressRepository _homeAddressRepository;
        private readonly IAccountInfoRepository _accountInfoRepository;
        private readonly IJwtUtils _jwtUtils;


        // Constructor with dependency injection for IUserRepository
        public UserService(IUserRepository userRepository, IPasswordEntityRepository PasswordEntityRepository, IHomeAddressRepository homeAddressRepository, IAccountInfoRepository accountInfoRepository, IJwtUtils jwtUtils)
        {
            _userRepository = userRepository;
            _accountInfoRepository = accountInfoRepository;
            _jwtUtils = jwtUtils;
            _passwordEntityRepository = PasswordEntityRepository;
            _homeAddressRepository = homeAddressRepository;
        }



        // Implementation of GetAll method
        public async Task<List<UserResponse>> GetAll()
        {
            try
            {
                // Retrieve all users from the repository
                List<User> users = await _userRepository.SelectAll();

                // If users are not null, map each user to a UserResponse object with home address
                return users?.Select(u => MapUserToUserResponse(u)).ToList() ?? new List<UserResponse>();
            }
            catch (Exception ex)
            {
                // Handle exceptions or log them as needed
                // You may want to replace Exception with a more specific exception type
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<UserResponse>();
            }
        }



        // Implementation of GetById method
        public async Task<UserResponse> GetById(int UserId)
        {
            // Retrieve a specific user by ID from the repository
            User User = await _userRepository.SelectById(UserId);

            // If the user is not null, map the user to a UserResponse object
            if (User != null)
            {
                return MapUserToUserResponse(User);
            }

            return null; // Return null if the user is not found
        }


        // Implementation of GetIdByEmail method
        public async Task<UserResponse> GetIdByEmail(string email)
        {
            // Retrieve a specific user by email from the repository
            User User = await _userRepository.SelectByEmail(email);

            // If the user is not null, map the user to a UserResponse object
            if (User != null)
            {
                return MapUserToUserResponse(User);
            }

            return null; // Return null if the user is not found
        }

        //Implementation og Register method
        public async Task<UserResponse> Register(UserRegisterRequest newuser)
        {
            User user = await _userRepository.SelectByEmail(newuser.Email);
            AccountInfo acc = user?.AccountInfo;

            if (user == null)
            {
                user = new User
                {

                    FirstName = newuser.FirstName,
                    LastName = newuser.LastName,
                    Email = newuser.Email,
                    //Password = HashedPW,
                    //Salt = salt,
                    Role = Helpers.Role.Member// force all users created through Register, to Role.AccountInfo
                };

                user = await _userRepository.Create(user);
                acc = new()
                {
                    UserId = user.Id

                };
                acc = await _accountInfoRepository.Create(acc);

            }
            else
            {
                user.FirstName = newuser.FirstName;

                user.LastName = newuser.LastName;

                user.Email = newuser.Email;
                //Password = "No Need",
                user.Role = Helpers.Role.Member;// force all users created through Register, to Role.AccountInfo
                user = await _userRepository.Update(user.Id, user);

            }
            HomeAddress homeaddress = new HomeAddress()
            {
                AccountInfoId = acc.Id,
                Address = newuser.Address,
                City = newuser.City,
                PostalCode = newuser.PostalCode,
                Country = newuser.Country,
                TelePhone = newuser.Telephone
                //etc
            };
            homeaddress = await _homeAddressRepository.Create(homeaddress);
            var salt = PasswordHelpers.GenerateSalt();
            var HashedPW = Helpers.PasswordHelpers.HashPassword($"{newuser.Password}{salt}");
            PasswordEntity pwd = new()
            {
                UserId = user.Id,
                Password = HashedPW,
                Salt = salt,
                LastUpdated = DateTime.Now
            };
            pwd = await _passwordEntityRepository.CreatePassword(pwd);
            return MapUserToUserResponse(user);
        }

        //Implementation og Register_Guest method
        public async Task<GuestResponse> Register_Guest(GuestRequest newguest)
        {

            User user = new User();
            user.FirstName = newguest.FirstName;
            user.LastName = newguest.LastName;
            user.Email = newguest.Email;
            user.Role = Helpers.Role.Guest;
            user = await _userRepository.Create(user);
            AccountInfo acc = new()
            {
                UserId = user.Id
            };
            acc = await _accountInfoRepository.Create(acc);
            return MapGuestToGuestResponse(user);
        }

        //Implementation of Login method
        public async Task<LoginResponse> Authenticate(LoginRequest login)
        {
            // Retrieve user information from the UserRepository based on the provided email.
            User user = await _userRepository.SelectByEmail(login.Email);

            // Check if the user with the provided email exists.
            if (user == null)
            {
                return null; // User not found.
            }

            // Retrieve the stored password information (including salt) from the PasswordEntityRepository.
            PasswordEntity pwd = await _passwordEntityRepository.SelectByUserId(user.Id);

            // Validate the provided password against the stored hashed password.
            if (Helpers.PasswordHelpers.HashPassword($"{login.Password}{pwd.Salt}") == pwd.Password)
            {
                // If the passwords match, create a LoginResponse with user information and a JWT token.
                LoginResponse response = new LoginResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Role = user.Role,
                    Token = _jwtUtils.GenerateJwtToken(user)
                };
                return response; // Return the LoginResponse upon successful authentication.
            }

            return null; // Return null if the provided password doesn't match the stored hashed password.
        }


        //Get Password by userid method
        public async Task<PasswordEntityResponse> GetPasswordByUserId(int UserId)
        {
            PasswordEntity pwd = await _passwordEntityRepository.SelectByUserId(UserId);
            if (pwd != null)
            {

                PasswordEntityResponse pwdresponse = new PasswordEntityResponse
                {
                    Password = pwd.Password,
                    Salt = pwd.Salt,
                    LastUpdatedDate = DateTime.Now

                };
                return pwdresponse;
            }
            return null;
        }

        // update function for password
        public async Task<bool> UpdatePassword(PasswordEntityRequest passwordEntityRequest)
        {
            var salt = PasswordHelpers.GenerateSalt();  //making salt
            var HashedPW = Helpers.PasswordHelpers.HashPassword($"{passwordEntityRequest.Password}{salt}");///hashing the requested password with salt
            PasswordEntity pwd = await _passwordEntityRepository.SelectByUserId(passwordEntityRequest.UserId); ///getting the user by userId
            //putting the new hashed password, salt, date in the object
            pwd.Salt = salt;
            pwd.Password = HashedPW;
            pwd.LastUpdated = DateTime.UtcNow;
            // updating the password in the database
            await _passwordEntityRepository.UpdatePassword(pwd);
            return true;
        }


        // Updates user information for the specified UserId.
        public async Task<UserResponse> Update(int userId, UserUpdateRequest updatedUser)
        {
            // Retrieve the existing user from the repository
            User existingUser = await _userRepository.SelectUserWithHomeAddress(userId);

            if (existingUser == null)
            {
                return null; // Handle the case where the user doesn't exist
            }

            // Update User information
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Email = updatedUser.Email;

            // Update HomeAddress information
            if (existingUser.AccountInfo != null && existingUser.AccountInfo.HomeAddress != null)
            {
                existingUser.AccountInfo.HomeAddress.Address = updatedUser.Address;
                existingUser.AccountInfo.HomeAddress.City = updatedUser.City;
                existingUser.AccountInfo.HomeAddress.PostalCode = updatedUser.PostalCode;
                existingUser.AccountInfo.HomeAddress.Country = updatedUser.Country;
                existingUser.AccountInfo.HomeAddress.TelePhone = updatedUser.Telephone;
            }

            // Call the repository to update the user
            User updatedUserData = await _userRepository.UpdateWithHomeAddress(existingUser);

            // Return the updated user information
            return MapUserToUserResponse(updatedUserData);
        }



        /*  public async Task<UserResponse> Update(int UserId, UserRequest updateUser)
      {
          // Create a new User object with updated information from UserRequest.
          User user = new User
          {
              FirstName = updateUser.FirstName,
              LastName = updateUser.LastName,
              Email = updateUser.Email
          };

          // Perform the update operation in the UserRepository.
          user = await _userRepository.Update(UserId, user);

          // Return a UserResponse with the updated user information, or null if the update was unsuccessful.
          return user == null ? null : new UserResponse
          {
              Id = user.Id,
              FirstName = user.FirstName,
              LastName = user.LastName,
              Email = user.Email,
              Role = user.Role
          };
      }*/


        // Private method to map a User object to a UserResponse object
        private static UserResponse MapUserToUserResponse(User user)
        {
            UserResponse response = new UserResponse();

            // If the user is not null, map relevant properties to the UserResponse object
            if (user != null)
            {
                response.Id = user.Id;
                response.Email = user.Email;
                response.FirstName = user.FirstName;
                response.LastName = user.LastName;
                response.Role = user.Role;

                // Check if AccountInfo is not null before accessing its properties
                if (user.AccountInfo != null)
                {
                    // Map home address details
                    response.HomeAddress = new HomeAddressResponse
                    {
                        AccountId = user.AccountInfo.Id,
                        Id = user.AccountInfo.HomeAddress?.Id ?? 0,
                        Address = user.AccountInfo.HomeAddress?.Address,
                        City = user.AccountInfo.HomeAddress?.City,
                        PostalCode = user.AccountInfo.HomeAddress?.PostalCode,
                        Country = user.AccountInfo.HomeAddress?.Country,
                        Phone = user.AccountInfo.HomeAddress?.TelePhone
                    };

                    // Check if HomeAddress is not null before accessing AccountInfo's properties
                    if (user.AccountInfo.HomeAddress != null)
                    {
                        // Create an AccountInfoResponse object within UserResponse
                        response.AccountInfo = new AccountInfoResponse
                        {
                            Id = user.AccountInfo.Id,
                            // Include other properties as needed
                        };
                    }
                }
            }

            return response; // Return the mapped UserResponse object
        }






        private static GuestResponse MapGuestToGuestResponse(User user)
        {

            return user == null ? null : new GuestResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
            };

        }


    }
}





