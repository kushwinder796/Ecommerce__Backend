using MediatR;
using Payment.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Queries
{
    public record GetPaymentByIdQuery(Guid Id) : IRequest<PaymentDto?>;
}
