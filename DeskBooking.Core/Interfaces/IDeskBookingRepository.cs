
using DeskBooking.Core.Domain;

namespace DeskBooking.Core.Interfaces
{
    public interface IDeskBookingRepository
    {
        void Save(DeskBookingModel deskBooking);
    }
}
