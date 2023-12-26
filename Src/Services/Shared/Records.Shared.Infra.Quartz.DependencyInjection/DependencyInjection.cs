#region Usings

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Reflection;

#endregion

namespace Records.Shared.Infra.Quartz.DependencyInjection;

/// <summary>
/// Extensions methods for dependency injection.
/// </summary>
public static class DependencyInjection
{
    #region Public methods

    /// <summary>
    /// Configures Quartz and Jobs.
    /// </summary>
    /// <remarks>
    /// Option 1: Using the configurator Action to register the Jobs.
    /// Option 2: Iterating the assembly to search Jobs.
    /// </remarks>
    /// <param name="services">The service collection.</param>
    /// <param name="appConfig">The IConfiguration to read schedule settings from appsettings.json.</param>
    /// <param name="configurator">The Quartz configurator Action (if null, it will iterate through the assembly to search Jobs).</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddQuartzCustom(
        this IServiceCollection services,
        IConfiguration appConfig,
        Action<IServiceCollectionQuartzConfigurator, IConfiguration>? configurator = null)
    {
        // Doc: https://www.quartz-scheduler.net/documentation/quartz-3.x/packages/microsoft-di-integration.html#persistent-job-stores.
        // Cron generator: https://www.freeformatter.com/cron-expression-generator-quartz.html.
        services.AddQuartz(configure =>
        {
            ////configure
            ////    .AddJob<Job1>(Job1.Key)
            ////    .AddTrigger(
            ////        trigger => trigger.ForJob(Job1.Key)
            ////            .WithSimpleSchedule(
            ////                schedule =>
            ////                    schedule.WithIntervalInSeconds(10).RepeatForever()));

            ////configure
            ////    .AddJob<Job2>(Job2.Key)
            ////    .AddTrigger(trigger => trigger
            ////        .ForJob(Job2.Key)
            ////        .WithCronSchedule("0/5 * * * * ?"));

            // Adds the jobs.
            if (configurator != null)
            {
                // Using delegate action.
                // Require a class with a method thath adds the jobs with AddJobAndTrigger<T>.
                configurator(configure, appConfig);
            }
            else
            {
                // Iterate through the assembly, get the types of the classes that implements IJob and
                // ends with "Job", and finaly adds the jobs with AddJobAndTrigger().
                Assembly myAssembly = Assembly.GetEntryAssembly() !;
                IEnumerable<Type> myTypes = myAssembly.GetTypes()
                    .Where(t =>
                        t.Name.EndsWith("Job")
                        && t.GetInterfaces().Contains(typeof(IJob)));
                foreach (Type type in myTypes)
                {
                    Console.WriteLine(type.FullName);
                    configure.AddJobAndTrigger(appConfig, type);
                }
            }

            // Tells Quartz.NET to register an IJobFactory that creates jobs by fetching them from
            // the DI container. The jobs will use scoped services, not just singleton or transient.
            // MODI: In Quart 3.8.0 it is no longer neded.
            ////configure.UseMicrosoftDependencyInjectionJobFactory();

            configure.SchedulerId = "records-persosns-quartz-Scheduler";
            configure.SchedulerName = "Records.Persosns - Quartz.NET Scheduler";
            ////configure.MaxBatchSize = 5; // Dafault 1.
            ////configure.UseDefaultThreadPool(1);
        });

        // Configures Quartz.NET with the worker service framework in .NET, which allows us
        // to run background tasks.
        services.AddQuartzHostedService(options =>
        {
            // When shutdown is requested, this setting ensures that Quartz.NET waits for the jobs
            // to end gracefully before exiting.
            options.WaitForJobsToComplete = true;
        });

        return services;
    }

    #endregion
}
