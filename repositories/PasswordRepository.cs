using KeyFortress;
using KeyFortress.models;
using KeyFortress.utils;
using Microsoft.EntityFrameworkCore;

public class PasswordRepository
{
  public (bool success, string message) Create(
    string Type,
    string UsernameOrEmail,
    string PlainPassword,
    string Name,
    string? Developer,
    string? URL
  )
  {
    try
    {
      if (PlainPassword.Length <= 0)
      {
        PlainPassword = Utils.GenerateStrongPassword();
      }

      string encryptionKey = SharedState.loggedInUser!.EncryptionKey;

      CipherGenius cipherGenius = new CipherGenius(encryptionKey);

      var encrypted = cipherGenius.Encrypt(PlainPassword);

      var newPassword = new Password
      {
        Type = Type.ToLower(),
        UsernameOrEmail = UsernameOrEmail,
        EncryptedPassword = encrypted,
        UserID = SharedState.loggedInUser!.UserID,
        User = SharedState.loggedInUser,
        Name = Utils.CapitalizeEachWord(Name),
        Developer = Utils.CapitalizeEachWord(Developer ?? ""),
        URL = Utils.CapitalizeEachWord(URL ?? ""),
      };

      SharedState.dB.Add(newPassword);
      SharedState.dB.SaveChanges();

      return (true, "Password Created!");
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine(ex);
      return (false, "error");
    }
  }

  public (bool success, IQueryable<IGrouping<string, Password>>? passwords, string message) ListAll()
  {
    try
    {
      var passwords = SharedState.dB.passwords
                      .Where(u => u.UserID == SharedState.loggedInUser!.UserID)
                      .GroupBy(u => u.Type);
      return (true, passwords, "Success");
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine(ex);
      return (false, null, "error");
    }
  }

  public (bool success, IQueryable<IGrouping<string, Password>>? passwords, string message) GetGroupedByType(string searchTerm = "")
  {
    try
    {
      var passwords = SharedState.dB.passwords
      .Where(p =>
            (EF.Functions.Like(p.Type.ToLower(), $"%{searchTerm}%") ||
            EF.Functions.Like(p.Name.ToLower(), $"%{searchTerm}%") ||
            ((p.URL != null) && EF.Functions.Like(p.URL.ToLower(), $"%{searchTerm}%")) ||
            (p.Developer != null && EF.Functions.Like(p.Developer.ToLower(), $"%{searchTerm}%"))) &&
            p.UserID == SharedState.loggedInUser!.UserID
      )
      .GroupBy(p => p.Type);

      return (true, passwords, "success");
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine(ex);
      return (false, null, "error");
    }
  }

  public (bool success, string message) DeleteById(int passwordID)
  {
    try
    {
      Password? passwordFound = SharedState.dB.passwords
                            .Where(p =>
                                    p.PasswordID == passwordID &&
                                    p.UserID == SharedState.loggedInUser!.UserID
                                  ).FirstOrDefault();
      if (passwordFound != null)
      {
        SharedState.dB.Remove(passwordFound);
        SharedState.dB.SaveChanges();

        return (true, "Deleted Successfully!");
      }
      else
      {
        return (false, "Password not found!");
      }
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine(ex);
      return (false, "error");
    }
  }
}