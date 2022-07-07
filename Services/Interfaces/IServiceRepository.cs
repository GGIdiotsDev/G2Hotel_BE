using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using g2hotel_server.Entities;

namespace g2hotel_server.Services.Interfaces
{
    public interface IServiceRepository
    {
        Service AddService(Service service);
        void Update(Service service);
        void Delete(Service service);
        Task<IEnumerable<Service>> GetServiceTasksAsync();
        Task<Service> GetServiceById(int id);
    }
}