using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Homely.Utilities.AzureQueueWatcher
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json");

            var configuration = builder.Build();
            var azure = configuration.GetSection("azure");
            var azureConnectionString = azure["connectionString"];
            var azureQueueName = azure["queueName"];

            var watcher = new AzureWatcher(azureConnectionString, azureQueueName);
            await watcher.DoWorkAsync();
        }
    }
}