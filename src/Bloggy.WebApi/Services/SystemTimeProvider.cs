
namespace Bloggy.WebApi.Services
{
    public class SystemTimeProvider : ITimeProvider
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

        public DateTimeOffset LocalNow => DateTimeOffset.Now;
    }
}
