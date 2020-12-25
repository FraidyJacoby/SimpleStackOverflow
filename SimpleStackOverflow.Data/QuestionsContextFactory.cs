using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SimpleStackOverflow.Data
{
    public class QuestionsContextFactory : IDesignTimeDbContextFactory<QuestionsContext>
    {
        public QuestionsContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}SimpleStackOverflow.Web"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new QuestionsContext(config.GetConnectionString("ConStr"));
        }
    }
}
