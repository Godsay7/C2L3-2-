using System;
using System.Text;
using PL;
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Menu menu = new Menu();
        menu.MainMenu();
    }
}