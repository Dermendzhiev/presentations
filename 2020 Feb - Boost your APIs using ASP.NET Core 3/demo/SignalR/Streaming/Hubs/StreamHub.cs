namespace StreamR
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    public class StreamHub : Hub
    {
        private readonly StreamManager streamManager;

        public StreamHub(StreamManager streamManager) => this.streamManager = streamManager;

        public List<string> ListStreams() => this.streamManager.ListStreams();

        public IAsyncEnumerable<string> WatchStream(string streamName, CancellationToken cancellationToken) => this.streamManager.Subscribe(streamName, cancellationToken);

        public async Task StartStream(string streamName, IAsyncEnumerable<string> streamContent)
        {
            try
            {
                Task streamTask = this.streamManager.RunStreamAsync(streamName, streamContent);

                // Tell everyone about your stream!
                await this.Clients.Others.SendAsync("NewStream", streamName);

                await streamTask;
            }
            finally
            {
                await this.Clients.Others.SendAsync("RemoveStream", streamName);
            }
        }
    }
}