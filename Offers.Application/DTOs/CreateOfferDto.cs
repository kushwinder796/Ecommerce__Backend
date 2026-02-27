using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offers.Application.DTOs
{
    public  class CreateOfferDto
    {
        public Guid? OrderId { get; set; }
        public decimal OfferedPrice { get; set; }
    }
}
