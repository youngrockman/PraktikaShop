using System;
using System.Collections.Generic;

namespace PraktikaShop.Models;

public partial class Basket
{
    public int BasketId { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
