using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class UserProfile
{
    public int ProfileId { get; set; }

    public string ProfileName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();

    public virtual ICollection<Module> Modules { get; } = new List<Module>();
}
