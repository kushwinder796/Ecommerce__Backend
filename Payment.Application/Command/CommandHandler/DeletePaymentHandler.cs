using MediatR;
using Payment.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Command.CommandHandler
{
    public class DeletePaymentHandler : IRequestHandler<DeletePaymentCommand, bool>
    {
        private readonly IPaymentUnitOfWork _context;

        public DeletePaymentHandler(IPaymentUnitOfWork context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeletePaymentCommand request, CancellationToken ct)
        {
            var payment = await _context.Payments.GetByIdAsync(request.Id);
            if (payment is null) return false;

            await _context.Payments.DeleteAsync(request.Id);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}
