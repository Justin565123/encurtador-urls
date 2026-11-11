using EncurtadorURLs.Data;
using EncurtadorURLs.Models;

namespace EncurtadorURLs.Services;

/// <summary>
/// Interface para o serviço de URLs. Define as operações de negócio disponíveis.
/// </summary>
public interface IUrlsService
{
    Task<UrlItem> CreateShortUrlAsync(string originalUrl);
    Task<UrlItem?> GetUrlByIdAsync(Guid id);
}

/// <summary>
/// Serviço de regras de negócio para gerenciamento de URLs encurtadas.
/// </summary>
public class UrlsService : IUrlsService
{
    private const int MaxShortCodeRetries = 5;
    private const int MinShortCodeLength = 6;
    private const int MaxShortCodeLength = 8;
    private const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    private readonly IUrlContext _context;
    private readonly Random _random = new();

    public UrlsService(IUrlContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Cria um novo registro de URL encurtada.
    /// </summary>
    public async Task<UrlItem> CreateShortUrlAsync(string originalUrl)
    {
        if (string.IsNullOrWhiteSpace(originalUrl))
            throw new ArgumentException("URL original não pode ser vazia.", nameof(originalUrl));

        if (!IsValidUrl(originalUrl))
            throw new ArgumentException("URL original possui formato inválido. Deve iniciar com http:// ou https://", nameof(originalUrl));

        if (originalUrl.Length > 2048)
            throw new ArgumentException("URL original não pode exceder 2048 caracteres.", nameof(originalUrl));

        var shortCode = await GenerateUniqueShortCodeAsync();
        var urlItem = new UrlItem
        {
            Id = Guid.NewGuid(),
            OriginalUrl = originalUrl,
            ShortCode = shortCode,
            CreatedAt = DateTimeOffset.UtcNow,
            ClickCount = 0
        };

        await _context.AddAsync(urlItem);
        return urlItem;
    }

    /// <summary>
    /// Busca um registro de URL encurtada pelo seu Id.
    /// </summary>
    public async Task<UrlItem?> GetUrlByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID não pode ser vazio.", nameof(id));

        return await _context.GetByIdAsync(id);
    }

    #region Métodos Auxiliares Privados

    private bool IsValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        try
        {
            var uri = new Uri(url);
            return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
        }
        catch
        {
            return false;
        }
    }

    private async Task<string> GenerateUniqueShortCodeAsync()
    {
        for (var attempt = 0; attempt < MaxShortCodeRetries; attempt++)
        {
            var shortCode = GenerateRandomShortCode();
            if (!await _context.ShortCodeExistsAsync(shortCode))
                return shortCode;
        }

        throw new InvalidOperationException($"Não foi possível gerar um código curto único após {MaxShortCodeRetries} tentativas.");
    }

    private string GenerateRandomShortCode()
    {
        var length = _random.Next(MinShortCodeLength, MaxShortCodeLength + 1);
        var result = new char[length];

        for (var index = 0; index < result.Length; index++)
            result[index] = Base62Chars[_random.Next(Base62Chars.Length)];

        return new string(result);
    }

    #endregion
}