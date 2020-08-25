using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashingFunction
{
    class HelperClass
    {
        public static string InputPassword()
        {
            StringBuilder password = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.Write("\n");
                    break;
                }

                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.Remove(password.Length - 1, 1);

                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);

                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                        Console.ForegroundColor = ConsoleColor.White;

                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    }
                }

                else
                {
                    password.Append(key.KeyChar);
                    Console.Write('*');
                }
            }
            return password.ToString();
        }
    }
}
