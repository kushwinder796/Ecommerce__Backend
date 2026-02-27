using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offers.Application.DTOs
{
    public class UpdateOfferDto
    {
        public decimal? OfferedPrice { get; set; }
        public string? Status { get; set; }
    }
}
