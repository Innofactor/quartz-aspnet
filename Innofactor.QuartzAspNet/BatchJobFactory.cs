using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Threading.Tasks;

namespace Innofactor.QuartzAspNet {

  public class BatchJobFactory : IJobFactory {
    private readonly IServiceProvider _serviceProvider;

    public BatchJobFactory(IServiceProvider serviceProvider) {
      _serviceProvider = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler) {

      var scope = _serviceProvider.CreateScope();
      var jobDetail = bundle.JobDetail;
      var actualJob = (IJob)scope.ServiceProvider.GetService(jobDetail.JobType);
      return new ScopedJob(actualJob, scope);

    }

    public void ReturnJob(IJob job) {
      //TODO: Clean up logic if needed
    }
  }

  public class ScopedJob : IJob {

    private IJob actualJob;
    private IServiceScope scope;

    public ScopedJob(IJob actualJob, IServiceScope scope) {
      this.actualJob = actualJob;
      this.scope = scope;
    }

    public async Task Execute(IJobExecutionContext context) {
      await actualJob.Execute(context);
      scope.Dispose();
    }
  }

}
