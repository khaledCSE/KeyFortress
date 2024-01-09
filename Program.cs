using KeyFortress.models;
using KeyFortress.ui;

void Main()
{
  Console.OutputEncoding = System.Text.Encoding.UTF8;

  var db = new DB();
  var ui = new UI(db);
}

Main();