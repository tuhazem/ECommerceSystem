using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }

        public string? ImgUrl { get; set; }

        public string? CategoryName { get; set; }

    }

    public class CreateProductDTO {

        public string Name { get; set; } = string.Empty ;
        public double Price { get; set; }

        public int CategoryId { get; set; }
        public IFormFile? Photo { get; set; }
    }

    public class UpdateProductDTO { 
    
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? Photo { get; set; }
    }


}
