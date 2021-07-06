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
        private readonly string _name;
        private readonly PapertrailOptions _options;

        private static readonly HttpClient Client = new();

        private readonly string _authorization;
        
        private readonly string _application;

        // Four spaces indent
        private const string Indent = "    ";

        public PapertrailLogger(string name, PapertrailOptions options)
        {
            _name = name;
            _options = options;
            _authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(options.AccessToken));
            _application = Assembly.GetEntryAssembly()?.GetName().Name;
        }

        public IDisposable BeginScope<TState>(TState state) => default;
        public bool IsEnabled(LogLevel logLevel) => logLevel >= _options.LogLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            string text = $"{Indent}{_application}\n \n{_name} - {formatter(state, exception)}";
            
            ThreadPool.QueueUserWorkItem(async _ => await DoLog(text));
        }

        private async Task DoLog(string text)
        {
            using (HttpRequestMessage request = new(HttpMethod.Post, _options.Url))
            {
                request.Headers.TryAddWithoutValidation("Authorization", $"Basic {_authorization}"); 
                
                request.Content = new StringContent(text);
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/plain");

                try
                {
                    await Client.SendAsync(request);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occured while attempting to log to Papertrail. Error: {e.Message}\nOriginal message:\n{e.Message}");
                }
            }
        } 
    }
}