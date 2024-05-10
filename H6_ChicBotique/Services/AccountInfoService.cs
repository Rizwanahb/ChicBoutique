
using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Repositories;

namespace H6_ChicBotique.Services
{
    public interface IAccountInfoService  ////Interface which declares only the methods
    {
        Task<List<AccountInfoResponse>> GetAll(); //For getting all AccountInfo Details

        Task<AccountInfoResponse> GetById(Guid AccountId); ///For getting AccountInfo by specific Id
        Task<Guid> GetGuidIdByUserId(int userId); //For getting GuidUderId by specific UserId


    }
    public class AccountInfoService : IAccountInfoService // This class is inheriting interfacae AccountInfoRepository and implement the interfaces
    {
        //making an instance of the class C

       

        private readonly IAccountInfoRepository _accountInfoRepository;



        // Implementation of IAccountInfoService interface in AccountService class

        public AccountInfoService(IAccountInfoRepository AccountInfoRepository)
        {
            

            _accountInfoRepository = AccountInfoRepository; // Constructor with dependency injection for IAccountInfoRepository


        }



        // Implementation of GetAll method
        public async Task<List<AccountInfoResponse>> GetAll()
        {
            // Retrieve all AccountInfo from the repository

            List<AccountInfo> Accounts = await _accountInfoRepository.SelectAll();


            return Accounts.Select(acc => MapAccountToAccountResponse(acc)).ToList();


        }


        // Implementation of GetById method
        public async Task<AccountInfoResponse> GetById(Guid AccountInfoId)
        {
            // Retrieve a specific AccountInfo by ID from the repository
            AccountInfo Account = await _accountInfoRepository.SelectById(AccountInfoId);
            // If the Account is not null, map the Account to a AccountInfoResponse object
            if (Account != null)
            {

                return MapAccountToAccountResponse(Account);
            }
            return null; // Return null if the Account is not found
        }

        //Getting GuidId by a specific userId
        public async Task<Guid> GetGuidIdByUserId(int userId)
        {
            var userGuid = await _accountInfoRepository.SelectGuidByUserId(userId);

            if (userGuid != null)
            {

                return userGuid;
            }
            return Guid.Empty;
        }


        // Private method to map a AccountInfo object to a AccountInfoResponse object
        private static AccountInfoResponse MapAccountToAccountResponse(AccountInfo Account)
        {
            if (Account == null)
            {
                throw new Exception("AccountInfo value was null");
            }
            var acc = Account == null ? null : new AccountInfoResponse
            {
                Id = Account.Id,
                UserId=Account.UserId,
                User=new UserResponse
                {
                    Id=Account.User.Id,
                    FirstName=Account.User.FirstName,
                    LastName=Account.User.LastName,
                    Email=Account.User.Email,
                    Role=Account.User.Role
                },
                CreatedDate = Account.CreatedDate,
                Orders = Account.Orders?.Select(Order => new OrderAndPaymentResponse
                {
                    Id = Order.Id,
                    OrderDate = Order.OrderDate,
                    AccountInfoId = Order.AccountInfo.Id,
                    Status=Order.Payment.Status,
                    TransactionId=Order.Payment.TransactionId,

                    //Amount=Order.Payment.Amount,
                    OrderDetails = Order.OrderDetails.Select(order => new OrderDetailsResponse
                    {
                        Id = order.Id,
                        ProductId = order.ProductId,
                        ProductTitle = order.ProductTitle,
                        ProductPrice = order.ProductPrice,
                        Quantity = order.Quantity


                    }).ToList()
                }).ToList(),

            };
            if (Account.HomeAddress != null)  //If HomeAddress is there in the database for the guest Account then map it
                                              //with the response otherwise it will give null
            {
                acc.HomeAddress = new HomeAddressResponse
                {
                    AccountId = Account.Id,
                    Id = Account.HomeAddress.Id,
                    Address = Account.HomeAddress.Address,
                    City = Account.HomeAddress.City,
                    PostalCode = Account.HomeAddress.PostalCode,
                    Country = Account.HomeAddress.Country,
                    Phone = Account.HomeAddress.TelePhone

                };
            }
            return acc;
        }
    }
}
