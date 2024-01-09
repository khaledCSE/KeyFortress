using Figgle;
using KeyFortress.models;

namespace KeyFortress.ui;

public class UI
{
  public DB dB { get; set; }
  public static User? loggedInUser { get; set; }

  public UI(DB database)
  {
    dB = database;
    GreetUser();
    UiFlow();
  }

  public static void GreetUser()
  {
    string text = "KeyFortress";

    Console.ForegroundColor = ConsoleColor.Cyan;

    string logo = FiggleFonts.Standard.Render(text);

    Console.WriteLine(logo);
    Console.ResetColor();
  }

  public void UiFlow()
  {
    _ = new Auth(dB);
    _ = new Menu(dB);
  }
}
