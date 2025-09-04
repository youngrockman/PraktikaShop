using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace PraktikaShop.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int? Count { get; set; }

    public int? Cost { get; set; }

    public int Discount
    {
        get
        {
            int a = 1 + 1;
            return a;
        }
    }

    public string? Image { get; set; }

    public Bitmap? ParseImage
    {
        get
        {
            return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/" + Image);
        }
    }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
