using MediatR;
using Payment.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Command
{
    public record UpdatePaymentCommand(Guid Id, UpdatePaymentDto Dto) : IRequest<PaymentDto?>;

}
