using System;
using System.Collections.Generic;

namespace UnoGame
{
    // Player class
    public class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; set; }
        public bool IsComputer { get; set; }
        public bool SaidUno { get; set; }

        public Player(string name, bool isComputer = false)
        {
            Name = name;
            Hand = new List<Card>();
            IsComputer = isComputer;
            SaidUno = false;
        }

        public void DrawCard(Card card)
        {
            Hand.Add(card);
        }

        public void DisplayHand(List<int>? playableIndices = null)
        {
            if (Hand.Count == 0)
            {
                Console.WriteLine("No cards in hand.");
                return;
            }

            // Display all cards in a single horizontal line
            // Draw 5 lines for all cards
            for (int line = 0; line < 5; line++)
            {
                for (int i = 0; i < Hand.Count; i++)
                {
                    Hand[i].DisplayInlineLine(line);
                }
                Console.WriteLine();
            }

            // Display card numbers and playability
            for (int i = 0; i < Hand.Count; i++)
            {
                bool isPlayable = playableIndices?.Contains(i) ?? false;
                
                if (isPlayable)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"  [{i + 1}]✓  ");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write($"  [{i + 1}]   ");
                    Console.ResetColor();
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        public Card? PlayCard(int index)
        {
            if (index < 0 || index >= Hand.Count)
                return null;

            Card card = Hand[index];
            Hand.RemoveAt(index);
            return card;
        }
    }
}

