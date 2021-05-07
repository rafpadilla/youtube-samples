using System;
using System.Collections.Generic;

namespace BogusSample.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
