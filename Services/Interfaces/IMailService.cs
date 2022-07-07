using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using g2hotel_server.DTOs;

namespace g2hotel_server.Services.Interfaces
{
    public interface IMailService
    {
        void Send(MailDTO mailDto);
    }
}