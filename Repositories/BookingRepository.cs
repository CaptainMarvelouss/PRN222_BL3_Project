using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class BookingRepository : IBookingRepository
    {
        public readonly FootballFieldBookingContext _context;
        public BookingRepository(FootballFieldBookingContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }
        public async Task<BookingTimeSlot> CreateBookingTimeSlot(BookingTimeSlot booking)
        {
            _context.BookingTimeSlots.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<String> GetTimeByBookingTimeId(int bookingTimeId)
        {
            return _context.BookingTimeSlots.Where(p => p.TimeslotId == bookingTimeId).FirstOrDefault().ToString();
        }
        public IEnumerable<Booking> GetBookingHistoryByUserId(int userId)
        {
            var bookings = _context.Bookings
                .Include(b => b.BookingTimeSlots)
                    .ThenInclude(bs => bs.Timeslot)
                .Include(b => b.Field)
                .Include(b => b.User)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate);

            return bookings.ToList();
        }
        public IEnumerable<Booking> SearchBookings(
        int? userId = null,
        string? keyword = null,
        DateOnly? fromDate = null,
        DateOnly? toDate = null)
        {
            var q = _context.Bookings
                .Include(b => b.BookingTimeSlots)
                    .ThenInclude(bs => bs.Timeslot)
                .Include(b => b.Field)
                .Include(b => b.User)
                .AsQueryable();

            if (userId.HasValue)
                q = q.Where(b => b.UserId == userId.Value);

            if (!string.IsNullOrWhiteSpace(keyword))
                q = q.Where(b =>
                    b.Field.FieldName.Contains(keyword) ||
                    b.User.UserName!.Contains(keyword));

            if (fromDate.HasValue)
                q = q.Where(b => b.BookingDate >= fromDate.Value);

            if (toDate.HasValue)
                q = q.Where(b => b.BookingDate <= toDate.Value);

            return q.OrderByDescending(b => b.BookingDate).ToList();
        }
    }
    /*public List<InvoicePitch> GetInvoicesWithDetails(List<int> invoiceIds)
    {
        return _context.InvoicePitches
    .Where(i => invoiceIds.Contains(i.InvoicePitchId))
    .Include(i => i.Pitch)
    .Include(i => i.PricePitch)
    .Include(i => i.BookingTime)
    .Include(i => i.TotalInvoice)
    .ToList();
    }

    public async Task<List<InvoicePitch>> GetInvoicePitchById(int accountId)
    {
        return await _context.InvoicePitches
                             .Where(p => p.TotalInvoice.UserId == accountId)
                             .Include(p => p.BookingTime)
                             .Include(p => p.PricePitch)
                             .Include(p => p.Pitch)
                             .Include(p => p.TotalInvoice)
                             .OrderByDescending(p => p.TotalInvoice.BookTime)
                             .ToListAsync();
    }

    public async Task<InvoicePitch> CancelInvoicePitchById(int id)
    {
        var pitches = await _context.InvoicePitches
                             .FirstOrDefaultAsync(p => p.InvoicePitchId == id);
        var tp = await _context.TotalInvoicePitches
                                .FirstOrDefaultAsync(p => p.TotalInvoiceId == id);
        _context.InvoicePitches.Remove(pitches);
        _context.TotalInvoicePitches.Remove(tp);
        await _context.SaveChangesAsync();
        return pitches;
    }

    public async Task<List<InvoicePitch>> SearchInvoicePitch(SearchOrderDto dto)
    {
        var query = _context.InvoicePitches
            .Include(s => s.Pitch)
            .Include(s => s.TotalInvoice)
            .Include(s => s.TotalInvoice.User)
            .Include(s => s.BookingTime)
            .Include(s => s.PricePitch)
            .OrderByDescending(p => p.TotalInvoice.BookTime)
            .AsQueryable();

        if (dto.UserId != null)
        {
            query = query.Where(q => q.TotalInvoice.UserId == dto.UserId);
        }
        if (dto.PitchId != null)
        {
            query = query.Where(q => q.Pitch.PitchId.Contains(dto.PitchId));
        }
        if (dto.Price != null)
        {
            query = query.Where(q => q.PricePitch.Price == dto.Price);
        }
        if (dto.date != null)
        {
            query = query.Where(q => q.TotalInvoice.BookTime == dto.date);
        }
        if (dto.CustomerName != null)
        {
            query = query.Where(q => q.TotalInvoice.User.Name.Contains(dto.CustomerName));
        }
        if (dto.CustomerPhone != null)
        {
            query = query.Where(q => q.TotalInvoice.User.PhoneNumber.Contains(dto.CustomerPhone));
        }
        return await query.ToListAsync();
    }

    public async Task<List<InvoicePitch>> SortInvoicePitch()
    {
        return await _context.InvoicePitches
            .OrderBy(ip => ip.Pitch.PitchId)
            //.ThenBy(ip => ip.Name)
            .ToListAsync();
    }

    public async Task<List<InvoicePitch>> GetAllInvoicePitch()
    {
        return await _context.InvoicePitches
                             .Include(p => p.BookingTime)
                             .Include(p => p.PricePitch)
                             .Include(p => p.Pitch)
                             .Include(p => p.TotalInvoice)
                             .Include(p => p.TotalInvoice.User)
                             .OrderByDescending(p => p.TotalInvoice.BookTime)
                             .ToListAsync();
    }


    public async Task<List<(int Month, int TotalRevenue)>> GetRevenueByYearAsync(int year)
    {
        var revenueData = await _context.InvoicePitches
            .Where(ip => ip.TotalInvoice != null && ip.TotalInvoice.BookTime.HasValue && ip.TotalInvoice.BookTime.Value.Year == year)
            .GroupBy(ip => ip.TotalInvoice.BookTime.Value.Month)
            .OrderBy(g => g.Key)
            .Select(g => new
            {
                Month = g.Key,
                TotalRevenue = g.Sum(ip => ip.PricePitch.Price ?? 0)
            })
            .ToListAsync();

        return revenueData.Select(r => (r.Month, r.TotalRevenue)).ToList();
    }

    public async Task<List<int>> GetAvailableYearsAsync()
    {
        return await _context.InvoicePitches
            .Where(ip => ip.TotalInvoice != null && ip.TotalInvoice.BookTime.HasValue)
            .Select(ip => ip.TotalInvoice.BookTime.Value.Year)
            .Distinct()
            .OrderByDescending(y => y)
            .ToListAsync();
    }*/
}

