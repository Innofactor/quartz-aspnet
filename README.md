# ASP.NET Core extensions for [Quartz.NET](https://www.quartz-scheduler.net/)

## Usage example

In Startup.cs:

```csharp

    public void ConfigureServices(IServiceCollection services) {
      // ...
      services.UseQuartz(typeof(HelloJob), typeof(HommaJob));
    }

```

