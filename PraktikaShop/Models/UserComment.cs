using System;
using System.Collections.Generic;

namespace PraktikaShop.Models;

public partial class UserComment
{
    public int Commentid { get; set; }

    public int? Userid { get; set; }

    public int? Productid { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
