using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RedLife.Configuration;
using RedLife.Web;

namespace RedLife.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class RedLifeDbContextFactory : IDesignTimeDbContextFactory<RedLifeDbContext>
    {
        public RedLifeDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<RedLifeDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            RedLifeDbContextConfigurer.Configure(builder, configuration.GetConnectionString(RedLifeConsts.ConnectionStringName));

            return new RedLifeDbContext(builder.Options);
        }
    }
}
