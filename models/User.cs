using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KeyFortress.models;

[Index(nameof(Username), IsUnique = true)]
public class User
{
  [Key]
  public int UserID { get; set; }
  public string Username { get; set; } = "";
  public string Password { get; set; } = "";
  public string EncryptionKey { get; set; } = "";
}
