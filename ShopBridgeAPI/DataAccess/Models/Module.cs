using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Module
{
    public int ModuleId { get; set; }

    public string ModuleName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Route { get; set; }

    public virtual ICollection<UserProfile> Profiles { get; } = new List<UserProfile>();
}
