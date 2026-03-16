using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.DTOs
{
   public  class CreatePaymentDto
    {
        public Guid? OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? Amount { get; set; }
        public string? PaymentStatus { get; set; }  
        public string? TransactionId { get; set; }
    }
}
