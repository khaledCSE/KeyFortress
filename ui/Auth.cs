using KeyFortress.enums;
using KeyFortress.models;
using KeyFortress.utils;
namespace KeyFortress.ui;

public class Auth
{
  private bool isAuthenticated = false;
  public DB dB { get; set; }

  public Auth(DB database)
  {
    dB = database;
    AuthFlow();
  }

  public LoginResponse Login()
  {
    Console.WriteLine("\nLogin Here");
    Console.Write("Enter username: ");
    string username = Console.ReadLine() ?? "";

    Console.Write("Enter password: ");
    string password = Console.ReadLine() ?? "";

    var user = dB.users.Where(u => u.Username == username).FirstOrDefault();

    if (user == null) return LoginResponse.newUser;

    CipherGenius encryptor = new CipherGenius(user.EncryptionKey);

    string encrypted = encryptor.Encrypt(password);

    bool passwordsMatch = user.Password.Equals(encrypted);

    if (!passwordsMatch) return LoginResponse.wrongPassword;

    UI.loggedInUser = user;
    return LoginResponse.ok;
  }

  public SignupResponse Signup()
  {
    Console.WriteLine("\nSignup Here");
    Console.Write("Enter username: ");
    string username = Console.ReadLine() ?? "";

    Console.Write("Enter password (Leave blank to generate a strong password): ");
    string password = Console.ReadLine() ?? "";

    var user = dB.users.Where(u => u.Username == username).FirstOrDefault();

    if (user != null) return SignupResponse.userExists;

    if (password.Length == 0)
    {
      password = Utils.GenerateStrongPassword();
    }

    string encryptionKey = Utils.GenerateStrongPassword();

    CipherGenius encryptor = new CipherGenius(encryptionKey);
    string encrypted = encryptor.Encrypt(password);

    try
    {
      dB.Add(new User { Username = username, Password = encrypted, EncryptionKey = encryptionKey });
      dB.SaveChanges();

      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine($"Welcome {username}!");

      return SignupResponse.ok;
    }
    catch (Exception ex)
    {
      Console.ForegroundColor = ConsoleColor.Red;
      if (ex.InnerException != null)
      {
        var IsUniqueError = ex?.InnerException.Message.Contains("'UNIQUE constraint failed: users.username'");
        if (IsUniqueError == true)
        {
          Console.WriteLine("Duplicate Username is not allowed.");
        }
      }
      else
      {
        Console.WriteLine($"An error occurred: {ex.Message}");
      }

      return SignupResponse.error;
    }
    finally
    {
      Console.ResetColor();
    }
  }

  public void AuthFlow()
  {
    string authType = "";
    while (authType.Length <= 0 || (authType != "1" && authType != "2"))
    {
      Console.WriteLine("Please Authenticate");
      Console.WriteLine("1. Login (For Existing User)");
      Console.WriteLine("2. Signup (For New User)");

      Console.Write("Enter an option: ");
      authType = Console.ReadLine() ?? "";
    }

    while (!isAuthenticated)
    {
      switch (authType)
      {
        case "1":
          var loginStatus = Login();
          if (loginStatus == LoginResponse.ok)
          {
            isAuthenticated = true;
            break;
          }
          else if (loginStatus == LoginResponse.newUser)
          {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Seems like you don't have an account, consider signing up");
            Console.ResetColor();
            authType = "2";
            break;
          }
          break;
        case "2":
          var signupStatus = Signup();
          if (signupStatus == SignupResponse.ok)
          {
            isAuthenticated = true;
          }
          else if (signupStatus == SignupResponse.userExists)
          {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Seems like you already have an account, consider logging in");
            Console.ResetColor();

            authType = "1";
          }
          break;
        default:
          break;
      }
    }
  }
}
