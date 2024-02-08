using KeyFortress.models;

namespace KeyFortress;

public static class SharedState
{
  public static DB dB = new DB();
  public static User? loggedInUser { get; set; }
}