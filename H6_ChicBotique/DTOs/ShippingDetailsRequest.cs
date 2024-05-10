namespace H6_ChicBotique.DTOs
{
    //Input from the clientside  for having Shippingaddress 
    public class ShippingDetailsRequest
    {
        public string Address { get; set; }
        public string City { get; set; }

        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
    }
}
