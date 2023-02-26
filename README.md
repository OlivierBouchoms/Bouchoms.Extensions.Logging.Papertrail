# Microsoft.Extensions.Logging.Papertrail

NuGet package that simplifies logging to Papertrail when using Microsoft.Extensions.Logging.

## Getting started

### Adding dependency

NuGet: https://www.nuget.org/packages/Bouchoms.Extensions.Logging.Papertrail/

#### .NET 6:

```
dotnet add package Bouchoms.Extensions.Logging.Papertrail --version 6.0.0-CI-20230221-214036
```

### Configuration
Add the configuration to your `appsettings.json` file. It's also possible to use other configuration sources. See [Configuration in ASP NET Core 6.0](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0).

```
{
  "Bouchoms.Extensions.Logging.Papertrail": {
    "AccessToken": "your-access-token",
    "Url": "your-papertrail-url"
  },
}
```

In your `Startup` class:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddPapertrail();

builder.Services.AddOptions<PapertrailOptions>()
                .Bind(builder.Configuration.GetSection(PapertrailOptions.ConfigurationSection))
                .ValidateDataAnnotations();
```

The logger provider is registered using the `Papertrail` alias. This makes it possible to customize logging behavior per scope.

```
{
  "Logging": {
    "LogLevel": { // No provider, LogLevel applies to all the enabled providers.
      "Default": "Error",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Warning"
    },
    "Debug": { // Debug provider.
      "LogLevel": {
        "Default": "Information" // Overrides preceding LogLevel:Default setting.
      }
    },
    "Papertrail": { // Papertrail provider.
      "LogLevel": {
        "Microsoft": "Information"
      }
    }
  }
}
```

See [Logging in .NET Core and ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/) for more information. 
