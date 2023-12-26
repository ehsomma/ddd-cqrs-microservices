#region Usings

using Quartz;
using Serilog;

#endregion

namespace Records.Shared.Infra.Quartz;

/// <summary>
/// Represents a wrapper of the Job class to inject a try/catch in the Execute method to log exceptions.
/// </summary>
/// <typeparam name="TJob">The type of our job.</typeparam>
public abstract class JobTry<TJob> : IJob
{
    #region Public methods

    /// <summary>
    /// Executes the Job called by the <see cref="IScheduler" /> when a <see cref="ITrigger" />
    /// fires that is associated with the <see cref="IJob" />.
    /// </summary>
    /// <param name="context">The execution context.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public abstract Task TryExecute(IJobExecutionContext context);

    /// <inheritdoc />
    /// <remarks>
    /// This method will be fired by Quartz.net and then it wraps with a try/catch and executes our
    /// implementation.
    /// NOTE: This wrap is to avoid writing the try/catch in every Quartz.Job implementation.
    /// </remarks>
    /// <param name="context">The execution context.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            // Executes implementation in the derived class.
            await TryExecute(context);
        }
        catch (Exception ex)
        {
            // Log the exception.
            Log.Error(ex, ex.Message);

            throw;
        }
    }

    #endregion
}
