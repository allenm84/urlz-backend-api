using Microsoft.EntityFrameworkCore;

namespace urlz;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public required DbSet<UrlEntry> Urls { get; set; }
}