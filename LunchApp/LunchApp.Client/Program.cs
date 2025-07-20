using LunchApp.Client.Services;
using LunchApp.Client.Services.Interfaces;
using LunchApp.Grains.Abstractions;
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

// DI
builder.Services.AddScoped<IVoteService, VoteService>();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
