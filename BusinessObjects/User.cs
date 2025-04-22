using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? GoogleId { get; set; }

    public string? PasswordHash { get; set; }

    public int RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<FeedBack> FeedBacks { get; set; } = new List<FeedBack>();

    public virtual Role? Role { get; set; }
}
