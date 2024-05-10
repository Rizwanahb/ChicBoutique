namespace H6_ChicBotique.DTOs
{
    public class OrderAndPaymentResponse
    {
        public int Id { get; set; }


        public DateTime OrderDate { get; set; }


        public Guid AccountInfoId { get; set; }

        public string? Status { get; set; }

        public string? TransactionId { get; set; }


        public string? PaymentMethod { get; set; }
        public decimal Amount { get; set; }

        public AccountInfoResponse Account { get; set; }
        public ShippingDetailsResponse ShippingDetails { get; set; }
        public List<OrderDetailsResponse> OrderDetails { get; set; } = new();
    }
}
