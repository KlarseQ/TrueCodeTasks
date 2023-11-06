namespace UserService;

internal static class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        InitDataBase(host);
        host.Run();
    }
    
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var configuration = hostContext.Configuration;
                var connectionString = configuration.GetValue<string>("AppConnection");

                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString));
                services.AddScoped<IUserRepository, UserRepository>();
            });

    private static void InitDataBase(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var yourService = services.GetRequiredService<AppDbContext>();
        yourService.Database.Migrate();
    }
}