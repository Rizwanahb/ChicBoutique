using H6_ChicBotique.Database.Entities;
using H6_ChicBotique.DTOs;
using H6_ChicBotique.Repositories;

namespace H6_ChicBotique.Services
{
    public interface IShippingDetailsService
    {
        Task<List<ShippingDetailsResponse>> GetAllShippingDetails();
        Task<ShippingDetailsResponse> GetShippingDetailsById(int ShippingDetailsId);

        Task<ShippingDetailsResponse> InsertShippingsDetails(ShippingDetailsRequest newShippingDetails);
        Task<ShippingDetailsResponse> UpdateShippingDetails(int ShippingDetailsId, ShippingDetailsRequest updateShippingDetails);
        // Task<ShippingDetailsResponse> DeletehippingAddress(int ShippingDetailsId);


    }
    public class ShippingDetailsService:IShippingDetailsService
    {
        private readonly IShippingDetailsRepository _shippingDetailsRepository;
        public ShippingDetailsService(IShippingDetailsRepository ShippingDetailsRepository)
        {
            _shippingDetailsRepository = ShippingDetailsRepository;
        }
        public async Task<List<ShippingDetailsResponse>> GetAllShippingDetails()
        {
            List<ShippingDetails> ShippingDetails = await _shippingDetailsRepository.SelectAll();

            return ShippingDetails.Select(ShippingDetails => MapShippingDetailsToShippingDetailsResponse(ShippingDetails)).ToList();

        }
        public async Task<ShippingDetailsResponse> GetShippingDetailsById(int ShippingDetailsId)
        {
            ShippingDetails ShippingDetails = await _shippingDetailsRepository.SelectById(ShippingDetailsId);

            if (ShippingDetails != null)
            {

                return MapShippingDetailsToShippingDetailsResponse(ShippingDetails);
            }
            return null;
        }

        public async Task<ShippingDetailsResponse> InsertShippingsDetails(ShippingDetailsRequest newShippingDetails)
        {
            ShippingDetails shippingsDetails = MapShippingDetailsRequestToShippingDetails(newShippingDetails);

            ShippingDetails insertedshippingsDetails = await _shippingDetailsRepository.Create(shippingsDetails);

            if (insertedshippingsDetails != null)
            {
                return MapShippingDetailsToShippingDetailsResponse(insertedshippingsDetails);

            }
            return null;
        }


        public async Task<ShippingDetailsResponse> UpdateShippingDetails(int ShippingDetailsId, ShippingDetailsRequest updateShippingDetails)
        {
            ShippingDetails local = await _shippingDetailsRepository.SelectById(ShippingDetailsId);
            ShippingDetails ShippingDetails = MapShippingDetailsRequestToShippingDetails(updateShippingDetails);
            ShippingDetails.Id = ShippingDetailsId;
            ShippingDetails.OrderId = local.OrderId;
            ShippingDetails updatedShippingDetails = await _shippingDetailsRepository.Update(ShippingDetails);

            if (updatedShippingDetails != null)
            {

                return MapShippingDetailsToShippingDetailsResponse(updatedShippingDetails);
            }

            return null;
        }


        private ShippingDetailsResponse MapShippingDetailsToShippingDetailsResponse(ShippingDetails newSippingDetails)
        {

            return new ShippingDetailsResponse
            {
                Id = newSippingDetails.Id,
                OrderId= newSippingDetails.OrderId,
                Address= newSippingDetails.Address,
                City= newSippingDetails.City,
                PostalCode= newSippingDetails.PostalCode,
                Country= newSippingDetails.Country,
                Phone= newSippingDetails.Phone,



            };

        }
        private static ShippingDetails MapShippingDetailsRequestToShippingDetails(ShippingDetailsRequest ShippingDetailsRequest)
        {
            return new ShippingDetails()
            {


                Address = ShippingDetailsRequest.Address,
                City = ShippingDetailsRequest.City,
                PostalCode = ShippingDetailsRequest.PostalCode,
                Country=ShippingDetailsRequest.Country,
                Phone=ShippingDetailsRequest.Phone
            };
        }
    }
}
