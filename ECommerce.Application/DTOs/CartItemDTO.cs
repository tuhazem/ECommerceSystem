using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal UnutePrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total => UnutePrice * Quantity;

    }

    public class AddCartItemDTO {

        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1 ;  

    }

    public class UpdateCartItemDTO {

        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
