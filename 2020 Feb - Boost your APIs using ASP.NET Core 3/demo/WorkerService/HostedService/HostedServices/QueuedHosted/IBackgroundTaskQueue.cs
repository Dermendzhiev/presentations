namespace HostedService.HostedServices.QueuedHosted
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);

        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
