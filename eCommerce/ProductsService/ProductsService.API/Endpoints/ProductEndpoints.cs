using FluentValidation;
using ProductsService.Business.DTO;
using ProductsService.Business.ServiceContracts;
using ProductsService.Business.Validators;
using System.Runtime.CompilerServices;

namespace ProductsService.API.Endpoints
{
    public static class ProductEndpoints
    {
        public static IEndpointRouteBuilder MapProductEndpoints(
            this IEndpointRouteBuilder app)
        {
            //GET /api/products
            app.MapGet("/api/products", async (IProductsService productsService) =>
            {
                List<ProductResponse?> products = await productsService.GetProducts();
                return Results.Ok(products);
            });

            //GET /api/products/search/product-id/{guid}
            app.MapGet("/api/products/search/product-id/{ProductID}",
                async (IProductsService productsService, Guid ProductID) =>
            {
                ProductResponse? product = await productsService.GetProductByCondition(
                    temp => temp.ProductID == ProductID
                    );
                if(product == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(product);
            });

            //GET /api/products/search/{searchString}
            app.MapGet("/api/products/search/{SearchString}",
                async (IProductsService productsService, string SearchString) =>
                {
                    List<ProductResponse?> productsByProductName = await productsService.GetProductsByCondition(
                        temp => temp.ProductName != null
                        && temp.ProductName.Contains(SearchString, StringComparison.OrdinalIgnoreCase)
                        );
                    List<ProductResponse?> productsByCategory = await productsService.GetProductsByCondition(
                        temp => temp.Category != null
                        && temp.Category.Contains(SearchString, StringComparison.OrdinalIgnoreCase)); ;

                    var products = productsByProductName.Union(productsByCategory);
                    return Results.Ok(products);
                });

            //POST /api/products
            app.MapPost("/api/products", async (IProductsService productsService,
                IValidator<ProductAddRequest> ProductAddRequestValidator,
                ProductAddRequest productAddRequest) =>
            {
                var validationResult = await ProductAddRequestValidator.ValidateAsync(productAddRequest);
                if (!validationResult.IsValid)
                {
                    Dictionary<string, string[]> errors = validationResult.Errors.
                        GroupBy(temp => temp.PropertyName)
                        .ToDictionary(grp => grp.Key,
                        grp => grp.Select(err => err.ErrorMessage).ToArray());
                    return Results.ValidationProblem(errors);
                }

                var addedProductResponse = await productsService.AddProduct(productAddRequest);
                if (addedProductResponse != null)
                {
                    return Results.Created($"/api/products/search/product-id/{addedProductResponse.ProductID}", addedProductResponse);
                }
                return Results.Problem("Error in adding product.");
            });

            //PUT /api/products
            app.MapPut("/api/products", async (IProductsService productsService,
                IValidator<ProductUpdateRequest> ProductUpdateRequestValidator,
                ProductUpdateRequest productUpdateRequest) =>
            {
                var validationResult = await ProductUpdateRequestValidator.ValidateAsync(productUpdateRequest);
                if (!validationResult.IsValid)
                {
                    Dictionary<string, string[]> errors = validationResult.Errors.
                        GroupBy(temp => temp.PropertyName)
                        .ToDictionary(grp => grp.Key,
                        grp => grp.Select(err => err.ErrorMessage).ToArray());
                    return Results.ValidationProblem(errors);
                }

                var updatedProductResponse = await productsService.UpdateProduct(productUpdateRequest);
                if (updatedProductResponse != null)
                {
                    return Results.Ok(updatedProductResponse);
                }
                return Results.Problem("Error in updating product.");
            });

            //DELETE /api/products/{guid}
            app.MapDelete("/api/products/{ProductID:guid}",
                async (IProductsService productsService,
                Guid ProductID) =>
                {
                    bool isDeleted = await productsService.DeleteProduct(ProductID);
                    if (isDeleted)
                    {
                        return Results.Ok(true);
                    }
                    return Results.Problem("Error in deleting product.");

                });


            return app;
        }
    }
}
