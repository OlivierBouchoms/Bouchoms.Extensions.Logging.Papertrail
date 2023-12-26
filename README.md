# Bouchoms.Extensions.Logging.Papertrail

NuGet package that simplifies logging to [Papertrail](https://www.papertrail.com/) when using Microsoft.Extensions.Logging.

## Getting started

### NuGet

https://www.nuget.org/packages/Bouchoms.Extensions.Logging.Papertrail/

### .NET 8

```
dotnet add package Bouchoms.Extensions.Logging.Papertrail --version 8.0.0
```

For development environments, add the configuration to your `appsettings.json` file. This feature is not safe for realworld usage. Refer to [Security and user secrets](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-8.0#security-and-user-secrets) documentation to safely configure the credentials for realworld usage.

```
{
  "Bouchoms.Extensions.Logging.Papertrail": {
    "AccessToken": "your-access-token",
    "Url": "your-papertrail-url"
  },
}
```

### .NET 6

```
dotnet add package Bouchoms.Extensions.Logging.Papertrail --version 6.0.0
```

For development environments, add the configuration to your `appsettings.json` file. This feature is not safe for realworld usage. Refer to [Security and user secrets](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0#security-and-user-secrets) documentation to safely configure the credentials for realworld usage.

```
{
  "Bouchoms.Extensions.Logging.Papertrail": {
    "AccessToken": "your-access-token",
    "Url": "your-papertrail-url"
  },
}
```

### Code

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
