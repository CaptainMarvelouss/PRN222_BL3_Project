using BusinessObjects;


namespace Repositories
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBooking(Booking booking);
        Task<BookingTimeSlot> CreateBookingTimeSlot(BookingTimeSlot booking);
        Task<String> GetTimeByBookingTimeId(int bookingTimeId);
        IEnumerable<Booking> SearchBookings(
        int? userId = null,
        string? keyword = null,
        DateOnly? fromDate = null,
        DateOnly? toDate = null);
        IEnumerable<Booking> GetBookingHistoryByUserId(int userId);
    }
}
