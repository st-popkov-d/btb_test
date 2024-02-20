namespace Bloggy.WebApi.Services
{
    public interface ITimeProvider
    {
        DateTimeOffset UtcNow { get; }
        DateTimeOffset LocalNow { get; }
    }
}
