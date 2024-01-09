using Microsoft.EntityFrameworkCore;

namespace KeyFortress.models;

public class DB : DbContext
{
  public DbSet<User> users { get; set; }
  public DbSet<Password> passwords { get; set; }
  public string DbPath { get; set; }
  public DB()
  {
    var currentDirectory = Directory.GetParent("models");

    // Create the 'db' folder if it doesn't exist
    var dbFolder = Path.Combine(currentDirectory!.ToString(), "db");
    if (!Directory.Exists(dbFolder))
    {
      Directory.CreateDirectory(dbFolder);
    }

    DbPath = Path.Join(dbFolder, "KeyFortress.db");
  }

  protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite($"Data source={DbPath}");
}
