using EncurtadorURLs.Data;
using EncurtadorURLs.Models;

namespace EncurtadorURLs.Services;

/// <summary>
/// Interface para o serviço de URLs. Define as operações de negócio disponíveis.
/// </summary>
public interface IUrlsService
{
    Task<UrlItem> CreateShortUrlAsync(string originalUrl);
    Task<UrlItem?> GetUrlByIdAsync(Guid id); // Adicionado no Exercício 2
}

/// <summary>
/// Serviço de regras de negócio para gerenciamento de URLs encurtadas.
/// </summary>
public class UrlsService : IUrlsService
{
    private readonly IUrlContext _context;

    public UrlsService(IUrlContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Cria um novo registro de URL encurtada.
    /// </summary>
    /// <remarks>
    /// IMPLEMENTAÇÃO PENDENTE (fase RED do TDD) — será implementada na próxima aula.
    /// </remarks>
    public Task<UrlItem> CreateShortUrlAsync(string originalUrl)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Busca um registro de URL encurtada pelo seu Id.
    /// </summary>
    /// <remarks>
    /// IMPLEMENTAÇÃO PENDENTE (fase RED do TDD - Exercício 2).
    /// </remarks>
    public Task<UrlItem?> GetUrlByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}