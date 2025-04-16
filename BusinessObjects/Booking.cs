using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Booking
{
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int FieldId { get; set; }

    public DateOnly BookingDate { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<BookingTimeSlot> BookingTimeSlots { get; set; } = new List<BookingTimeSlot>();

    public virtual Field Field { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
