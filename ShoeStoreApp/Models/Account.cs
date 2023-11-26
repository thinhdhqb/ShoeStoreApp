using Microsoft.EntityFrameworkCore;
using System;

namespace ShoeStoreApp.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Account
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
