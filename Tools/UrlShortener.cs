using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace urlz;

public static class UrlShortener
{
  private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
  private static readonly int Base = Alphabet.Length;

  public static string Shorten(string url)
  {
    // generate a unique key
    var unhashedBytes = Encoding.UTF8.GetBytes(url);
    var hashed = SHA256.HashData(unhashedBytes);
    var hashValue = new BigInteger(new(hashed, 0, 16), true);
    var sb = new StringBuilder();
    while (hashValue > 0)
    {
      sb.Append(Alphabet[(int)(hashValue % Base)]);
      hashValue /= Base;
    }
    return sb.ToString();
  }
}