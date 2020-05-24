using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tweetbook.Domain;

namespace Tweetbook.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        // This represents a Posts table. Note that we have used the domain object for now, but should switch to a DTO in the future
        // if the database expects a different interface than given by the domain object
        public DbSet<Post> Posts { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
