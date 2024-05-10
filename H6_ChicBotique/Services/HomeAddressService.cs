using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Repositories;

namespace H6_ChicBotique.Services
{
    public interface IHomeAddressService  // Interface definition for user service
    {
        Task<List<HomeAddressResponse>> GetAllHomeAddresses(); // Method to retrieve all HomeAddress as HomeAddressResponse objects
        Task<HomeAddressResponse> GetHomeAddressById(int HomeAddressId); // Method to retrieve HomeAddress of specific user by HomeAddressId
                                                                         // as a HomeAddressResponse object


        Task<HomeAddressResponse> UpdateHomeAddress(int HomeAddressId, HomeAddressRequest updateHomeAddress); // Method to update for HomeAddress of  a user 
        


    }
    public class HomeAddressService: IHomeAddressService // Implementation of IUserService interface in UserService class
    {
        private readonly IHomeAddressRepository _HomeAddressRepository;  //Creating instance of IHomeAddressRepository
        public HomeAddressService(IHomeAddressRepository HomeAddressRepository)// Constructor with dependency injection for IHomeAddressRepository
        {
            _HomeAddressRepository = HomeAddressRepository;
        }

        // Implementation of GetAllHomeAddresses method
        public async Task<List<HomeAddressResponse>> GetAllHomeAddresses()
        {
            // Retrieve all HomeAddresses from the repository
            List<HomeAddress> HomeAddresss = await _HomeAddressRepository.SelectAll();

            return HomeAddresss.Select(HomeAddress => MapHomeAddressToHomeAddressResponse(HomeAddress)).ToList();

        }


        // Implementation of GetById method
        public async Task<HomeAddressResponse> GetHomeAddressById(int HomeAddressId)
        {
            // Retrieve a specific HomeAddress by ID from the repository
            HomeAddress HomeAddress = await _HomeAddressRepository.SelectById(HomeAddressId);

            if (HomeAddress != null)
            {

                return MapHomeAddressToHomeAddressResponse(HomeAddress);
            }
            return null;
        }



        //Implementing the UpdateHomeAddress function
        public async Task<HomeAddressResponse> UpdateHomeAddress(int HomeAddressId, HomeAddressRequest updateHomeAddress)
        {
            //
            HomeAddress local = await _HomeAddressRepository.SelectById(HomeAddressId);//getting HomeAddress by specific HomeAddressId
                                                                                       //from the HomeAddressrepository
            HomeAddress HomeAddress = MapHomeAddressRequestToHomeAddress(updateHomeAddress);//MApping updateHomeAddress with HomeAddressRequest
            HomeAddress.Id = HomeAddressId; //saving HomeAddressId to requested HomeAddress.Id
            HomeAddress.AccountInfoId = local.AccountInfoId;//saving AccountInfoId to requested AccountInfoId
            HomeAddress updatedHomeAddress = await _HomeAddressRepository.Update(HomeAddress); //Updating the Existing HomeAddress with the requested HomeAddress 

            if (updatedHomeAddress != null)  //If it is not null then map the updatedHomeAddress with the Response. 
            {

                return MapHomeAddressToHomeAddressResponse(updatedHomeAddress);
            }

            return null;
        }


        //mapping function for updatedHomeAddress with HomeAddressResponse
        private HomeAddressResponse MapHomeAddressToHomeAddressResponse(HomeAddress HomeAddress)
        {

            return new HomeAddressResponse
            {
                Id = HomeAddress.Id,
                AccountId= HomeAddress.AccountInfoId,
                Address=HomeAddress.Address,
                City=HomeAddress.City,
                PostalCode=HomeAddress.PostalCode,
                Country=HomeAddress.Country,
                Phone=HomeAddress.TelePhone,


                Account = new AccountInfoResponse
                {
                    Id = HomeAddress.AccountInfo.Id,
                    CreatedDate=HomeAddress.AccountInfo.CreatedDate,
                    UserId=HomeAddress.AccountInfo.UserId

                }
            };

        }
        //mapping function for HomeAddressRequest
        private static HomeAddress MapHomeAddressRequestToHomeAddress(HomeAddressRequest HomeAddressRequest)
        {
            return new HomeAddress()
            {


                Address = HomeAddressRequest.Address,
                City = HomeAddressRequest.City,
                PostalCode = HomeAddressRequest.PostalCode,
                Country=HomeAddressRequest.Country,
                TelePhone=HomeAddressRequest.Phone
            };
        }
    }
}
