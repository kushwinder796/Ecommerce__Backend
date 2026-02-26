using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Payment.Infrastructure.Persistence.Entities;

[Table("payments", Schema = "payments")]
public partial class PaymentSysten
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("order_id")]
    public Guid? OrderId { get; set; }

    [Column("payment_method")]
    [StringLength(50)]
    public string PaymentMethod { get; set; }

    [Column("amount")]
    [Precision(12, 2)]
    public decimal? Amount { get; set; }

    [Column("payment_status")]
    [StringLength(50)]
    public string PaymentStatus { get; set; }

    [Column("transaction_id")]
    [StringLength(255)]
    public string TransactionId { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }
}
