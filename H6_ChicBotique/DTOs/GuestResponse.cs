using H6_ChicBotique.Helpers;

namespace H6_ChicBotique.DTOs
{
    public class GuestResponse
    {
        //It is a output for the corresponding request
        public int Id { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }


        public string Telephone { get; set; }
        public string Email { get; set; }

        public Role Role { get; set; }
        public AccountInfoResponse Account { get; set; }
        public HomeAddressResponse ShippingAddress { get; set; }
    }
}
