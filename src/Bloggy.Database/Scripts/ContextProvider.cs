using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bloggy.Database.Scripts
{
    public class ContextProvider : IDesignTimeDbContextFactory<BloggyDbContext>
    {
        public BloggyDbContext CreateDbContext(string[] args)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<BloggyDbContext>()
                .UseSqlServer(args[0]);
            

            return new BloggyDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
