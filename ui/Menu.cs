using System.Text.Json;
using ConsoleTables;
using KeyFortress.models;
using KeyFortress.utils;
using Microsoft.EntityFrameworkCore;

namespace KeyFortress.ui;

public class Menu
{
  private PasswordRepository passwordRepository;
  private string[] menuItems = [
      "🔑 Add Password",
      "📃 List Passwords",
      "🔍 Search Password",
      "🛡️ Update Password",
      "🗑️ Delete Password",
      "❌ Exit"
      ];
  public Menu()
  {
    passwordRepository = new PasswordRepository();

    while (true)
    {
      string selected = DisplayMenu();

      switch (selected)
      {
        case "🔑 Add Password":
          AddPassword();
          break;
        case "📃 List Passwords":
          ListPasswords();
          break;
        case "🔍 Search Password":
          SearchPasswords();
          break;
        case "🛡️ Update Password":
          UpdatePassword();
          break;
        case "🗑️ Delete Password":
          DeletePassword();
          break;
        case "❌ Exit":
          Console.WriteLine("Time to leave the fortress like a 👑");
          Environment.Exit(0);
          break;
        default:
          break;
      }
    }
  }

  public string DisplayMenu()
  {

    Console.WriteLine("\nMenu:");
    for (int i = 0; i < menuItems.Length; i++)
    {
      Console.WriteLine($"{i + 1}: {menuItems[i]}");
    }

    Console.Write("\nEnter an option: ");
    string choice = Console.ReadLine() ?? "";
    int index = int.Parse(choice) - 1;
    return menuItems[index <= menuItems.Length ? index : 0];
  }

  public void AddPassword()
  {
    string? url = null;
    string nameOfApp = "";
    string? developer = null;
    string usernameOrEmail = "";
    string password = "";

    Console.Write("Enter the type of application (Website/Application/Game): ");
    string typeOfApp = Console.ReadLine() ?? "";


    switch (typeOfApp.ToLower())
    {
      case "website":
        {
          Console.Write("Enter the name of the website (Ex: Facebook): ");
          nameOfApp = Utils.CapitalizeEachWord(Console.ReadLine() ?? "");

          Console.Write("Enter the url of the website: ");
          url = Console.ReadLine() ?? "";

          Console.Write("Enter the email/username of the website: ");
          usernameOrEmail = Console.ReadLine() ?? "";

          Console.Write("Enter a password (Leave blank to generate a strong 💪 one): ");
          password = Utils.MaskPasswordInput();
          break;
        }
      case "application":
        {
          Console.Write("Enter the name of the application (Ex: KeyFortress): ");
          nameOfApp = Console.ReadLine() ?? "";

          Console.Write($"Enter the email/username for {nameOfApp}: ");
          usernameOrEmail = Console.ReadLine() ?? "";

          Console.Write("Enter a password (Leave blank to generate a strong 💪 one): ");
          password = Utils.MaskPasswordInput();
          break;
        }
      case "game":
        {
          Console.Write("Enter the name of the game (Ex: Sniper Elite 3, EA FC 24): ");
          nameOfApp = Console.ReadLine() ?? "";

          Console.Write($"Enter the developer name of {nameOfApp}: ");
          developer = Console.ReadLine() ?? "";

          Console.Write($"Enter the username for {nameOfApp}: ");
          usernameOrEmail = Console.ReadLine() ?? "";

          Console.Write("Enter a password (Leave blank to generate a strong 💪 one): ");
          password = Utils.MaskPasswordInput();
          break;
        }
      default:
        break;
    }

    var (success, message) = passwordRepository.Create(typeOfApp, usernameOrEmail, password, nameOfApp, developer, url);

    if (success)
    {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(message);
    }
    else
    {
      if (message != "error")
      {

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
      }
    }

    Console.ResetColor();
  }

  public void ListPasswords()
  {
    var (success, passwords, _) = passwordRepository.ListAll();

    if (success && passwords != null)
    {

      PrintPasswordTable(passwords);
    }

  }

  public void SearchPasswords()
  {
    Console.Write("\nEnter the search term: ");
    string searchTerm = (Console.ReadLine() ?? "").ToLower();

    var passwords = SharedState.dB.passwords
      .Where(p =>
            EF.Functions.Like(p.Type.ToLower(), $"%{searchTerm}%") ||
            EF.Functions.Like(p.Name.ToLower(), $"%{searchTerm}%") ||
            ((p.URL != null) && EF.Functions.Like(p.URL.ToLower(), $"%{searchTerm}%")) ||
            (p.Developer != null && EF.Functions.Like(p.Developer.ToLower(), $"%{searchTerm}%"))
      )
      .GroupBy(p => p.Type);

    PrintPasswordTable(passwords);
  }

  public void UpdatePassword()
  {
    bool isValidIdProvided = false;

    while (!isValidIdProvided)
    {
      Console.Write("\nEnter the Password ID: ");
      int passwordID = int.Parse((Console.ReadLine() ?? "").ToLower());

      // * Find the password
      Password? passwordFound = SharedState.dB.passwords
                            .Where(p =>
                                    p.PasswordID == passwordID &&
                                    p.UserID == SharedState.loggedInUser!.UserID
                                  ).FirstOrDefault();

      if (passwordFound != null)
      {
        Console.Write($"Enter the type of application (Current: {passwordFound.Type.ToUpper()}, Leave blank to keep as is): ");
        string typeOfApp = Console.ReadLine() ?? "";

        Console.Write($"Enter the name of the {typeOfApp.ToUpper()} (Current: {passwordFound.Name.ToUpper()}, Leave blank to keep as is): ");
        string nameOfApp = Console.ReadLine() ?? "";

        Console.Write("\nEnter New Password: ");
        string newPassword = Utils.MaskPasswordInput();

        if (typeOfApp.Equals("website"))
        {

          Console.Write($"Enter the url of {nameOfApp} (Current: {passwordFound.URL}, Leave blank to keep as is): ");
          string url = Console.ReadLine() ?? "";

          if (url.Length > 0) passwordFound.URL = url;

          Console.Write($"Enter the email/username for {nameOfApp} (Current: {passwordFound.UsernameOrEmail} Leave blank to keep as is): ");
          string usernameOrEmail = Console.ReadLine() ?? "";

          if (usernameOrEmail.Length > 0)
          {
            passwordFound.UsernameOrEmail = usernameOrEmail;
          }
        }
        else if (typeOfApp.Equals("application"))
        {
          Console.Write($"Enter the email/username for {nameOfApp} (Current: {passwordFound.UsernameOrEmail} Leave blank to keep as is): ");
          string usernameOrEmail = Console.ReadLine() ?? "";

          if (usernameOrEmail.Length > 0)
          {
            passwordFound.UsernameOrEmail = usernameOrEmail;
          }
        }
        else if (typeOfApp.Equals("game"))
        {
          Console.Write($"Enter the developer name of {nameOfApp}(Current: {passwordFound.Developer} Leave blank to keep as is): ");
          string developer = Console.ReadLine() ?? "";

          if (developer.Length > 0)
          {
            passwordFound.UsernameOrEmail = Utils.CapitalizeEachWord(developer);
          }

          Console.Write($"Enter the username for {nameOfApp} (Current: {passwordFound.UsernameOrEmail} Leave blank to keep as is): ");
          string usernameOrEmail = Console.ReadLine() ?? "";

          if (usernameOrEmail.Length > 0)
          {
            passwordFound.UsernameOrEmail = usernameOrEmail;
          }
        }


        if (newPassword.Length == 0)
        {
          Console.Write("Would you like to generate a strong password? (Y/N): ");
          string passwordGen = Console.ReadLine() ?? "N";
          if (passwordGen.ToLower().Equals("y"))
          {

            newPassword = Utils.GenerateStrongPassword();
            CipherGenius cipherGenius = new CipherGenius(SharedState.loggedInUser!.EncryptionKey);
            string encrypted = cipherGenius.Encrypt(newPassword);
            passwordFound.EncryptedPassword = encrypted;
          }
        }


        if (typeOfApp.Length > 0) passwordFound.Type = typeOfApp;
        if (nameOfApp.Length > 0) passwordFound.Name = Utils.CapitalizeEachWord(nameOfApp);

        SharedState.dB.SaveChanges();
        Console.WriteLine("Updated Successfully!");
        isValidIdProvided = true;
      }
      else
      {
        Console.WriteLine("Password not found!");
      }
    }
  }

  public void DeletePassword()
  {
    bool isValidIdProvided = false;

    while (!isValidIdProvided)
    {
      Console.Write("\nEnter the Password ID: ");
      int passwordID = int.Parse((Console.ReadLine() ?? "").ToLower());

      // * Find the password
      Password? passwordFound = SharedState.dB.passwords
                            .Where(p =>
                                    p.PasswordID == passwordID &&
                                    p.UserID == SharedState.loggedInUser!.UserID
                                  ).FirstOrDefault();

      if (passwordFound != null)
      {
        SharedState.dB.Remove(passwordFound);
        SharedState.dB.SaveChanges();
        Console.WriteLine("Deleted Successfully!");

        isValidIdProvided = true;
      }
      else
      {
        Console.WriteLine("Password not found!");
      }
    }
  }

  public static void PrintPasswordTable(IQueryable<IGrouping<string, Password>> passwords)
  {
    Console.Write("Would you like to see the passwords in plain text? (Y/N): ");
    string choice = Console.ReadLine() ?? "";



    foreach (var group in passwords)
    {
      // Access the group's key (Type) and elements

      CipherGenius cipherGenius = new CipherGenius(SharedState.loggedInUser!.EncryptionKey);

      switch (group.Key.ToLower())
      {
        case "website":
          Console.WriteLine("Passwords for: {0}S", group.Key.ToUpper());
          ConsoleTable websiteTable = new ConsoleTable("ID", "Name", "URL", "Username / Email", "Password", "Created", "Last Updated");
          foreach (Password password in group)
          {
            string plainText = cipherGenius.Decrypt(password.EncryptedPassword);
            websiteTable.AddRow(
              password.PasswordID,
              password.Name,
              password.URL,
              password.UsernameOrEmail,
              choice.ToLower().Equals("y") ? plainText : new string('*', plainText.Length),
              Utils.GetTimeAgo(password.DateCreated),
              Utils.GetTimeAgo(password.DateLastUpdated)
            );
          }
          websiteTable.Write(Format.Default);
          break;
        case "application":
          Console.WriteLine("Passwords for: {0}S", group.Key.ToUpper());
          ConsoleTable appTable = new ConsoleTable("ID", "Name", "Username", "Password", "Created", "Last Updated");
          foreach (Password password in group)
          {
            string plainText = cipherGenius.Decrypt(password.EncryptedPassword);
            appTable.AddRow(
              password.PasswordID,
              password.Name,
              password.UsernameOrEmail,
              choice.ToLower().Equals("y") ? plainText : new string('*', plainText.Length),
              Utils.GetTimeAgo(password.DateCreated),
              Utils.GetTimeAgo(password.DateLastUpdated)
            );
          }
          appTable.Write(Format.Default);
          break;
        case "game":
          Console.WriteLine("Passwords for: {0}S", group.Key.ToUpper());
          ConsoleTable gameTable = new ConsoleTable("ID", "Name", "Developer", "Username", "Password", "Created", "Last Updated");
          foreach (Password password in group)
          {
            string plainText = cipherGenius.Decrypt(password.EncryptedPassword);
            gameTable.AddRow(
              password.PasswordID,
              password.Name,
              password.Developer,
              password.UsernameOrEmail,
              choice.ToLower().Equals("y") ? plainText : new string('*', plainText.Length),
              Utils.GetTimeAgo(password.DateCreated),
              Utils.GetTimeAgo(password.DateLastUpdated)
            );
          }
          gameTable.Write(Format.Default);
          break;
        default:
          break;
      }
    }
  }


}
