using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.DTOs
{
    public class MailDTO
    {
        public string To { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Html { get; set; } = null!;
        public string From { get; set; } = null!;
    }
}