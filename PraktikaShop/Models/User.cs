using System;
using System.Collections.Generic;

namespace PraktikaShop.Models;

public partial class User
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public string? Fullname { get; set; }

    public string? Passport { get; set; }

    public string? Phone { get; set; }

    public DateOnly? Birthday { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();

    public virtual Role Role { get; set; } = null!;
}
