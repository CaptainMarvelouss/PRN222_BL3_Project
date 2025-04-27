namespace PRN222_BL3_Project.Models;
public class BookingHistoryViewModel
{
    public int BookingId { get; set; }
    public DateTime BookingDate { get; set; }
    public string FieldName { get; set; } = "";
    public string TimeSlots { get; set; } = "";
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "";
    public string UserName { get; set; } = "";
}
