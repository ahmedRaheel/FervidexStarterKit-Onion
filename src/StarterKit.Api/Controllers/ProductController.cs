using Microsoft.AspNetCore.Mvc;
using StarterKit.Domain.Constants;
using StarterKit.Domain.DTO.API;
using StarterKit.UseCase.Handlers.Products.Create;
using StarterKit.UseCase.Handlers.Products.Delete;
using StarterKit.UseCase.Handlers.Products.GetById;
using StarterKit.UseCase.Handlers.Products.GetPaged;
using StarterKit.UseCase.Handlers.Products.Update;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ISender _sender;

    public ProductController(ISender sender) => _sender = sender;

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProduct(Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetProductByIdQuery(id), ct);

        return result switch
        {
            { IsSuccess: true } => Ok(result.Value),
            { Error: { Code: ErrorCodes.NotFound } } => NotFound(result.Error.Message),
            _ => BadRequest(result.Error?.Message)
        };
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest command, CancellationToken ct)
    {
        var result = await _sender.Send(new CreateProductCommand(command.Name, command.Price, command.Sku), ct);

        return result switch
        {
            { IsSuccess: true } => CreatedAtAction(nameof(GetProduct), new { id = result.Value!.Id }, result.Value),
            { Error: { Code: ErrorCodes.Validation } } => BadRequest(result.Error.Message),
            _ => BadRequest(result.Error?.Message)
        };
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductRequest command, CancellationToken ct)
    {
        var result = await _sender.Send(new UpdateProductCommand(id, command.Name, command.Price, command.Sku), ct);

        return result switch
        {
            { IsSuccess: true } => NoContent(),
            { Error: { Code: ErrorCodes.NotFound } } => NotFound(result.Error.Message),
            _ => BadRequest(result.Error?.Message)
        };
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new DeleteProductCommand(id), ct);

        return result switch
        {
            { IsSuccess: true } => NoContent(),
            { Error: { Code: ErrorCodes.NotFound } } => NotFound(result.Error.Message),
            _ => BadRequest(result.Error?.Message)
        };
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken ct)
    {
        var result = await _sender.Send(new GetPagedProductsQuery(pageNumber, pageSize), ct);

        return result switch
        {
            { IsSuccess: true } => Ok(result.Value),
            _ => BadRequest(result.Error?.Message)
        };
    }
}