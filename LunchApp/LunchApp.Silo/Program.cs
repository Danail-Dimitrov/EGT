using Microsoft.Extensions.Hosting;
using Orleans.Configuration;

await Host.CreateDefaultBuilder(args)
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseAzureStorageClustering(options =>
        {
            // Use the Azure Storage emulator for local development
            // The construcotr parameters are what is telling the client to use the local azurite
            options.TableServiceClient = new Azure.Data.Tables.TableServiceClient("UseDevelopmentStorage=true");
        });

        siloBuilder.Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "dev"; // Our cluster
            options.ServiceId = "LunchApp"; // Our service
        });

        siloBuilder.AddAzureTableGrainStorage("lunchapp", options =>
        {
            options.TableServiceClient = new Azure.Data.Tables.TableServiceClient("UseDevelopmentStorage=true");
        });
    })
    .RunConsoleAsync();