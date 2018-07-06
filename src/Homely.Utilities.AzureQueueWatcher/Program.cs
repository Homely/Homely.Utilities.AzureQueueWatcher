using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Homely.Utilities.AzureQueueWatcher
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", true)
                .AddEnvironmentVariables()
                .Build();

            var azureConnectionString = configuration.GetSection("AZURE_CONNECTION_STRING").Value;
            var azureQueueName = configuration.GetSection("AZURE_QUEUE_NAME").Value;

            var watcher = new AzureWatcher(azureConnectionString, azureQueueName);
            await watcher.DoWorkAsync();
        }
    }
}