using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<ProductDTO> GetByIdAsync(int id);
        Task<ProductDTO> AddAsync(CreateProductDTO dto);
        Task<ProductDTO> UpdateAsync(int id , UpdateProductDTO dto);
        Task DeleteAsync(int id);
    }
}
