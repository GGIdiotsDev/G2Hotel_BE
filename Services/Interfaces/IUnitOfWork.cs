using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace g2hotel_server.Services.Interfaces
{
    public interface IUnitOfWork
    {
        IRoomRepository RoomRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IPaymentTypeRepository PaymentTypeRepository { get; }
        IPhotoRepository PhotoRepository { get; }
        IUserRepository UserRepository { get; }
        IServiceRepository ServiceRepository { get; }
        IRoomTypeRepository RoomTypeRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IDetailRoomPaymentRepository DetailRoomPaymentRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}