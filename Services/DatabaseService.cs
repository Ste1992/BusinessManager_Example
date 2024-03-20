using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessManager.Services
{
    public class DatabaseService
    {
        private readonly MyDbContext _dbContext;

        public DatabaseService (MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public static IServiceProvider ConfigureServices()
        {
            // Configura i servizi
            return new ServiceCollection()
                .AddDbContext<MyDbContext>(options =>
                    options.UseSqlite("Data Source=/directory/to/database.db"))
                .AddTransient<DatabaseService>()
                .BuildServiceProvider();
        }

    }
}
