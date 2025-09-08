using System;
using System.Collections.Generic;

namespace PraktikaShop.Models;

public partial class Basket
{
    public int BasketId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<BasketProduct> BasketProducts { get; set; } = new List<BasketProduct>();

    public virtual User User { get; set; } = null!;
}
