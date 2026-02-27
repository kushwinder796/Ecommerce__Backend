using MediatR;
using Offers.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offers.Application.Command.CommandHandler
{
    public class DeleteOfferHandler : IRequestHandler<DeleteOfferCommand, bool>
    {
        private readonly IOfferUnitOfWork _context;

        public DeleteOfferHandler(IOfferUnitOfWork context)
        {
            _context = context;
        }
        public async Task<bool> Handle(DeleteOfferCommand request, CancellationToken ct)
        {
            var offer = await _context.Offers.GetByIdAsync(request.Id);
            if (offer is null) return false;

            await _context.Offers.DeleteAsync(request.Id);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}
