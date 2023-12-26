#region Usings

using Microsoft.AspNetCore.Mvc;
using Quartz;
using Records.Shared.Contracts;

#endregion

namespace Records.Countries.BackgroundTasks.Controllers;

/// <summary>
/// Controllers with endpoints to manage the background tasks.
/// </summary>
[ApiController]
[Produces("application/json")]
public class TasksSchedulerController : ControllerBase
{
    #region Declarations

    /// <summary>Quartz scheduler factory.</summary>
    private readonly ISchedulerFactory _schedulerFactory;

    #endregion

    #region Contructor

    /// <summary>
    /// Initializes a new instance of the <see cref="TasksSchedulerController"/> class.
    /// </summary>
    /// <param name="schedulerFactory">Quartz scheduler factory.</param>
    public TasksSchedulerController(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory ?? throw new ArgumentNullException(nameof(schedulerFactory));
    }

    #endregion

    #region Endpoints

    /// <summary>
    /// Starts the scheduler for the tasks.
    /// </summary>
    /// <returns>A string "Start ok" if the operation was successful. Otherwise, an <see cref="ErrorResponse"/>.</returns>
    /// <response code="500">If an unhandled error occurs.</response>
    [HttpPost]
    [Route("scheduler/start")]
    public async Task<string> Start()
    {
        IScheduler scheduler = await _schedulerFactory.GetScheduler();

        ////if (scheduler.IsShutdown)
        if (!scheduler.IsStarted)
        {
            await scheduler.Start();
            ////await scheduler.ResumeAll();
        }

        return "Start ok";
    }

    /// <summary>
    /// Stop the scheduler for the tasks.
    /// </summary>
    /// <param name="waitForJobsToComplete">If <see langword="true" />, the scheduler will not allow this method to return until all currently executing jobs have completed.</param>
    /// <returns>A string "Start ok" if the operation was successful. Otherwise, an <see cref="ErrorResponse"/>.</returns>
    /// <response code="500">If an unhandled error occurs.</response>
    [HttpPost]
    [Route("scheduler/stop")]
    public async Task<string> Stop(bool waitForJobsToComplete = true)
    {
        IScheduler scheduler = await _schedulerFactory.GetScheduler();

        if (scheduler.IsStarted)
        {
            await scheduler.Shutdown(waitForJobsToComplete);
            ////await scheduler.PauseAll();
        }

        return "Shutdown ok";
    }

    #endregion
}
