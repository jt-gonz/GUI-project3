using System;
using System.Collections.Generic;
using System.Linq;

namespace UnoGame
{
    // Card colors for UNO
    enum CardColor
    {
        Red,
        Yellow,
        Green,
        Blue,
        Wild
    }

    // Card values
    enum CardValue
    {
        Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine,
        Skip, Reverse, DrawTwo, Wild, WildDrawFour
    }

    // UNO Card class
    class Card
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

        public void Display()
        {
            ConsoleColor consoleColor = Color switch
            {
                CardColor.Red => ConsoleColor.Red,
                CardColor.Yellow => ConsoleColor.Yellow,
                CardColor.Green => ConsoleColor.Green,
                CardColor.Blue => ConsoleColor.Cyan,
                CardColor.Wild => ConsoleColor.Magenta,
                _ => ConsoleColor.White
            };

            Console.ForegroundColor = consoleColor;
            Console.Write($"[{Color} {Value}]");
            Console.ResetColor();
        }

        public override string ToString()
        {
            return $"{Color} {Value}";
        }
    }

    // Player class
    class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; set; }
        public bool IsComputer { get; set; }

        public Player(string name, bool isComputer = false)
        {
            Name = name;
            Hand = new List<Card>();
            IsComputer = isComputer;
        }

        public void DrawCard(Card card)
        {
            Hand.Add(card);
        }

        public void DisplayHand()
        {
            Console.Write($"{Name}'s hand ({Hand.Count} cards): ");
            for (int i = 0; i < Hand.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                Hand[i].Display();
                Console.Write(" ");
            }
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

    // UNO Game class
    class UnoGame
    {
        private List<Card> deck;
        private List<Card> discardPile;
        private List<Player> players;
        private int currentPlayerIndex;
        private bool isClockwise;
        private CardColor? currentWildColor;
        private Random random;

        public UnoGame()
        {
            deck = new List<Card>();
            discardPile = new List<Card>();
            players = new List<Player>();
            currentPlayerIndex = 0;
            isClockwise = true;
            currentWildColor = null;
            random = new Random();
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
                    Console.WriteLine("Reshuffling discard pile into deck...");
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
            players.Add(new Player("You", false));
            players.Add(new Player("Computer 1", true));
            players.Add(new Player("Computer 2", true));
            players.Add(new Player("Computer 3", true));

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
            Console.WriteLine("\n" + new string('=', 60));
            Console.Write("Top card: ");
            GetTopCard().Display();
            if (currentWildColor.HasValue)
            {
                Console.Write(" (Current color: ");
                Console.ForegroundColor = currentWildColor.Value switch
                {
                    CardColor.Red => ConsoleColor.Red,
                    CardColor.Yellow => ConsoleColor.Yellow,
                    CardColor.Green => ConsoleColor.Green,
                    CardColor.Blue => ConsoleColor.Cyan,
                    _ => ConsoleColor.White
                };
                Console.Write(currentWildColor.Value);
                Console.ResetColor();
                Console.Write(")");
            }
            Console.WriteLine("\n");

            // Show all players' card counts
            foreach (var player in players)
            {
                Console.WriteLine($"{player.Name}: {player.Hand.Count} cards");
            }
            Console.WriteLine();
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
                Console.WriteLine("Choose a color:");
                Console.WriteLine("1. Red");
                Console.WriteLine("2. Yellow");
                Console.WriteLine("3. Green");
                Console.WriteLine("4. Blue");
                
                while (true)
                {
                    Console.Write("Enter choice (1-4): ");
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
                    Console.WriteLine($"Skip! Next player loses their turn.");
                    MoveToNextPlayer();
                    break;

                case CardValue.Reverse:
                    isClockwise = !isClockwise;
                    Console.WriteLine($"Reverse! Direction changed.");
                    if (players.Count == 2)
                        MoveToNextPlayer(); // In 2-player, reverse acts like skip
                    break;

                case CardValue.DrawTwo:
                    MoveToNextPlayer();
                    Player nextPlayer = players[currentPlayerIndex];
                    Console.WriteLine($"{nextPlayer.Name} draws 2 cards!");
                    nextPlayer.DrawCard(DrawFromDeck());
                    nextPlayer.DrawCard(DrawFromDeck());
                    MoveToNextPlayer();
                    break;

                case CardValue.Wild:
                    currentWildColor = ChooseWildColor(player);
                    Console.WriteLine($"{player.Name} chose {currentWildColor}");
                    break;

                case CardValue.WildDrawFour:
                    currentWildColor = ChooseWildColor(player);
                    Console.WriteLine($"{player.Name} chose {currentWildColor}");
                    MoveToNextPlayer();
                    Player drawFourPlayer = players[currentPlayerIndex];
                    Console.WriteLine($"{drawFourPlayer.Name} draws 4 cards!");
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
            Console.WriteLine($"\n>>> {player.Name}'s turn <<<");
            
            if (!player.IsComputer)
            {
                player.DisplayHand();
                Console.Write("Top card: ");
                GetTopCard().Display();
                Console.WriteLine();

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
                    Console.WriteLine("You have no playable cards. Drawing 2 cards and skipping turn...");
                    player.DrawCard(DrawFromDeck());
                    player.DrawCard(DrawFromDeck());
                    Console.WriteLine($"You drew 2 cards. You now have {player.Hand.Count} cards.");
                }
                else
                {
                    Console.WriteLine($"Playable cards: {string.Join(", ", playableIndices.Select(i => i + 1))}");
                    
                    while (true)
                    {
                        Console.Write("Enter card number to play (or 0 to draw 2 and skip): ");
                        string? input = Console.ReadLine();
                        
                        if (int.TryParse(input, out int choice))
                        {
                            if (choice == 0)
                            {
                                Console.WriteLine("Drawing 2 cards and skipping turn...");
                                player.DrawCard(DrawFromDeck());
                                player.DrawCard(DrawFromDeck());
                                Console.WriteLine($"You drew 2 cards. You now have {player.Hand.Count} cards.");
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
                                        discardPile.Add(playedCard);
                                        Console.Write($"{player.Name} plays: ");
                                        playedCard.Display();
                                        Console.WriteLine();
                                        ExecuteCardEffect(playedCard, player);
                                    }
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("That card cannot be played!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid card number!");
                            }
                        }
                    }
                }
            }
            else
            {
                // Computer player logic
                System.Threading.Thread.Sleep(800); // Pause for effect
                
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
                    Console.WriteLine($"{player.Name} has no playable cards. Drawing 2 cards and skipping turn...");
                    player.DrawCard(DrawFromDeck());
                    player.DrawCard(DrawFromDeck());
                }
                else
                {
                    // Play a random playable card (simple AI)
                    int randomIndex = playableIndices[random.Next(playableIndices.Count)];
                    Card? playedCard = player.PlayCard(randomIndex);
                    if (playedCard != null)
                    {
                        discardPile.Add(playedCard);
                        Console.Write($"{player.Name} plays: ");
                        playedCard.Display();
                        Console.WriteLine();
                        ExecuteCardEffect(playedCard, player);
                    }
                }
            }

            // Check if player won
            return player.Hand.Count == 0;
        }

        public void Play()
        {
            Console.WriteLine("Welcome to UNO!");
            Console.WriteLine("================");
            
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

            Console.WriteLine("\nStarting game...");
            Console.Write("First card: ");
            firstCard.Display();
            Console.WriteLine("\n");

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
                    Console.WriteLine($"\n🎉 {currentPlayer.Name} wins! 🎉");
                    break;
                }

                if (!currentPlayer.IsComputer)
                {
                    Console.Write("\nPress Enter to continue...");
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

            Console.WriteLine("\nFinal standings:");
            foreach (var player in players.OrderBy(p => p.Hand.Count))
            {
                Console.WriteLine($"{player.Name}: {player.Hand.Count} cards");
            }
        }
    }

    class Program
    {
        public static void Main()
        {
            UnoGame game = new UnoGame();
            game.Play();
        }
    }
}