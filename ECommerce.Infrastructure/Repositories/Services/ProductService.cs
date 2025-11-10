using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository repository;
        private readonly IMapper mapper;
        private readonly string webRootPath;

        public ProductService(IProductRepository repository, IMapper mapper , string webRootPath)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.webRootPath = webRootPath;
        }

        public async Task<ProductDTO> AddAsync(CreateProductDTO dto)
        {
            var entity = mapper.Map<Product>(dto);

            if (dto.Photo != null) {

                var uploadFolder = Path.Combine(webRootPath, "images", "products");
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Photo.FileName);
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Photo.CopyToAsync(stream);
                }
                entity.PhotoUrl = Path.Combine("images/products", fileName).Replace("\\", "/");
            }

            await repository.AddAsync(entity);
            var product = mapper.Map<ProductDTO>(entity);
            return product;
        }

        public  async Task DeleteAsync(int id)
        {
            var product = await repository.GetByIdAsync(id);
            if (product == null) throw new KeyNotFoundException("not found");
            if (!string.IsNullOrEmpty(product.PhotoUrl))
            {
                var filePath = Path.Combine(webRootPath, product.PhotoUrl);
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

                await repository.DeleteAsync(id);
            }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = await repository.GetAllAsync();
            return mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            var product = await repository.GetByIdAsync(id);
            if(product == null) throw new KeyNotFoundException($"{id} is not found");
            return mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> UpdateAsync(int id, UpdateProductDTO dto)
        {
            var product = await repository.GetByIdAsync(id);
            if (product == null) throw new KeyNotFoundException("NotFound");
            mapper.Map(dto, product);

            if (dto.Photo != null)
            {
                if (!string.IsNullOrEmpty(product.PhotoUrl))
                {
                    var oldPath = Path.Combine(webRootPath, product.PhotoUrl);
                    if (File.Exists(oldPath))
                        File.Delete(oldPath);
                }

                var uploadsFolder = Path.Combine(webRootPath, "images", "products");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(dto.Photo.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Photo.CopyToAsync(stream);
                }

                product.PhotoUrl = Path.Combine("images/products", fileName).Replace("\\", "/");
            }



            await repository.UpdateAsync(product);
            return mapper.Map<ProductDTO>(product);
        }

        public async Task<PagedResult<ProductDTO>> GetFilteredAsync(string? search, int? categoryId, int pageNumber, int pageSize)
        {
            var (products, totalCount) = await repository.GetFilteredAsync(search, categoryId, pageNumber, pageSize);

            return new PagedResult<ProductDTO>
            {
                Items = mapper.Map<IEnumerable<ProductDTO>>(products),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


    }
}
