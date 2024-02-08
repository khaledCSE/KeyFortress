using Figgle;

namespace KeyFortress.ui;

public class UI
{

  public UI()
  {
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
    _ = new Auth();
    _ = new Menu();
  }
}
