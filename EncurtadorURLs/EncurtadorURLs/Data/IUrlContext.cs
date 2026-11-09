using EncurtadorURLs.Models;

namespace EncurtadorURLs.Data;

/// <summary>
/// Contrato da camada de persistência. A implementação real (em memória, banco, etc.)
/// será criada na fase GREEN — por enquanto só precisamos do contrato para poder
/// mockar esta dependência nos testes.
/// </summary>
public interface IUrlContext
{
    Task<bool> ShortCodeExistsAsync(string shortCode);
    Task AddAsync(UrlItem item);
}