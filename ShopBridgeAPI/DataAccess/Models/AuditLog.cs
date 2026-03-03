using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class AuditLog
{
    public int AuditId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; } = null!;

    public DateTime? ActionDateTime { get; set; }

    public virtual User User { get; set; } = null!;
}
