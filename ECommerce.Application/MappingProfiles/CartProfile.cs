using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.MappingProfiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : null))
                .ForMember(d => d.UnutePrice, o => o.MapFrom(s => s.Product != null ? s.Product.Price : 0));

            CreateMap<Cart, CartDTO>()
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));
        }
    }
}
