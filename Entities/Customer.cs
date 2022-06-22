using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.Entities
{
    [Table("Customers")]
    public class Customer : AbstractEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string CitizenIdentity { get; set; } = null!;
        public ICollection<Payment>? Payments { get; set; }
    }
}