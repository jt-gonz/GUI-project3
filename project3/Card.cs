using System;

namespace UnoGame
{
    // UNO Card class
    public class Card
    {
        public CardColor Color { get; set; }
        public CardValue Value { get; set; }

        public Card(CardColor color, CardValue value)
        {
            Color = color;
            Value = value;
        }

        public bool CanPlayOn(Card topCard, CardColor? currentWildColor)
        {
            // Wild cards can be played on anything
            if (Value == CardValue.Wild || Value == CardValue.WildDrawFour)
                return true;

            // If top card is wild, use the declared color
            if (topCard.Color == CardColor.Wild && currentWildColor.HasValue)
                return Color == currentWildColor.Value;

            // Match color or value
            return Color == topCard.Color || Value == topCard.Value;
        }

        private ConsoleColor GetConsoleColor()
        {
            return Color switch
            {
                CardColor.Red => ConsoleColor.Red,
                CardColor.Yellow => ConsoleColor.Yellow,
                CardColor.Green => ConsoleColor.Green,
                CardColor.Blue => ConsoleColor.Cyan,
                CardColor.Wild => ConsoleColor.Magenta,
                _ => ConsoleColor.White
            };
        }

        private string GetValueSymbol()
        {
            return Value switch
            {
                CardValue.Zero => "0",
                CardValue.One => "1",
                CardValue.Two => "2",
                CardValue.Three => "3",
                CardValue.Four => "4",
                CardValue.Five => "5",
                CardValue.Six => "6",
                CardValue.Seven => "7",
                CardValue.Eight => "8",
                CardValue.Nine => "9",
                CardValue.Skip => "⊘",
                CardValue.Reverse => "⇄",
                CardValue.DrawTwo => "+2",
                CardValue.Wild => "W",
                CardValue.WildDrawFour => "+4",
                _ => "?"
            };
        }

        public void DisplayCard(bool showNumber = false, int number = 0, bool highlight = false)
        {
            ConsoleColor bgColor = GetConsoleColor();
            ConsoleColor fgColor = ConsoleColor.White;
            
            if (Color == CardColor.Yellow)
                fgColor = ConsoleColor.Black;

            string symbol = GetValueSymbol();
            string numStr = showNumber ? $"{number}" : " ";

            // Card design using box drawing characters
            Console.ForegroundColor = bgColor;
            Console.Write("╔═════╗");
            Console.ResetColor();
            
            if (highlight)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" ✓");
                Console.ResetColor();
            }
            
            if (showNumber)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($" [{numStr}]");
                Console.ResetColor();
            }
            
            Console.WriteLine();

            // Top section
            Console.ForegroundColor = bgColor;
            Console.Write("║");
            Console.BackgroundColor = bgColor;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($" {symbol,-3} ");
            Console.ResetColor();
            Console.ForegroundColor = bgColor;
            Console.WriteLine("║");

            // Middle section
            Console.Write("║");
            Console.BackgroundColor = bgColor;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("     ");
            Console.ResetColor();
            Console.ForegroundColor = bgColor;
            Console.WriteLine("║");

            // Bottom section
            Console.Write("║");
            Console.BackgroundColor = bgColor;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($" {symbol,3} ");
            Console.ResetColor();
            Console.ForegroundColor = bgColor;
            Console.WriteLine("║");

            Console.Write("╚═════╝");
            Console.ResetColor();
            Console.WriteLine();
        }

        public void DisplayInline()
        {
            ConsoleColor bgColor = GetConsoleColor();
            ConsoleColor fgColor = ConsoleColor.White;
            
            if (Color == CardColor.Yellow)
                fgColor = ConsoleColor.Black;

            string symbol = GetValueSymbol();

            Console.ForegroundColor = bgColor;
            Console.Write("╔═════╗ ");
            Console.ResetColor();
        }

        public void DisplayInlineLine(int lineNum)
        {
            ConsoleColor bgColor = GetConsoleColor();
            ConsoleColor fgColor = ConsoleColor.White;
            
            if (Color == CardColor.Yellow)
                fgColor = ConsoleColor.Black;

            string symbol = GetValueSymbol();

            if (lineNum == 0)
            {
                Console.ForegroundColor = bgColor;
                Console.Write("╔═════╗ ");
                Console.ResetColor();
            }
            else if (lineNum == 1)
            {
                Console.ForegroundColor = bgColor;
                Console.Write("║");
                Console.BackgroundColor = bgColor;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write($" {symbol,-3} ");
                Console.ResetColor();
                Console.ForegroundColor = bgColor;
                Console.Write("║ ");
                Console.ResetColor();
            }
            else if (lineNum == 2)
            {
                Console.ForegroundColor = bgColor;
                Console.Write("║");
                Console.BackgroundColor = bgColor;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("     ");
                Console.ResetColor();
                Console.ForegroundColor = bgColor;
                Console.Write("║ ");
                Console.ResetColor();
            }
            else if (lineNum == 3)
            {
                Console.ForegroundColor = bgColor;
                Console.Write("║");
                Console.BackgroundColor = bgColor;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write($" {symbol,3} ");
                Console.ResetColor();
                Console.ForegroundColor = bgColor;
                Console.Write("║ ");
                Console.ResetColor();
            }
            else if (lineNum == 4)
            {
                Console.ForegroundColor = bgColor;
                Console.Write("╚═════╝ ");
                Console.ResetColor();
            }
        }

        public override string ToString()
        {
            return $"{Color} {Value}";
        }
    }
}

