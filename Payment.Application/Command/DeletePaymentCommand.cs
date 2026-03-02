using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Command
{
    public record DeletePaymentCommand(Guid Id) : IRequest<bool>;
}
