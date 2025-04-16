using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class TimeSlot
{
    public int TimeslotId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public virtual ICollection<BookingTimeSlot> BookingTimeSlots { get; set; } = new List<BookingTimeSlot>();
}
