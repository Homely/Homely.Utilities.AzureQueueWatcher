# Create the build environment image
FROM microsoft/dotnet:2.1-sdk as build-env
WORKDIR /app
 
# Copy the project file and restore the dependencies
COPY *.csproj ./
RUN dotnet restore
 
# Copy the remaining source files and build the application
COPY . ./
RUN dotnet publish -c Release -o out
 
# Build the runtime image
FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Homely.Utilities.AzureQueueWatcher.dll"]