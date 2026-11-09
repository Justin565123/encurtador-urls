namespace EncurtadorURLs.Models;

/// <summary>
/// Representa um registro de URL encurtada no sistema.
/// </summary>
public class UrlItem
{
    public Guid Id { get; set; }
    public string OriginalUrl { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public long ClickCount { get; set; }
}
