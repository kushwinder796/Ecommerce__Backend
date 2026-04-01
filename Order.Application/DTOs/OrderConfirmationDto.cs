using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.DTOs
{
    public  class OrderConfirmationDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ShippingDate { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
