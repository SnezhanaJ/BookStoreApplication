namespace AdminBookStoreApp.Models
{
    public class BookInOrder
    { 
        public Guid BookId { get; set; }
        public Books Book { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }
    }
}
