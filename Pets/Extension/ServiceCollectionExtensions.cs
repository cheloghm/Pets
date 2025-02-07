using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.OpenSearch;
using System;
using Pets.Config;
using Pets.EventHandlers;
using Pets.Filters;
using Pets.Interfaces.Events;
using Pets.Interfaces.Repositories;
using Pets.Interfaces.Services;
using Pets.Repositories;
using Pets.Services;
using Pets.Validators;
using Pets.Mapper;
using MongoDB.Driver;

namespace Pets.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControllersWithValidators(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<LoggingActionFilter>();
            })
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<CreatePetValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<UpdatePetValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<DeletePetValidator>();
            });

            return services;
        }

        public static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            services.AddScoped<IPetEventHandler, PetCreatedEventHandler>();
            services.AddScoped<IPetEventHandler, PetUpdatedEventHandler>();
            services.AddScoped<IPetEventHandler, PetDeletedEventHandler>();

            services.AddScoped<IEventDispatcher, Pets.Events.EventDispatcher>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Register the MongoDB-based repository.
            services.AddScoped<IPetRepository, PetRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
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

        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind the MongoDb configuration section to MongoDbConfig.
            services.Configure<MongoDbConfig>(configuration.GetSection("MongoDb"));
            var mongoConfig = configuration.GetSection("MongoDb").Get<MongoDbConfig>();

            // Create a MongoClient and get the database.
            var client = new MongoClient(mongoConfig.ConnectionString);
            var database = client.GetDatabase(mongoConfig.DatabaseName);

            // Register the IMongoDatabase as a singleton.
            services.AddSingleton<IMongoDatabase>(database);
            return services;
        }

        public static IServiceCollection ConfigurePetsServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureSerilog(configuration)
                    .AddControllersWithValidators()
                    .AddMongoDb(configuration)
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
