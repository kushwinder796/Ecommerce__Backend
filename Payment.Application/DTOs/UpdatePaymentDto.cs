using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.DTOs
{
    public class UpdatePaymentDto
    {
        public string? PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
    }
}
