using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Order.Infrastructure.Persistence.Entities;

[Table("order_items", Schema = "orders")]
public partial class OrderItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_id")]
    public Guid? OrderId { get; set; }

    [Column("product_id")]
    public Guid? ProductId { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [Column("price")]
    [Precision(10, 2)]
    public decimal? Price { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderItems")]
    public virtual OrderEntity Order { get; set; }
}
