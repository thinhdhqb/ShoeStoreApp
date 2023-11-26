namespace ShoeStoreApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public long Price { get; set; }
        public string Description { get; set; }
        public string Gender { get; set; }
        public string Category { get; set; }
        public ICollection<ProductVariant> Variants { get; set; }
    }
}
