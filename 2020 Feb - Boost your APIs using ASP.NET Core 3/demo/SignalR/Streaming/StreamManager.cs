namespace StreamR
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;

    public class StreamManager
    {
        private readonly ConcurrentDictionary<string, StreamHolder> streams = new ConcurrentDictionary<string, StreamHolder>();
        private long globalClientId;

        public List<string> ListStreams()
        {
            var streamList = new List<string>();
            foreach (KeyValuePair<string, StreamHolder> item in this.streams)
            {
                streamList.Add(item.Key);
            }
            return streamList;
        }

        public async Task RunStreamAsync(string streamName, IAsyncEnumerable<string> stream)
        {
            var streamHolder = new StreamHolder() { Source = stream };

            // Add before yielding
            // This fixes a race where we tell clients a new stream arrives before adding the stream
            this.streams.TryAdd(streamName, streamHolder);

            await Task.Yield();

            try
            {
                await foreach (string item in stream)
                {
                    foreach (KeyValuePair<long, Channel<string>> viewer in streamHolder.Viewers)
                    {
                        try
                        {
                            await viewer.Value.Writer.WriteAsync(item);
                        }
                        catch { }
                    }
                }
            }
            finally
            {
                this.RemoveStream(streamName);
            }
        }

        public void RemoveStream(string streamName)
        {
            this.streams.TryRemove(streamName, out StreamHolder streamHolder);
            foreach (KeyValuePair<long, Channel<string>> viewer in streamHolder.Viewers)
            {
                viewer.Value.Writer.TryComplete();
            }
        }

        public IAsyncEnumerable<string> Subscribe(string streamName, CancellationToken cancellationToken)
        {
            if (!this.streams.TryGetValue(streamName, out StreamHolder source))
            {
                throw new HubException("stream doesn't exist");
            }

            long id = Interlocked.Increment(ref this.globalClientId);

            var channel = Channel.CreateBounded<string>(options: new BoundedChannelOptions(2)
            {
                FullMode = BoundedChannelFullMode.DropOldest
            });

            source.Viewers.TryAdd(id, channel);

            // Register for client closing stream, this token will always fire (handled by SignalR)
            cancellationToken.Register(() =>
            {
                source.Viewers.TryRemove(id, out _);
            });

            return channel.Reader.ReadAllAsync();
        }

        private class StreamHolder
        {
            public IAsyncEnumerable<string> Source;
            public ConcurrentDictionary<long, Channel<string>> Viewers = new ConcurrentDictionary<long, Channel<string>>();
        }
    }
}