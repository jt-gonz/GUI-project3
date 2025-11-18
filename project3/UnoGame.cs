using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace UnoGame
{
    // UNO Game class
    public class UnoGame
    {
        private List<Card> deck;
        private List<Card> discardPile;
        private List<Player> players;
        private int currentPlayerIndex;
        private bool isClockwise;
        private CardColor? currentWildColor;
        private Random random;
        private int numberOfPlayers;

        public UnoGame()
        {
            deck = new List<Card>();
            discardPile = new List<Card>();
            players = new List<Player>();
            currentPlayerIndex = 0;
            isClockwise = true;
            currentWildColor = null;
            random = new Random();
            numberOfPlayers = 4; // Default
        }

        private void InitializeDeck()
        {
            deck.Clear();

            // Add numbered cards (0-9) for each color
            foreach (CardColor color in new[] { CardColor.Red, CardColor.Yellow, CardColor.Green, CardColor.Blue })
            {
                // One 0 card per color
                deck.Add(new Card(color, CardValue.Zero));

                // Two of each 1-9 card per color
                for (int i = 1; i <= 9; i++)
                {
                    deck.Add(new Card(color, (CardValue)i));
                    deck.Add(new Card(color, (CardValue)i));
                }

                // Two Skip, Reverse, and Draw Two per color
                deck.Add(new Card(color, CardValue.Skip));
                deck.Add(new Card(color, CardValue.Skip));
                deck.Add(new Card(color, CardValue.Reverse));
                deck.Add(new Card(color, CardValue.Reverse));
                deck.Add(new Card(color, CardValue.DrawTwo));
                deck.Add(new Card(color, CardValue.DrawTwo));
            }

            // Add Wild cards
            for (int i = 0; i < 4; i++)
            {
                deck.Add(new Card(CardColor.Wild, CardValue.Wild));
                deck.Add(new Card(CardColor.Wild, CardValue.WildDrawFour));
            }

            ShuffleDeck();
        }

        private void ShuffleDeck()
        {
            for (int i = deck.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (deck[i], deck[j]) = (deck[j], deck[i]);
            }
        }

        private Card DrawFromDeck()
        {
            if (deck.Count == 0)
            {
                // Reshuffle discard pile into deck (keep top card)
                if (discardPile.Count > 1)
                {
                    Card topCard = discardPile[discardPile.Count - 1];
                    discardPile.RemoveAt(discardPile.Count - 1);
                    deck.AddRange(discardPile);
                    discardPile.Clear();
                    discardPile.Add(topCard);
                    ShuffleDeck();
                    Console.WriteLine("\n♻ Reshuffling discard pile into deck...");
                }
            }

            if (deck.Count > 0)
            {
                Card card = deck[0];
                deck.RemoveAt(0);
                return card;
            }

            // Fallback: create a new random card
            return new Card(CardColor.Red, CardValue.One);
        }

        private void InitializePlayers()
        {
            players.Clear();
            players.Add(new Player("You", false));

            for (int i = 1; i < numberOfPlayers; i++)
            {
                players.Add(new Player($"Computer {i}", true));
            }

            // Deal 8 cards to each player
            foreach (var player in players)
            {
                for (int i = 0; i < 8; i++)
                {
                    player.DrawCard(DrawFromDeck());
                }
            }
        }

        private Card GetTopCard()
        {
            return discardPile[discardPile.Count - 1];
        }

        private void DisplayGameState()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                            UNO GAME                                    ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════╝");

            // Calculate next player index
            int nextPlayerIndex;
            if (isClockwise)
            {
                nextPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            }
            else
            {
                nextPlayerIndex = (currentPlayerIndex - 1 + players.Count) % players.Count;
            }

            // Show all players' card counts in horizontal colored squares
            Console.WriteLine();
            
            // Top border
            for (int i = 0; i < players.Count; i++)
            {
                Console.Write("┌──────────────┐ ");
            }
            Console.WriteLine();
            
            // Player names with arrow
            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i];
                string arrow = i == currentPlayerIndex ? "> " : "  ";
                string nameWithArrow = arrow + player.Name;
                string nameDisplay = nameWithArrow.Length > 12 ? nameWithArrow.Substring(0, 12) : nameWithArrow.PadRight(12);
                
                if (i == currentPlayerIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (i == nextPlayerIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                
                Console.Write($"│ {nameDisplay} │ ");
                Console.ResetColor();
            }
            Console.WriteLine();
            
            // Card counts
            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i];
                string cardCount = $"  {player.Hand.Count} cards".PadRight(12);
                
                if (i == currentPlayerIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (i == nextPlayerIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                
                Console.Write($"│ {cardCount} │ ");
                Console.ResetColor();
            }
            Console.WriteLine();
            
            // Bottom border
            for (int i = 0; i < players.Count; i++)
            {
                Console.Write("└──────────────┘ ");
            }
            Console.WriteLine();
            Console.WriteLine();

            // Display top card
            Console.WriteLine("Current Card on Table:");
            GetTopCard().DisplayCard();
            
            if (currentWildColor.HasValue)
            {
                Console.Write("Active Color: ");
                Console.ForegroundColor = currentWildColor.Value switch
                {
                    CardColor.Red => ConsoleColor.Red,
                    CardColor.Yellow => ConsoleColor.Yellow,
                    CardColor.Green => ConsoleColor.Green,
                    CardColor.Blue => ConsoleColor.Cyan,
                    _ => ConsoleColor.White
                };
                Console.WriteLine($"█ {currentWildColor.Value} █");
                Console.ResetColor();
            }
        }

        private CardColor ChooseWildColor(Player player)
        {
            if (player.IsComputer)
            {
                // Computer chooses the color it has most of
                var colorCounts = new Dictionary<CardColor, int>();
                foreach (CardColor color in new[] { CardColor.Red, CardColor.Yellow, CardColor.Green, CardColor.Blue })
                {
                    colorCounts[color] = player.Hand.Count(c => c.Color == color);
                }
                return colorCounts.OrderByDescending(kv => kv.Value).First().Key;
            }
            else
            {
                Console.WriteLine("\n┌─────────────────────────────┐");
                Console.WriteLine("│   Choose a Wild Color:      │");
                Console.WriteLine("├─────────────────────────────┤");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("│ 1. ██ Red                   │");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("│ 2. ██ Yellow                │");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("│ 3. ██ Green                 │");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("│ 4. ██ Blue                  │");
                Console.ResetColor();
                Console.WriteLine("└─────────────────────────────┘");
                
                while (true)
                {
                    Console.Write("\nEnter choice (1-4): ");
                    string? input = Console.ReadLine();
                    if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 4)
                    {
                        return choice switch
                        {
                            1 => CardColor.Red,
                            2 => CardColor.Yellow,
                            3 => CardColor.Green,
                            4 => CardColor.Blue,
                            _ => CardColor.Red
                        };
                    }
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
        }

        private void ExecuteCardEffect(Card card, Player player)
        {
            currentWildColor = null;

            switch (card.Value)
            {
                case CardValue.Skip:
                    MoveToNextPlayer();
                    Console.WriteLine($"\n⊘ SKIP! {players[currentPlayerIndex].Name} loses their turn.");
                    break;

                case CardValue.Reverse:
                    isClockwise = !isClockwise;
                    Console.WriteLine($"\n⇄ REVERSE! Direction changed.");
                    if (players.Count == 2)
                        MoveToNextPlayer(); // In 2-player, reverse acts like skip
                    break;

                case CardValue.DrawTwo:
                    MoveToNextPlayer();
                    Player nextPlayer = players[currentPlayerIndex];
                    Console.WriteLine($"\n+2 {nextPlayer.Name} draws 2 cards and loses their turn!");
                    nextPlayer.DrawCard(DrawFromDeck());
                    nextPlayer.DrawCard(DrawFromDeck());
                    MoveToNextPlayer();
                    break;

                case CardValue.Wild:
                    currentWildColor = ChooseWildColor(player);
                    Console.WriteLine($"\n🎨 {player.Name} chose {currentWildColor}");
                    break;

                case CardValue.WildDrawFour:
                    currentWildColor = ChooseWildColor(player);
                    Console.WriteLine($"\n🎨 {player.Name} chose {currentWildColor}");
                    MoveToNextPlayer();
                    Player drawFourPlayer = players[currentPlayerIndex];
                    Console.WriteLine($"+4 {drawFourPlayer.Name} draws 4 cards and loses their turn!");
                    for (int i = 0; i < 4; i++)
                    {
                        drawFourPlayer.DrawCard(DrawFromDeck());
                    }
                    MoveToNextPlayer();
                    break;
            }
        }

        private void MoveToNextPlayer()
        {
            if (isClockwise)
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            }
            else
            {
                currentPlayerIndex = (currentPlayerIndex - 1 + players.Count) % players.Count;
            }
        }

        private bool PlayerTurn(Player player)
        {
            Console.WriteLine($"\n╔═══════════════════════════════════════════════════════════════════════╗");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"║              {player.Name}'s TURN                                          ║");
            Console.ResetColor();
            Console.WriteLine($"╚═══════════════════════════════════════════════════════════════════════╝");

            if (!player.IsComputer)
            {
                // Find playable cards
                List<int> playableIndices = new List<int>();
                for (int i = 0; i < player.Hand.Count; i++)
                {
                    if (player.Hand[i].CanPlayOn(GetTopCard(), currentWildColor))
                    {
                        playableIndices.Add(i);
                    }
                }

                Console.WriteLine("Your Hand:");
                Console.WriteLine("─────────────────────────────────────────────────────────────────────────");
                player.DisplayHand(playableIndices);

                if (playableIndices.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("✗ No playable cards! Drawing 2 cards and skipping turn...");
                    Console.ResetColor();
                    player.DrawCard(DrawFromDeck());
                    player.DrawCard(DrawFromDeck());
                    Console.WriteLine($"You now have {player.Hand.Count} cards.");
                    Thread.Sleep(1500);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✓ Playable cards marked with ✓");
                    Console.ResetColor();
                    
                    while (true)
                    {
                        Console.WriteLine("\n┌─────────────────────────────────────┐");
                        Console.WriteLine("│ Enter card # to play, or 0 to draw │");
                        Console.WriteLine("└─────────────────────────────────────┘");
                        Console.Write("Your choice: ");
                        string? input = Console.ReadLine();
                        
                        if (int.TryParse(input, out int choice))
                        {
                            if (choice == 0)
                            {
                                Console.WriteLine("\nDrawing 2 cards and skipping turn...");
                                player.DrawCard(DrawFromDeck());
                                player.DrawCard(DrawFromDeck());
                                Console.WriteLine($"You now have {player.Hand.Count} cards.");
                                Thread.Sleep(1500);
                                break;
                            }
                            else if (choice >= 1 && choice <= player.Hand.Count)
                            {
                                int index = choice - 1;
                                if (player.Hand[index].CanPlayOn(GetTopCard(), currentWildColor))
                                {
                                    Card? playedCard = player.PlayCard(index);
                                    if (playedCard != null)
                                    {
                                        Console.WriteLine($"\n✓ Playing card:");
                                        playedCard.DisplayCard();
                                        discardPile.Add(playedCard);
                                        ExecuteCardEffect(playedCard, player);
                                    }
                                    break;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("✗ That card cannot be played!");
                                    Console.ResetColor();
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("✗ Invalid card number!");
                                Console.ResetColor();
                            }
                        }
                    }
                }
            }
            else
            {
                // Computer player logic
                Console.WriteLine($"{player.Name} is thinking...");
                Thread.Sleep(2000); // Thinking time

                // Find playable cards
                List<int> playableIndices = new List<int>();
                for (int i = 0; i < player.Hand.Count; i++)
                {
                    if (player.Hand[i].CanPlayOn(GetTopCard(), currentWildColor))
                    {
                        playableIndices.Add(i);
                    }
                }

                if (playableIndices.Count == 0)
                {
                    Console.WriteLine($"{player.Name} has no playable cards.");
                    Thread.Sleep(1000);
                    Console.WriteLine($"Drawing 2 cards and skipping turn...");
                    player.DrawCard(DrawFromDeck());
                    player.DrawCard(DrawFromDeck());
                    Thread.Sleep(2500);
                }
                else
                {
                    // Play a random playable card (simple AI)
                    int randomIndex = playableIndices[random.Next(playableIndices.Count)];
                    Card? playedCard = player.PlayCard(randomIndex);
                    if (playedCard != null)
                    {
                        Console.WriteLine($"{player.Name} plays:");
                        Thread.Sleep(800);
                        playedCard.DisplayCard();
                        discardPile.Add(playedCard);
                        Thread.Sleep(1500);
                        ExecuteCardEffect(playedCard, player);
                        Thread.Sleep(2000);
                    }
                }
            }

            // Check if player won
            return player.Hand.Count == 0;
        }

        private void ShowRules()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                            UNO RULES                                   ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("OBJECTIVE:");
            Console.WriteLine("  Be the first player to get rid of all your cards!");
            Console.WriteLine();
            Console.WriteLine("HOW TO PLAY:");
            Console.WriteLine("  • Match the top card by COLOR or NUMBER");
            Console.WriteLine("  • If you can't play, you must draw 2 cards and skip your turn");
            Console.WriteLine("  • You can choose to draw 2 cards even if you have playable cards");
            Console.WriteLine();
            Console.WriteLine("SPECIAL CARDS:");
            Console.WriteLine("  ⊘  SKIP      - Next player loses their turn");
            Console.WriteLine("  ⇄  REVERSE   - Reverses the direction of play");
            Console.WriteLine("  +2 DRAW TWO  - Next player draws 2 cards and loses their turn");
            Console.WriteLine("  W  WILD      - Play on any card, choose the next color");
            Console.WriteLine("  +4 WILD +4   - Choose color, next player draws 4 and loses turn");
            Console.WriteLine();
            Console.WriteLine("CARD COLORS:");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("  ██ RED   ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("██ YELLOW   ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("██ GREEN   ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("██ BLUE");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("═══════════════════════════════════════════════════════════════════════════");
            Console.Write("Press Enter to return to menu...");
            Console.ReadLine();
        }

        private void SetNumberOfPlayers()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                       SELECT NUMBER OF PLAYERS                         ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("                           ┌──────────────────┐");
            Console.WriteLine("                           │  2 Players       │");
            Console.WriteLine("                           │  3 Players       │");
            Console.WriteLine("                           │  4 Players       │");
            Console.WriteLine("                           │  5 Players       │");
            Console.WriteLine("                           │  6 Players       │");
            Console.WriteLine("                           └──────────────────┘");
            Console.WriteLine();
            Console.Write($"                   Current: {numberOfPlayers} players. Enter choice (2-6): ");
            
            string? input = Console.ReadLine();
            
            if (int.TryParse(input, out int choice) && choice >= 2 && choice <= 6)
            {
                numberOfPlayers = choice;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n                   ✓ Number of players set to {numberOfPlayers}!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n                   ✗ Invalid choice! Keeping current setting.");
                Console.ResetColor();
            }
            
            Thread.Sleep(1000);
        }

        private bool ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                                                                        ║");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("║                          WELCOME TO UNO!                               ║");
                Console.ResetColor();
                Console.WriteLine("║                                                                        ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("                           ┌──────────────────────────┐");
                Console.WriteLine("                           │      MAIN MENU           │");
                Console.WriteLine("                           ├──────────────────────────┤");
                Console.WriteLine("                           │ 1. Start Game            │");
                Console.WriteLine("                           │ 2. Rules                 │");
                Console.WriteLine($"                           │ 3. Players ({numberOfPlayers})           │");
                Console.WriteLine("                           │ 4. Quit                  │");
                Console.WriteLine("                           └──────────────────────────┘");
                Console.WriteLine();
                Console.Write("                           Enter choice (1-4): ");
                
                string? input = Console.ReadLine();
                
                if (int.TryParse(input, out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            return true; // Start game
                        case 2:
                            ShowRules();
                            break;
                        case 3:
                            SetNumberOfPlayers();
                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine("\n                    Thanks for playing UNO! Goodbye!\n");
                            return false; // Quit
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n                    Invalid choice! Press Enter to try again...");
                            Console.ResetColor();
                            Console.ReadLine();
                            break;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n                    Invalid input! Press Enter to try again...");
                    Console.ResetColor();
                    Console.ReadLine();
                }
            }
        }

        public void Play()
        {
            if (!ShowMainMenu())
            {
                return; // User chose to quit
            }
            
            InitializeDeck();
            InitializePlayers();

            // Place first card
            Card firstCard = DrawFromDeck();
            // Make sure first card is not a special card
            while (firstCard.Value == CardValue.Wild || firstCard.Value == CardValue.WildDrawFour)
            {
                deck.Add(firstCard);
                ShuffleDeck();
                firstCard = DrawFromDeck();
            }
            discardPile.Add(firstCard);

            Console.Clear();
            Console.WriteLine("Starting card:");
            firstCard.DisplayCard();
            Thread.Sleep(1500);

            // Handle first card effects
            if (firstCard.Value == CardValue.Skip || 
                firstCard.Value == CardValue.DrawTwo || 
                firstCard.Value == CardValue.Reverse)
            {
                ExecuteCardEffect(firstCard, players[0]);
            }

            // Game loop
            bool gameWon = false;
            while (!gameWon)
            {
                DisplayGameState();
                Player currentPlayer = players[currentPlayerIndex];
                
                gameWon = PlayerTurn(currentPlayer);
                
                if (gameWon)
                {
                    Console.Clear();
                    Console.WriteLine("╔════════════════════════════════════════════════════════════════════════╗");
                    Console.WriteLine("║                                                                        ║");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    string winMessage = currentPlayer.Name == "You" ? "You Win!" : $"{currentPlayer.Name} WINS!";
                    Console.WriteLine($"║                    🎉 {winMessage.PadLeft(20)} 🎉                        ║");
                    Console.ResetColor();
                    Console.WriteLine("║                                                                        ║");
                    Console.WriteLine("╚════════════════════════════════════════════════════════════════════════╝");
                    break;
                }

                if (!currentPlayer.IsComputer)
                {
                    Console.WriteLine("\n─────────────────────────────────────────────────────────────────────────");
                    Console.Write("Press Enter to continue...");
                    Console.ReadLine();
                }

                // Only move to next player if not already moved by card effect
                if (GetTopCard().Value != CardValue.Skip && 
                    GetTopCard().Value != CardValue.DrawTwo && 
                    GetTopCard().Value != CardValue.WildDrawFour)
                {
                    MoveToNextPlayer();
                }
            }

            Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                          FINAL STANDINGS                               ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════════╣");
            foreach (var player in players.OrderBy(p => p.Hand.Count))
            {
                string nameDisplay = player.Name.PadRight(20);
                string cardDisplay = $"{player.Hand.Count} cards".PadLeft(10);
                Console.WriteLine($"║  {nameDisplay}  {cardDisplay}                                 ║");
            }
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════════╝");
        }
    }
}
namespace UnoGame
{
    // Card colors for UNO
    public enum CardColor
    {
        Red,
        Yellow,
        Green,
        Blue,
        Wild
    }
}

