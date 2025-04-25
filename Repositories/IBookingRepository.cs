using BusinessObjects;


namespace Repositories
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBooking(Booking booking);
        Task<BookingTimeSlot> CreateBookingTimeSlot(BookingTimeSlot booking);
        Task<String> GetTimeByBookingTimeId(int bookingTimeId);
    }
}
