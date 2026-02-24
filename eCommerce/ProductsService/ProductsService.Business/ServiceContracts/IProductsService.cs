
using ProductsService.Business.DTO;
using ProductsService.DataAccess.Entities;
using System.Linq.Expressions;

namespace ProductsService.Business.ServiceContracts
{
    public interface IProductsService
    {
        Task<List<ProductResponse?>> GetProducts();
        Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression);
        Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionExpression);
        Task<ProductResponse?> AddProduct(ProductAddRequest request);
        Task<ProductResponse?> UpdateProduct(ProductUpdateRequest request);
        Task<bool> DeleteProduct(Guid productID);

    }
}
