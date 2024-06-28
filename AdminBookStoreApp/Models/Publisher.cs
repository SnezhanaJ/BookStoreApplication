namespace AdminBookStoreApp.Models
{
    public class Publisher
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Books>? Books { get; set; }
    }
}
