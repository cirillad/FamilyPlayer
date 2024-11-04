using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FamilyPlayer.Data; 
using Microsoft.Extensions.Configuration;
using FamilyPlayer.Data;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<MusicPlayerContext>(options =>
                    options.UseSqlServer(context.Configuration.GetConnectionString("MusicDb")));
            });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<MusicPlayerContext>();
            context.SeedData();
        }

        app.Run();
    }
}