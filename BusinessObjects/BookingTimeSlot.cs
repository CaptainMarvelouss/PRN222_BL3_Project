using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObjects;

public partial class BookingTimeSlot
{
    public int BookingId { get; set; }

    public int TimeslotId { get; set; }

    public int FieldId { get; set; }

    public DateOnly BookingDate { get; set; }
    [JsonIgnore]
    public virtual Booking? Booking { get; set; } = null!;
    [JsonIgnore]
    public virtual Field? Field { get; set; } = null!;
    [JsonIgnore]
    public virtual TimeSlot? Timeslot { get; set; } = null!;
}
