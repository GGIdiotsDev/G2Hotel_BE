using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.Helper
{
    public class MailSettings
    {
        public string EmailFrom { get; set; } = "g2hotelluxstay@gmail.com";
        public string SmtpHost { get; set; } = "smtp.gmail.com";
        public int SmtpPort { get; set; } = 465;
        public string SmtpUser { get; set; } = "g2hotelluxstay@gmail.com";
        public string SmtpPass { get; set; } = "mnbtmhtmoupbbpgo";
    }
}