namespace ShoeStoreApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int DeliveryAddressId { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
        public long Total { get; set; }
        public long DeliveryFee { get; set; }
        public DateTime TimeCreated { get; set; }
        public int Status { get; set; }
        public ICollection<OrderItem> Items { get; set; }
    }
}
