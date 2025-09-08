using System;
using System.Collections.Generic;

namespace PraktikaShop.Models;

public partial class UserComment
{
    public int UserCommentId { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public string? Comment { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
