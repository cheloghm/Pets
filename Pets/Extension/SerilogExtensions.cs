using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.OpenSearch;
using System;
using Serilog.Events; // for RollingInterval
using Serilog.Extensions.Hosting; // for UseSerilog()

namespace Pets.Extensions
{
    public static class SerilogExtensions
    {
        public static IHostBuilder ConfigureSerilog(this IHostBuilder hostBuilder, IConfiguration configuration)
        {
            var esUri = configuration["SERILOG_ES_URI"] ?? "http://localhost:9200";
            var username = configuration["SERILOG_ES_USER"];
            var password = configuration["SERILOG_ES_PASS"];

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.OpenSearch(new OpenSearchSinkOptions(new Uri(esUri))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "pets-service-logs-{0:yyyy.MM.dd}",
                    ModifyConnectionSettings = x =>
                    {
                        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                        {
                            x.BasicAuthentication(username, password);
                        }
                        return x;
                    }
                })
                .CreateLogger();

            return hostBuilder.UseSerilog();
        }
    }
}
