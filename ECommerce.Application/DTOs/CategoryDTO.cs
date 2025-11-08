using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ProductinCategoryDTO> products { get; set; }
    }

    public class ProductinCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

    }

    public class CreateCategoryDTO {

        public string Name { get; set; } = string.Empty;
    }

    public class UpdateCategoryDTO { public string Name { get; set; } }

}
