using System.Data.Common;

namespace StarterKit.UseCase.Abstractions;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
    Task<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default);
}
