using KeyFortress.models;
using KeyFortress.utils;

namespace KeyFortress.repositories;

public class UserRepository
{
  private readonly DB dB;

  public UserRepository()
  {
    dB = SharedState.dB;
  }

  public (bool success, string message) Create(string username, string password = "")
  {
    try
    {
      var user = dB.users.Where(u => u.Username == username).FirstOrDefault();

      if (user != null) return (false, "Not Logged In!");

      if (password.Length == 0)
      {
        password = Utils.GenerateStrongPassword();
      }

      string encryptionKey = Utils.GenerateStrongPassword();

      CipherGenius encryptor = new CipherGenius(encryptionKey);
      string encrypted = encryptor.Encrypt(password);

      dB.Add(new User { Username = username, Password = encrypted, EncryptionKey = encryptionKey });
      dB.SaveChanges();

      Console.ForegroundColor = ConsoleColor.Green;
      return (true, $"\nWelcome {username}!");

    }
    catch (Exception ex)
    {
      Console.Error.WriteLine(ex);
      return (false, "Internal Error");
    }
  }
}