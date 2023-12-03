using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BE.Models;

public partial class User
{
    public int Id { get; set; }
    [Required]
    public string? UserName { get; set; }
    [Required]
    public string? Password { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? NumberCode { get; set; }
    [Required]
    public string? Class { get; set; }
    [Required]
    public string? Specification { get; set; }
    [Required]
    public string? Section { get; set; }
    [Required]
    public DateTime? JoinDate { get; set; }
    [Required]
    public string? Role { get; set; }
    [Required]
    public int? Active { get; set; }

    public virtual ICollection<Award> Awards { get; set; } = new List<Award>();

    public virtual ICollection<ForgotPassword> ForgotPasswords { get; set; } = new List<ForgotPassword>();

    public virtual ICollection<PasswordReset> PasswordResets { get; set; } = new List<PasswordReset>();

    public virtual ICollection<Support> Supports { get; set; } = new List<Support>();
}
