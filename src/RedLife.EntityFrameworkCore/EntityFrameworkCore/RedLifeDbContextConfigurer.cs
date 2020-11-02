using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace RedLife.EntityFrameworkCore
{
    public static class RedLifeDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<RedLifeDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<RedLifeDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
