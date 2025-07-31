using Microsoft.EntityFrameworkCore;
using urlz;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=urls.db"));

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();
app.Urls.Add("http://*:3003");
app.UseCors("AllowAllOrigins");

// Ensure the database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.MapPost("/shorten", async (UrlDto request, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(request.Url))
    {
        return Results.BadRequest($"{nameof(UrlDto.Url)} is required");
    }

    var shortUrl = UrlShortener.Shorten(request.Url);
    var match = await db.Urls.FirstOrDefaultAsync(it => string.Equals(it.Shortened, shortUrl));
    if (match != null) return Results.Ok(new { shortUrl });
    
    await db.Urls.AddAsync(new()
    { 
      Original = request.Url, 
      Shortened = shortUrl, 
      Created = DateTime.UtcNow,
    });
    await db.SaveChangesAsync();
    return Results.Ok(new { shortUrl });
});

app.MapGet("/{shortUrl}", async (string shortUrl, AppDbContext db) =>
{
    var url = await db.Urls.FirstOrDefaultAsync(u => string.Equals(u.Shortened, shortUrl));
    return url == null ? Results.NotFound("Short URL not found.") : Results.Redirect(url.Original);
});

app.Run();