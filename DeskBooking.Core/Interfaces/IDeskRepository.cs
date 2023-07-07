using DeskBooking.Core.Domain;

namespace DeskBooking.Core.Interfaces
{
    public interface IDeskRepository
    {
        IEnumerable<Desk> GetAvailableDesks(DateTime date);
    }
}
