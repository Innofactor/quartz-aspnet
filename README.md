# ASP.NET Core extensions for [Quartz.NET](https://www.quartz-scheduler.net/)

![status badge](https://innofactor-agile.visualstudio.com/_apis/public/build/definitions/8f49bcda-8276-4721-8f2e-aa1f54924edf/18/badge)

## Usage example

In Startup.cs:

```csharp

    public void ConfigureServices(IServiceCollection services) {
      // ...
      services.UseQuartz(typeof(HelloJob), typeof(HommaJob));
    }

```

