using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
