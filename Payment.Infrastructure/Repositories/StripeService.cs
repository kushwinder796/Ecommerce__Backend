using Microsoft.Extensions.Configuration;
using Payment.Application.DTOs;
using Payment.Application.Interface;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Repositories
{
    public class StripeService : IStripeService
    {
        private readonly IConfiguration _config;

        public StripeService(IConfiguration config)
        {
            _config = config;
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }

        public async Task<StripePaymentResponseDto> CreatePaymentIntentAsync(StripePaymentRequestDto request)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(request.Amount * 100),
                Currency = request.Currency,
                Description = request.Description,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true
                }
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            return new StripePaymentResponseDto
            {
                ClientSecret = intent.ClientSecret,
                PaymentIntentId = intent.Id,
                Status = intent.Status
            };
        }

        public async Task<string> GetPaymentStatusAsync(string paymentIntentId)
        {
            var service = new PaymentIntentService();
            var intent = await service.GetAsync(paymentIntentId);
            return intent.Status;
        }
    }
}
