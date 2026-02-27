using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.DTOs
{
    public class CreateOrderRequest
    {
        public List<CreateOrderItemDto> Items { get; set; }= new List<CreateOrderItemDto>();
    }
}
