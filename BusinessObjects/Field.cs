using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
    [JsonIgnore]
    public virtual ICollection<BookingTimeSlot>? BookingTimeSlots { get; set; } = new List<BookingTimeSlot>();
    [JsonIgnore]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    [JsonIgnore]
    public virtual ICollection<FeedBack> FeedBacks { get; set; } = new List<FeedBack>();
}
