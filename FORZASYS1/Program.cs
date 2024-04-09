using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure ElasticClient as a singleton to make sure that it is instantiated only once.
builder.Services.AddSingleton<IElasticClient>(new ElasticClient(new ConnectionSettings(
        new Uri("https://e5a011ed8a0046b7b29019b1ff352e35.us-central1.gcp.cloud.es.io:443"))
    .DefaultIndex("aisearchindex")
    .ApiKeyAuthentication("gLniwo4BZdwrVZPeVsls", "jCnxigoDSzGgdZjPXkzHfQ"))); 

// Register ElasticsearchService
builder.Services.AddScoped<ElasticsearchService>();

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