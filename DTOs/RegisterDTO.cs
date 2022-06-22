using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class RegisterDTO
    {
        [Required] public string Email { get; set; } = null!;
        [Required] public string Username { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;
    }
}