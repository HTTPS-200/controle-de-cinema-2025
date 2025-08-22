using Serilog;
using Serilog.Events;

namespace ControleDeCinema.WebApp.DependencyInjection;

public static class SerilogConfig
{
    public static void AddSerilogConfig(this IServiceCollection services, ILoggingBuilder logging, IConfiguration configuration)
    {
        var licenseKey = configuration["NEWRELIC_LICENSE_KEY"];

        var caminhoAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var caminhoArquivoLogs = Path.Combine(caminhoAppData, "ControleDeCinema", "erro.log");

        var loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(caminhoArquivoLogs, LogEventLevel.Error);


        if (!string.IsNullOrWhiteSpace(licenseKey))
        {
            loggerConfig.WriteTo.NewRelicLogs(
                endpointUrl: "https://log-api.newrelic.com/log/v1",
                applicationName: "controle-de-cinema-app",
                licenseKey: licenseKey
            );
        }

        Log.Logger = loggerConfig.CreateLogger();

        logging.ClearProviders();
        services.AddSerilog();
    }
}
