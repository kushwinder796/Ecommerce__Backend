using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.DTOs
{
    public class StripePaymentResponseDto
    {
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }
        public string Status { get; set; }
    }
}
