using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string? Status { get; set; } = string.Empty;
        public decimal? TotalAmount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}

