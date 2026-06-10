using System;
using System.Collections.Generic;
using System.Text;

namespace StarterKit.Domain.DTO;

public sealed record ProductResponse(Guid Id, string Name, decimal Price, string Sku, DateTime CreatedAtUtc);