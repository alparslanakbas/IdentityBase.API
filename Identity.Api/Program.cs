var logger = LogManager.Setup()
                          .LoadConfigurationFromAppSettings()
                          .GetCurrentClassLogger();

    var builder = WebApplication.CreateBuilder(args);

    // NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddHttpClient();

    // PostgreSQL
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"));
    });

    // Scopes
    builder.Services.AddScoped<ILogRepository, LogRepository>();

    // Identity Configuration
    builder.Services.AddIdentityConfiguration();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

// Database Migration ve Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();

    try
    {
        db.Database.Migrate();
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<Role>>();
        await UserSeed.SeedAsync(userManager, roleManager);
        logger.Info("Database migration ve seed i�lemleri ba�ar�l�");
    }
    catch (Exception)
    {
        logger.Error("Database migration veya seed s�ras�nda hata olu�tu");

        try
        {
            await db.Database.EnsureDeletedAsync();
            logger.Info("Hata sonras� database silindi");
        }
        catch (Exception)
        {
            logger.Error("Database silinirken hata olu�tu");
            throw;
        }

        throw;
    }
}

if (app.Environment.IsDevelopment())
    {
        logger.Info("Uygulama Development modunda ba�lat�ld�");
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
