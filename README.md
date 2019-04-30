# ASP.NET Core extensions for [Quartz.NET](https://www.quartz-scheduler.net/)

![status badge](https://innofactor-agile.visualstudio.com/_apis/public/build/definitions/8f49bcda-8276-4721-8f2e-aa1f54924edf/18/badge)

## Usage example

In Startup.cs:

```csharp

    /// Register scheduler and jobs for DI ///
    public void ConfigureServices(IServiceCollection services) {
      // ...
      services.UseQuartz(typeof(HelloJob), typeof(HommaJob));
    }
    
    public void Configure(IApplicationBuilder app, IHostingEnvironment env) {

      var configuration = app.ApplicationServices.GetService<IOptions<AppConfig>>();
      var parameters = configuration.Value;
      var trigger = TriggerBuilder.Create()
        .WithIdentity(nameof(HelloJob))
        .WithCalendarIntervalSchedule(x => x.WithIntervalInMinutes(parameters.HelloJobIntervalInMinutes).PreserveHourOfDayAcrossDaylightSavings(true))
        .Build();

      // Fetch IScheduler whenever afterwards from DI container and configure schedules for jobs
      var scheduler = app.ApplicationServices.GetService<IScheduler>();
      scheduler.StartJob<HelloJob>(trigger);
    }

```

