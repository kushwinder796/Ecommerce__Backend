using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Entities;

[Table("users", Schema = "identity")]
[Index("Email", Name = "users_email_key", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required]
    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; }

    [Required]
    [Column("password_hash")]
    public string PasswordHash { get; set; }

    [Column("first_name")]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Column("last_name")]
    [StringLength(100)]
    public string LastName { get; set; }

    [Column("role")]
    [StringLength(50)]
    public string Role { get; set; }

    [Column("isactive")]
    public bool? Isactive { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }
}
