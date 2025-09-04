using System;
using System.Collections.Generic;

namespace PraktikaShop.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
