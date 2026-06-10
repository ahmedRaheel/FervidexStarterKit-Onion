using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarterKit.Domain.DTO;
using StarterKit.Domain.DTO.API;
using StarterKit.UseCase.Handlers.Products.Create;
using StarterKit.UseCase.Handlers.Products.Delete;
using StarterKit.UseCase.Handlers.Products.GetById;
using StarterKit.UseCase.Handlers.Products.Update;
using System.Reflection;

namespace StarterKit.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ISender _sender;

    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponse>> GetProduct(Guid id, CancellationToken ct)
    {
        return await _sender.Send(new GetProductByIdQuery(id), ct) is { } product
            ? Ok(product)
            : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductRequest command, CancellationToken ct)
    {
        var product = await _sender.Send(new CreateProductCommand(command.Name, command.Price, command.Sku), ct);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }


    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(
        Guid id,
        [FromBody] UpdateProductRequest command,
        CancellationToken ct)
    {
        await _sender.Send(
            new UpdateProductCommand(id, command.Name, command.Price, command.Sku),
            ct);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken ct)
    {
        await _sender.Send(new DeleteProductCommand(id), ct);
        return NoContent();
    }
}
