using System;
using System.Collections.Generic;

namespace PraktikaShop.Models;

public partial class BasketProduct
{
    public int Id { get; set; }

    public int? BasketId { get; set; }

    public int? ProductId { get; set; }

    public int ProductCount { get; set; }

    public virtual Basket? Basket { get; set; }

    public virtual Product? Product { get; set; }
}
