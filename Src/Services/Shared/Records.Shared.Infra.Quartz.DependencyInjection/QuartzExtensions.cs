using Microsoft.Extensions.Configuration;

////namespace Records.Shared.Infra.Quartz.DependencyInjection;
#pragma warning disable IDE0130 // Namespace does not match folder structure
//// ReSharper disable once CheckNamespace
namespace Quartz;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Extensions methods for Quartz.
/// </summary>
public static class QuartzExtensions
{
    #region Public methods

    /// <summary>
    /// Registers the job, loading the schedule from appsettings.
    /// </summary>
    /// <typeparam name="TJob">The job class type (The class that implements IJob).</typeparam>
    /// <param name="quartzConfig">Quartz configurator.</param>
    /// <param name="appConfig">The IConfiguration to read schedule settings from appsettings.json.</param>
    /// <exception cref="Exception">When no Quartz.NET Cron schedule found for job in appsettings.</exception>
    public static void AddJobAndTrigger<TJob>(
        this IServiceCollectionQuartzConfigurator quartzConfig,
        IConfiguration appConfig)
        where TJob : IJob
    {
        ArgumentNullException.ThrowIfNull(appConfig);

        // From: https://andrewlock.net/using-quartz-net-with-asp-net-core-and-worker-services/.

        // Uses the name of the IJob as the appsettings.json key.
        string jobName = typeof(TJob).Name;

        // Try and load the schedule from configuration.
        string configKey = $"Quartz:{jobName}";
        string? cronSchedule = appConfig[configKey];

        // Some minor validation.
        if (string.IsNullOrEmpty(cronSchedule))
        {
            throw new Exception($"No Quartz.NET Cron schedule found for job in appsettings at {configKey}");
        }

        // Registers the job.
        JobKey jobKey = new (jobName);
        quartzConfig.AddJob<TJob>(opts => opts.WithIdentity(jobKey));

        // Registers the trigger for the job.
        quartzConfig.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity(jobName + "-trigger")
            .WithCronSchedule(cronSchedule)); // Uses the schedule from appsettings.json.
    }

    /// <summary>
    /// Registers the job, loading the schedule from appsettings.
    /// </summary>
    /// <param name="quartzConfig">Quartz configurator.</param>
    /// <param name="appConfig">The IConfiguration to read schedule settings from appsettings.json.</param>
    /// <param name="jobType">The type class of the job.</param>
    /// <exception cref="Exception">When no Quartz.NET Cron schedule found for job in appsettings.</exception>
    public static void AddJobAndTrigger(
        this IServiceCollectionQuartzConfigurator quartzConfig,
        IConfiguration appConfig,
        Type jobType)
    {
        ArgumentNullException.ThrowIfNull(appConfig);
        ArgumentNullException.ThrowIfNull(jobType);

        // From: https://andrewlock.net/using-quartz-net-with-asp-net-core-and-worker-services/.

        // Uses the name of the IJob as the appsettings.json key.
        string jobName = jobType.Name;

        // Try and load the schedule from configuration.
        string configKey = $"Quartz:{jobName}";
        string? cronSchedule = appConfig[configKey];

        // Some minor validation.
        if (string.IsNullOrEmpty(cronSchedule))
        {
            throw new Exception($"No Quartz.NET Cron schedule found for job in appsettings at {configKey}");
        }

        // Registers the job.
        JobKey jobKey = new (jobName);
        ////quartzConfig.AddJob<TJob>(opts => opts.WithIdentity(jobKey));
        quartzConfig.AddJob(jobType, jobKey, opts => opts.WithIdentity(jobKey));

        // Registers the trigger for the job.
        quartzConfig.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity(jobName + "-trigger")
            .WithCronSchedule(cronSchedule)); // Uses the schedule from appsettings.json.
    }

    #endregion
}
