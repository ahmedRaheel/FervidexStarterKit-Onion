using StarterKit.Api.Endpoints.Features.Products.Create;
using StarterKit.Api.Endpoints.Features.Products.Delete;
using StarterKit.Api.Endpoints.Features.Products.GetById;
using StarterKit.Api.Endpoints.Features.Products.GetPaged;
using StarterKit.Api.Endpoints.Features.Products.Update;
namespace StarterKit.Api.Extensions;
public static class EndpointExtensions
{
    /// <summary>
    /// Map API endpoints
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication MapApiEndpoints(this WebApplication app)
    { 
       
        app.MapCreateProduct()
           .MapGetProductById()
           .MapGetPagedProducts()
           .MapUpdateProduct()
           .MapDeleteProduct();
        return app;
    }
}
