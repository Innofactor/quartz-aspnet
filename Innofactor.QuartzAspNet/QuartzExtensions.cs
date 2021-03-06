﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace Innofactor.QuartzAspNet {

  public static class QuartzExtensions {

    /// <summary>
    /// Register Quartz.NET and a list of jobs for the DI container.
    /// </summary>
    /// <param name="services">Services.</param>
    /// <param name="jobs">List of Quartz jobs to register.</param>
    public static void UseQuartz(this IServiceCollection services, params Type[] jobs) {

      services.AddSingleton<IJobFactory, BatchJobFactory>();
      services.Add(jobs.Select(jobType => new ServiceDescriptor(jobType, jobType, ServiceLifetime.Transient)));

      services.AddSingleton(provider => {

        var props = new NameValueCollection {
          { "quartz.serializer.type", "json" }
        };
        var schedulerFactory = new StdSchedulerFactory(props);
        var scheduler = schedulerFactory.GetScheduler().Result;
        scheduler.JobFactory = provider.GetService<IJobFactory>();
        scheduler.Clear();
        scheduler.Start();

        return scheduler;
      });

    }

    public static void StartJob<TJob>(this IScheduler scheduler, ITrigger trigger)
      where TJob : IJob {

      var jobName = typeof(TJob).Name;

      var job = JobBuilder.Create<TJob>()
        .WithIdentity(jobName)
        .Build();

      scheduler.ScheduleJob(job, trigger);
    }

  }

}
