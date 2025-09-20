using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;

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
            try
            {
                if (!string.IsNullOrEmpty(Image) && File.Exists(Image))
                {
                    return new Bitmap(Image);
                }
                
                string placeholderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shop", "picture.png");
                
                if (File.Exists(placeholderPath))
                {
                    return new Bitmap(placeholderPath);
                }
                
                return null;
            }
            catch (Exception)
            {
                return null;
            }
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
