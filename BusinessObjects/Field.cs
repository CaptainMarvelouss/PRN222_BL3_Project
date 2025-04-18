using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Field
{
    public int FieldId { get; set; }

    public string FieldName { get; set; } = null!;

    public string FieldType { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<BookingTimeSlot> BookingTimeSlots { get; set; } = new List<BookingTimeSlot>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<FeedBack> FeedBacks { get; set; } = new List<FeedBack>();
}
