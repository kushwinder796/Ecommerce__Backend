using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Offers.Infrastructure.Persistence.Entities;

[Table("offers", Schema = "negotiation")]
public partial class Offer
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("order_id")]
    public Guid? OrderId { get; set; }

    [Column("offered_price")]
    [Precision(10, 2)]
    public decimal OfferedPrice { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string Status { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }
}
