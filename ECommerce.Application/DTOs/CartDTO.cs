using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class CartDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public IEnumerable<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
        public decimal TotalAmount => Items.Sum(i => i.Total);

    }
}
