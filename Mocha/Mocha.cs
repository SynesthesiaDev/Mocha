using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Mocha.Authentication;
using Mocha.Components;
using Mocha.Endpoints;
using Mocha.Models.Configuration;
using Nocturne.Database;
using Serilog;
using Serilog.Sinks.SpectreConsole;
using SynesthesiaDev.ConfigLibrary;
using SynesthesiaDev.ConfigLibrary.Location;
using SynesthesiaDev.Synx.Codon;
using SynesthesiaDev.Synx.Types;

namespace Mocha;

public class Mocha
{
    public static readonly string DATA_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
    public static readonly string DATABASE_PATH = Path.Combine(DATA_PATH, "database.nocturne");
    public static readonly string PERSISTENT_KEYS_PATH = Path.Combine(DATA_PATH, "keys");

    public static readonly ConfigLib<Config, ISynxElement> CONFIG = ConfigLib.For<Config, ISynxElement>()
        .Encoding(Config.CODEC, SynxTranscoder.INSTANCE)
        .Default(Config.DEFAULT)
        .Location(ConfigLocation.DataFolder("config.synx"))
        .Build();

    public static readonly NocturneDatabase NOCTURNE_DATABASE = new NocturneDatabase
    {
        FilePath = DATABASE_PATH,
        CompactOnLaunch = true,
        AutomaticallyCompact = false
    };

    public static void Main(string[] args)
    {
        Directory.CreateDirectory(DATA_PATH);
        CONFIG.Load();
        NOCTURNE_DATABASE.Open();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.SpectreConsole(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u4}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();


        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<MochaContext>();
        builder.Services.AddSerilog();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(PERSISTENT_KEYS_PATH));
        builder.Services.AddDiscordAuthentication();

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        var app = builder.Build();

        app.MapMobileLinkEndpoints();
        app.MapMobileHealthSyncEndpoints();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            KnownIPNetworks = {},
            KnownProxies = {}
        });

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }


        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();

        app.UseAntiforgery();
        app.MapAuthEndpoints();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();


        app.Run();
    }
}
