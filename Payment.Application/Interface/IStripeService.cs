using Payment.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Interface
{
    public interface IStripeService
    {
        public Task<StripePaymentResponseDto> CreatePaymentIntentAsync(StripePaymentRequestDto request);
        public Task<string> GetPaymentStatusAsync(string paymentIntentId);
    }
}
