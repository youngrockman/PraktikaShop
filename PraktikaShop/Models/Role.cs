using System;
using System.Collections.Generic;

namespace PraktikaShop.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
