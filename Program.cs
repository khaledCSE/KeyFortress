using KeyFortress.models;
using KeyFortress.ui;

void Main()
{
  Console.OutputEncoding = System.Text.Encoding.UTF8;

  _ = new DB();
  _ = new UI();
}

Main();