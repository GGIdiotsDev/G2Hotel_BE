using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using g2hotel_server.Entities;

namespace g2hotel_server.Services.Interfaces
{
    public interface ICustomerRepository
    {
        Customer AddCustomer(Customer customer);
        void Update(Customer customer);
        void Delete(Customer customer);
    }
}