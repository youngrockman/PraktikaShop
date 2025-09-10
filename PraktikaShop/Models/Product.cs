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

    public string? Image { get; set; }

    public Bitmap ParseImage
    {
        get
        {
            return new Bitmap(this.Image);
        }
    }

    public int Discount
    {
        get
        {

            if (Cost >= 1000)
            {
                return 10;
            }

            if (Cost >= 5000)
            {
                return 25;
            }

            return 0;

        }
    }


    

    public virtual ICollection<BasketProduct> BasketProducts { get; set; } = new List<BasketProduct>();

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ICollection<UserComment> UserComments { get; set; } = new List<UserComment>();
}
