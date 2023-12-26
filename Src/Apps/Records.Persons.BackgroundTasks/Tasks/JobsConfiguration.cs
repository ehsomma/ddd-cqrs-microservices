using Quartz;

namespace Records.Persons.BackgroundTasks.Tasks;

/// <summary>
/// Represents a Job configurator.
/// </summary>
public static class JobsConfiguration
{
    #region Public methods

    /// <summary>
    /// Registers the jobs (Option 1).
    /// </summary>
    /// <remarks>
    /// NOTE: This method must be used as delegate (Action) in the AddQuartzCustom extension method
    /// if you want to register Jobs from this method.
    ///
    /// NOTE: Delete this class if you chose the Option 2.
    /// (See: Records.Shared.Infra.Quartz.DependencyInjection.DependencyInjection.AddQuartzCustom class).
    /// </remarks>
    /// <param name="quartzConfig">Quartz configurator.</param>
    /// <param name="appConfig">The IConfiguration to read schedule settings from appsettings.json.</param>
    public static void Configure(IServiceCollectionQuartzConfigurator quartzConfig, IConfiguration appConfig)
    {
        // NOTE: Use this lines as example if you chose Option 1.
        ////quartzConfig.AddJobAndTrigger<FirstJob>(appConfig);
        ////quartzConfig.AddJobAndTrigger<SecondJob>(appConfig);
    }

    #endregion
}
