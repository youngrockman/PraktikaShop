using System;
using System.Collections.Generic;

namespace PraktikaShop.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int? Count { get; set; }

    public int? Cost { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
