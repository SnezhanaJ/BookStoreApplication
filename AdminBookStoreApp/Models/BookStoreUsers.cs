using Microsoft.AspNetCore.Identity;

namespace AdminBookStoreApp.Models
{
    public class BookStoreUsers 
    {

        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
