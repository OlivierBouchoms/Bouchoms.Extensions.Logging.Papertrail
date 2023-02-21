using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Bouchoms.Extensions.Logging.Papertrail
{
    public static class PapertrailLoggerFactoryExtensions
    {
        /// <summary>
        /// Configures logging to Papertrail for the ILoggingBuilder
        /// </summary>
        /// <param name="builder">ILoggingBuilder</param>
        /// <returns>ILoggingBuilder</returns>
        public static ILoggingBuilder AddPapertrail(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, PapertrailLoggerProvider>());
            
            LoggerProviderOptions.RegisterProviderOptions<PapertrailOptions, PapertrailLoggerProvider>(builder.Services);
            
            return builder;
        }
    }
}