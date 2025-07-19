using Azure.Data.Tables;
using LunchApp.Grains.Abstractions;
using Orleans;
using Orleans.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseOrleansClient((context, client) =>
{
    client.UseAzureStorageClustering(options =>
    {
        // Use the Azure Storage emulator for local development
        // The construcotr parameters are what is telling the client to use the local azurite
        options.TableServiceClient = new Azure.Data.Tables.TableServiceClient("UseDevelopmentStorage=true");    
    });

    client.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "dev"; // Our cluster
        options.ServiceId = "LunchApp"; // Our service
    });
});

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

app.MapGet("test/{id}", 
    async (
        int id,
        IClusterClient clusterClient
    ) =>
{
    var grain = clusterClient.GetGrain<ITestGrain>(1);

    var res = await grain.GetValue();

    return Results.Ok($"Grain value: {res}");
});

app.MapPost("test", async (IClusterClient clusterClient) =>
{
    var grain = clusterClient.GetGrain<ITestGrain>(1);

    await grain.Init(1);

    return Results.Ok("Grain initialized");
});

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();

//app.UseRouting();

//app.UseAuthorization();

//app.MapStaticAssets();
//app.MapRazorPages()
//   .WithStaticAssets();

app.Run();
