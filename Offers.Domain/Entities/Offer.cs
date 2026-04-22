using System;
using System.Collections.Generic;

namespace Offers.Domain.Entities;

public partial class Offer
{
    public Guid Id { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? ProductId { get; set; }

    public decimal OfferedPrice { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }
}
