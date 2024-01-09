using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeyFortress.models;

public class Password
{
  [Key]
  public int PasswordID { get; set; }

  public int UserID { get; set; }  // Foreign Key

  [ForeignKey("UserID")]
  public required User User { get; set; }  // Navigation Property

  public required string Type { get; set; }  // website, application, game

  public required string Name { get; set; }

  public string? URL { get; set; }  // Nullable for applications and games

  public required string UsernameOrEmail { get; set; }  // Email or Username

  public required string EncryptedPassword { get; set; }  // Encrypted password

  public string? Developer { get; set; }  // Nullable for websites and applications

  public DateTime DateCreated { get; set; } = DateTime.UtcNow;

  public DateTime DateLastUpdated { get; set; } = DateTime.UtcNow;
}
