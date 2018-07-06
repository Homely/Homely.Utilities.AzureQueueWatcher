# Homely.Utilities.AzureQueueWatcher
## What is this?
.NET Console application to watch an Azure queue, and give an ETA on when it might be empty.

## Why do i need this?
TLDR; to tell your boss when "that script" will be finished.

If you have a queue with lots of messages that you need to process, and something processing them (Azure Function, WebJob, etc), it'd be nice to know when the queue might be empty. 

This "ETA" might change depending on the instances running, so you can use this app to scale out if you need it to finish earlier.

## Screenshot:
![Imgur](https://i.imgur.com/ZWInkcQ.png)

## How to run
# Docker (public DockerHub image coming soon!)
```
docker build . -t azure-queue-watcher
docker run -e "AZURE_CONNECTION_STRING=<your-connection-string>" -e "AZURE_QUEUE_NAME=<your-queue-name>" azure-queue-watcher
```

# Windows
1. Edit the `appSettings.json` file with the correct variables.
2. Run the app!