using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class UserUpdateDTO
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string? Gender { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}