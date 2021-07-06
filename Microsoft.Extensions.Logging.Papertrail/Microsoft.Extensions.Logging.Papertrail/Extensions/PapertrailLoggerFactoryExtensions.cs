using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;

namespace Microsoft.Extensions.Logging.Papertrail
{
    public static class PapertrailLoggerFactoryExtensions
    {
        private static readonly string UrlKey = "PAPERTRAIL_URL";
        private static readonly string TokenKey = "PAPERTRAIL_ACCESS_TOKEN";
        private static readonly string LogLevelKey = "PAPERTRAIL_LOG_LEVEL";

        private static readonly string AppSettingsSection = "Microsoft.Extensions.Logging.Papertrail";

        /// <summary>
        /// Configures logging to Papertrail for the ILoggingBuilder
        /// </summary>
        /// <param name="builder">ILoggingBuilder</param>
        /// <returns>ILoggingBuilder</returns>
        /// <exception cref="ArgumentNullException">Url or access token was not provided</exception>
        public static ILoggingBuilder AddPapertrail(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();
            
            ConfigureOptions(builder);

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, PapertrailLoggerProvider>());
            
            LoggerProviderOptions.RegisterProviderOptions<PapertrailOptions, PapertrailLoggerProvider>(builder.Services);
            
            return builder;
        }

        private static void ConfigureOptions(ILoggingBuilder builder)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string appSettingsFile = $"appsettings.json";
            string appSettingsFileEnv = $"appsettings.{environment}.json";
            
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), appSettingsFile), true)
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), appSettingsFileEnv), true)
                .Build();

            if (configuration.GetSection(AppSettingsSection).Exists())
            {
                // Use values from appsettings if defined
                builder.Services.AddOptions<PapertrailOptions>()
                    .Bind(configuration.GetSection(AppSettingsSection))
                    .ValidateDataAnnotations();
            }
            else
            {
                // Use values from environment variables
                string url = Environment.GetEnvironmentVariable(UrlKey);
                string accessToken = Environment.GetEnvironmentVariable(TokenKey);

                if (!Enum.TryParse(Environment.GetEnvironmentVariable(LogLevelKey), true, out LogLevel logLevel))
                {
                    throw new ArgumentException(nameof(logLevel));
                }

                PapertrailOptions papertrailOptions = new()
                {
                    Url = url,
                    AccessToken = accessToken,
                    LogLevel = logLevel
                };
                
                builder.Services.AddOptions<PapertrailOptions>()
                    .Configure(options => options = papertrailOptions)
                    .ValidateDataAnnotations();
            }
        }
    }
}