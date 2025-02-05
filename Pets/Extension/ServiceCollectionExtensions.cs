using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Sinks.OpenSearch;
using System;
using System.Text;
using Pets.Config;
using Pets.Data;
using Pets.EventHandlers;
using Pets.Filters;
using Pets.Interfaces.Events;
using Pets.Interfaces.Repositories;
using Pets.Interfaces.Services;
using Pets.Repositories;
using Pets.Services;
using Pets.Validators;
using Pets.Mapper;  // For MappingProfile

namespace Pets.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPostgresDbContext(this IServiceCollection services)
        {
            services.AddDbContext<PetDbContext>((serviceProvider, options) =>
            {
                var dbConfig = serviceProvider.GetRequiredService<IOptions<DatabaseConfig>>().Value;
                var connectionString = dbConfig.BuildConnectionString();

                // Log the connection string for debugging purposes (sanitized).
                Log.Debug("Using connection string: {ConnectionString}", dbConfig.SanitizeConnectionString());

                options.UseNpgsql(connectionString);
            });
            return services;
        }

        public static IServiceCollection AddControllersWithValidators(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                // Register a logging action filter.
                options.Filters.Add<LoggingActionFilter>();
            })
            .AddFluentValidation(fv =>
            {
                // Register validators from this assembly.
                fv.RegisterValidatorsFromAssemblyContaining<CreatePetValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UpdatePetValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<DeletePetValidator>();
            });

            return services;
        }

        public static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            // Register the event handlers.
            services.AddScoped<IPetEventHandler, PetCreatedEventHandler>();
            services.AddScoped<IPetEventHandler, PetUpdatedEventHandler>();
            services.AddScoped<IPetEventHandler, PetDeletedEventHandler>();

            // Register the event dispatcher.
            services.AddScoped<IEventDispatcher, Pets.Events.EventDispatcher>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Register repository implementations.
            services.AddScoped<IPetRepository, PetRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Register business services.
            services.AddScoped<IPetService, PetService>();

            return services;
        }

        public static IServiceCollection AddPetsMappingProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }

        public static IServiceCollection AddLoggingFilters(this IServiceCollection services)
        {
            services.AddScoped<LoggingActionFilter>();
            return services;
        }

        public static IServiceCollection ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.OpenSearch(new OpenSearchSinkOptions(new Uri(configuration["SERILOG_ES_URI"] ?? "http://localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "pets-service-logs-{0:yyyy.MM.dd}",
                    ModifyConnectionSettings = x =>
                    {
                        var username = configuration["SERILOG_ES_USER"];
                        var password = configuration["SERILOG_ES_PASS"];
                        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                        {
                            x.BasicAuthentication(username, password);
                        }
                        return x;
                    }
                })
                .CreateLogger();

            services.AddSingleton<Serilog.ILogger>(Log.Logger);
            return services;
        }

        public static IServiceCollection RegisterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind and validate DatabaseConfig.
            services.Configure<DatabaseConfig>(configuration.GetSection("DatabaseConfig"));
            var dbConfig = configuration.GetSection("DatabaseConfig").Get<DatabaseConfig>();
            if (dbConfig == null || string.IsNullOrEmpty(dbConfig.Host))
                throw new InvalidOperationException("DatabaseConfig is missing or invalid.");

            Log.Debug("DatabaseConfig Loaded: {@DatabaseConfig}", dbConfig);

            return services;
        }

        public static IServiceCollection ConfigurePetsServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureSerilog(configuration)
                    .RegisterConfiguration(configuration)
                    .AddPostgresDbContext()
                    .AddControllersWithValidators()
                    .AddEventHandlers()
                    .AddRepositories()
                    .AddServices()
                    .AddPetsMappingProfiles()
                    .AddLoggingFilters();

            // Optionally add Swagger:
            services.AddSwaggerGen();

            return services;
        }
    }
}
