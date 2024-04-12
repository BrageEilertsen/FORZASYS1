using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Options; // Make sure to include this for IOptions

var builder = WebApplication.CreateBuilder(args);

// Bind Elastic configuration section to ElasticConfig class
builder.Services.Configure<ElasticConfig>(builder.Configuration.GetSection("Elastic"));

// This is where you're adding services to the DI container
builder.Services.AddControllersWithViews();

// Configure HttpClient for ElasticsearchService
builder.Services.AddHttpClient<ElasticsearchService>((serviceProvider, client) =>
{
    var elasticConfig = serviceProvider.GetService<IOptions<ElasticConfig>>().Value; // Use IOptions to access configuration

    if (string.IsNullOrWhiteSpace(elasticConfig.ApiKey))
    {
        throw new InvalidOperationException("Elasticsearch API key is not configured properly.");
    }

    client.BaseAddress = new Uri(elasticConfig.Uri);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add("Authorization", $"ApiKey {elasticConfig.ApiKey}");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();