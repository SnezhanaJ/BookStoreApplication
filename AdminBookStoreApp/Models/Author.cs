﻿namespace AdminBookStoreApp.Models
{
    public class Author
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Books>? WrittenBooks { get; set; }
    }
}
