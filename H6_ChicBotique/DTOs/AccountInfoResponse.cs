namespace H6_ChicBotique.DTOs
{
    public class AccountInfoResponse //It is a output for the corresponding request
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public int? UserId { get; set; }
        public IEnumerable<OrderAndPaymentResponse>? Orders { get; set; }
        public HomeAddressResponse HomeAddress { get; set; }
        public UserResponse User { get; set; }
    }
}
