using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository repository;
        private readonly IMapper mapper;

        public CategoryService(ICategoryRepository repository , IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }


        public async Task<CategoryDTO> AddAsync(CreateCategoryDTO dto)
        {
            var category = mapper.Map<Category>(dto);
            await repository.AddAsync(category);
            return mapper.Map<CategoryDTO>(category);
        }

        public Task DeleteAsync(int id)
        {
            return repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var category = await repository.GetAllAsync();
            return mapper.Map<IEnumerable<CategoryDTO>>(category);
        }

        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            var category = await repository.GetByIdAsync(id);
            if (category == null) throw new KeyNotFoundException("Not Found");
            return mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO> UpdateAsync(int id , UpdateCategoryDTO dto)
        {
            var category = await repository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            mapper.Map(dto, category);
            await repository.UpdateAsync(category);
            return mapper.Map<CategoryDTO>(category);
        }
    }
}
