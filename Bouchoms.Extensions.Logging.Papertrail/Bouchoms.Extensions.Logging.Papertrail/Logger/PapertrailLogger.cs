using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Bouchoms.Extensions.Logging.Papertrail
{
    public class PapertrailLogger : ILogger
    {
        private readonly PapertrailOptions _options;

        private readonly string _name;


        private readonly string _authorization;
        private static readonly HttpClient Client = new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(5)
        });

        private static readonly MediaTypeHeaderValue ContentTypeHeader = MediaTypeHeaderValue.Parse("text/plain");


        public PapertrailLogger(string name, PapertrailOptions options)
        {
            _name = name;
            _options = options;
            _authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(options.AccessToken));
        }

        public IDisposable BeginScope<TState>(TState state) => default;
        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None; // Is already handled by .NET logging

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string text = $"{logLevel}: {formatter(state, exception)}";

            ThreadPool.QueueUserWorkItem(async _ => await DoLog(text));
        }

        private async Task DoLog(string text)
        {
            using (HttpRequestMessage request = new(HttpMethod.Post, _options.Url))
            {
                request.Headers.TryAddWithoutValidation("Authorization", $"Basic {_authorization}");

                request.Content = new StringContent(text);
                request.Content.Headers.ContentType = ContentTypeHeader;

                try
                {
                    await Client.SendAsync(request);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"An exception occured while attempting to log to Papertrail. {e.GetType().FullName}: {e.Message}");
                }
            }
        }
    }
}