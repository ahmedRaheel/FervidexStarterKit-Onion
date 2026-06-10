namespace StarterKit.Domain.DTO.API;

public sealed record CreateProductRequest(string Name, decimal Price, string Sku);
