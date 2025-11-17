using System;

class Example
{
   public static void Main()
   {
      // Get an array with the values of ConsoleColor enumeration members.
      ConsoleColor[] colors = (ConsoleColor[]) ConsoleColor.GetValues(typeof(ConsoleColor));
      // Save the current background and foreground colors.
      ConsoleColor currentBackground = Console.BackgroundColor;

      // Display all foreground colors except the one that matches the background.
      Console.WriteLine("All the foreground colors except {0}, the background color:",
                        currentBackground);
      foreach (var color in colors) {
         if (color == currentBackground) continue;

         Console.ForegroundColor = color;
         Console.WriteLine("   The foreground color is {0}.", color);
      }
      Console.WriteLine();
      Console.ResetColor();
   }
}
//The example displays output like the following:
//    All the foreground colors except DarkCyan, the background color:
//       The foreground color is Black.
//       The foreground color is DarkBlue.
//       The foreground color is DarkGreen.
//       The foreground color is DarkRed.
//       The foreground color is DarkMagenta.
//       The foreground color is DarkYellow.
//       The foreground color is Gray.
//       The foreground color is DarkGray.
//       The foreground color is Blue.
//       The foreground color is Green.
//       The foreground color is Cyan.
//       The foreground color is Red.
//       The foreground color is Magenta.
//       The foreground color is Yellow.
//       The foreground color is White.