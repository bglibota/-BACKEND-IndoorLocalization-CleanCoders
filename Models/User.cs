using System;
using System.Collections.Generic;

namespace IndoorLocalization_API.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Salt { get; set; }

    public int? RoleId { get; set; }

    public virtual Role? Role { get; set; }
}
