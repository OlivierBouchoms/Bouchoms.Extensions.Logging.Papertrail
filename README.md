# Microsoft.Extensions.Logging.Papertrail

NuGet package that simplifies logging to Papertrail when using Microsoft.Extensions.Logging.

## Getting started

### Adding dependency

// TODO

### Configuration
Add the configuration to your `appsettings.json` file.

```
{
  "Microsoft.Extensions.Logging.Papertrail": {
    "AccessToken": "your-access-token",
    "Url": "your-papertrail-url",
    "LogLevel": "Information"
  },
}
```

If you don't want to the appsettings, you can use environment variables instead. These should be set to the keys `PAPERTRAIL_URL`, `PAPERTRAIL_ACCESS_TOKEN` and `PAPERTRAIL_LOG_LEVEL`.

### Code
In your code, call `ILoggingBuilder.AddPapertrail()` to add logging to Papertrail. See [Logging in .NET Core and ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/) for more information. 