using System.ComponentModel.DataAnnotations;

namespace urlz;

public class UrlEntry
{
  public int Id { get; set; }
  [MaxLength(4096)]
  public required string Original { get; set; }
  [MaxLength(128)]
  public required string Shortened { get; set; }
  public required DateTime Created { get; set; }
}