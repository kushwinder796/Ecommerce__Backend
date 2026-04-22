using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offers.Application.DTOs
{
    public class OfferDto
    {
        public Guid Id { get; set; }
        public Guid? ProductId { get; set; }
        public string? ProductName { get; set; }
        public Guid? OrderId { get; set; }
        public decimal OfferedPrice { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
