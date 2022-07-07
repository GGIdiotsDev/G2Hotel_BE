using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class ForgotMailDTO
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Nhập chính xác địa chỉ email")]
        public string Email { get; set; } = null!;
    }
}