using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class BookingTimeSlot
{
    public int BookingId { get; set; }

    public int TimeslotId { get; set; }

    public int FieldId { get; set; }

    public DateOnly BookingDate { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Field Field { get; set; } = null!;

    public virtual TimeSlot Timeslot { get; set; } = null!;
}
