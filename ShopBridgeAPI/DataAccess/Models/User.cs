using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int ProfileId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? StatusSession { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; } = new List<AuditLog>();

    public virtual UserProfile Profile { get; set; } = null!;

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; } = new List<ShoppingCart>();
}
