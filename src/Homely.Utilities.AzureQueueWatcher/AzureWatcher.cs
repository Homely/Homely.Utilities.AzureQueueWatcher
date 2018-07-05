using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Homely.Utilities.AzureQueueWatcher
{
    public class AzureWatcher
    {
        private readonly CloudQueue _queue;
        private int _queueCount;
        private readonly int _queueSizeWhenStartedWatching;
        private readonly Stopwatch _timer;
        private readonly string _azureConnectionString;
        private readonly string _queueName;

        public AzureWatcher(string azureConnectionString, string queueName)
        {
            if (string.IsNullOrWhiteSpace(azureConnectionString))
            {
                throw new ArgumentException(nameof(azureConnectionString));
            }

            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentException(nameof(queueName));
            }

            _azureConnectionString = azureConnectionString;
            _queueName = queueName;

            var storageAccount = CloudStorageAccount.Parse(azureConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            _queue = queueClient.GetQueueReference(queueName);

            _queueSizeWhenStartedWatching = GetQueueSizeAsync().Result;
            _timer = Stopwatch.StartNew();
        }

        private async Task<int> GetQueueSizeAsync()
        {
            await _queue.FetchAttributesAsync();
            return (int)_queue.ApproximateMessageCount;
        }

        public async Task DoWorkAsync()
        {
            try
            {
                Console.WriteLine("Homely.Utilities.AzureQueueWatcher");
                Console.WriteLine("==================================");
                Console.WriteLine($"Connection String: {_azureConnectionString}");
                Console.WriteLine($"Queue name: {_queueName}");
                Console.WriteLine("==================================");

                while (true)
                {
                    Console.WriteLine();

                    await WatchAsync();

                    Console.WriteLine();

                    await Task.Delay(5000);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception!: {exception}");
            }
        }

        private async Task WatchAsync()
        {
            _queueCount = await GetQueueSizeAsync();

            if (_queueCount > 0)
            {
                Console.WriteLine($">>>    Queue size: {_queueCount:N0}");

                var messagesLeft = _queueSizeWhenStartedWatching - _queueCount;

                if (messagesLeft > 0)
                {
                    var etaMs = (_timer.ElapsedMilliseconds / messagesLeft) * _queueCount;
                    Console.WriteLine($">>>    ETA until queue is empty: {etaMs / 1000/60} minutes");
                }
                else
                {
                    Console.WriteLine(">>>    ETA until queue is empty: (unknown)");
                }
            }
            else
            {
                Console.WriteLine(">>>    Queue is empty.");
            }
        }
    }
}