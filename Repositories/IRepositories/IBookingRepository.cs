using BusinessObjects;


namespace Repositories.IRepositories
{
    public interface IBookingRepository
    {
        //InvoicePitch GetInvoicePitchByTotalInvoicePitchID(int TotalInvoiceID);

        //List<InvoicePitch> GetInvoicesWithDetails(List<int> invoiceIds);
        //Task<List<InvoicePitch>> SearchInvoicePitch(SearchOrderDto dto);
        //Task<List<InvoicePitch>> SortInvoicePitch();
        Task<Booking> CreateBooking(Booking booking);
        //Task<List<Booking>> GetBookingById(int userId);
		/*Task<Booking> CancelBookingById(int id);
		Task<List<Booking>> GetAllBooking();
        Task<List<(int Month, int TotalRevenue)>> GetRevenueByYearAsync(int year);
        Task<List<int>> GetAvailableYearsAsync(); // Lấy danh sách các năm có dữ liệu*/
    }
}
