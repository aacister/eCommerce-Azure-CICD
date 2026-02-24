using AutoMapper;
using ProductsService.Business.DTO;
using ProductsService.DataAccess.Entities;


namespace ProductsService.Business.Mappers
{
    public class ProductAddRequestToProductMappingProfile 
        : Profile
    {
        public ProductAddRequestToProductMappingProfile() {
            CreateMap<ProductAddRequest, Product>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock))
                .ForMember(dest => dest.ProductID, opt => opt.Ignore());
        }
    }
}
