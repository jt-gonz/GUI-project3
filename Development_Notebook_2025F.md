**CSCI-310 Development Notebook**

---

**Guideline:**

- Please document all your development activities, whether you use any AI coding tool or not. You might mix your manual coding or AI tool usage. Just document the entire process.

- If this is a team project or assignment, list all team members’ names in the “Name” field. For each iteration, record the name of the person who contributed any part of the work in the “What do you do?” field.

- Any interactions with AI coding tools such as ChatGPT, Gemini, Copilot, and others must capture the full conversation history.

- Use the format below to record your development activities in a clear and consistent manner.

- Adding more iteration sections if needed.

---

#### **Name:**

Jonathan Gonzalez

Jesutofunmi Oyeleke

#### **Project/Assignment:**

Project 3: Command Line Game

##### **Problem/Task:**

In this project, you will design and implement an interactive console-based game using
C#. The game should run entirely in the console window and provide a responsive and engaging
gameplay experience. While the inspiration comes from classic block-based games, you are not
allowed to recreate Tetris. You are encouraged to be creative and implement your own unique
game mechanics.

## Program Entry Point (`Program` class / `Main` function)

- **File:** `project3/Program.cs`
- **Class:** `Program`
- **Main Function:** `public static void Main()`

##### **Development Log**

**Iteration 1:**

- **Goal/Task/Rationale:** Set up Project Structure.

- **What do you do?**
  - **Name**: Jonathan Gonzalez
  - **What do you do?**  
    Implemented a `Program` class with a `Main` method inside the `UnoGame` namespace. The method instantiates a `UnoGame` object and calls `Play()` to run the game loop. Kept `Main` free of game logic to follow separation of concerns.
  - **Response/Result:**  
    Code compiles and the game starts correctly by invoking `UnoGame.Play()`. `Main` now serves as a thin wrapper around the core game engine.
  - **Your Evaluation:** done – The entry point is clear and idiomatic. The code level is appropriate for an intermediate C# course (simple but correctly structured).

**Iteration 2:**

- **What do you do?**
  - **Name**: Jonathan Gonzalez
  - **What do you do?**  
    Reviewed the structure of `Program.Main` and considered potential extensions such as command-line options or a main menu before starting the game. No code change was required at this point.
  - **Response/Result:**  
    Confirmed that `Main` is sufficiently decoupled from the rest of the logic, so adding pre-game configuration later will be straightforward.
  - **Your Evaluation:** done – Current design is simple and maintainable; code level is beginner-to-intermediate but follows good design practices.

**Iteration 3:**

- **Goal/Task/Rationale:** Implement the core UNO game engine: deck management, player turns, discard pile, direction of play, and special cards.

- **What do you do?**
  - **Name**: Jonathan Gonzalez
  - **What do you do?**
    - Designed fields for `deck`, `discardPile`, `players`, `currentPlayerIndex`, `isClockwise`, `currentWildColor`, `random`, and `numberOfPlayers`.
    - Implemented `InitializeDeck()` to generate the standard UNO deck with numbered cards, action cards (Skip, Reverse, DrawTwo), and wild cards (Wild, WildDrawFour), and then shuffle it.
    - Implemented `ShuffleDeck()` using a Fisher–Yates shuffle with a `Random` instance.
    - Implemented `DrawFromDeck()` to draw a card, including the logic to reshuffle the discard pile back into the deck when needed while preserving the top card.
    - Implemented `InitializePlayers()` to create one human player ("You") and several computer players, and deal them 8 cards each.
    - Added methods like `GetTopCard()` and `DisplayGameState()` to show current players, card counts, and turn direction in a stylized console UI.
  - **Response/Result:**  
    The game can initialize a full deck and set up multiple players. Deck and discard reshuffling work without crashing even when the deck runs out. The console UI shows players and card counts, helping visualize the state.
  - **Your Evaluation:** done – The logic is relatively robust and covers key UNO rules. The code level is intermediate: it uses collections, loops, and some stateful logic appropriately, though there is room to further refactor for readability (e.g., splitting UI-specific code from game rules).

**Iteration 4:**

- - **Goal/Task/Rationale:** Represent a player and manage their hand of cards, including the ability to draw and play cards.

- **What do you do?**
  - **Name**: Jonathan Gonzalez
  - **What do you do?**
    - Implemented the `Player` class with properties `Name`, `Hand` (as `List<Card>`), and `IsComputer`.
    - Wrote a constructor to initialize the player name, create an empty hand, and flag whether the player is computer-controlled.
    - Implemented `DrawCard(Card)` to add cards to the player’s hand.
    - Implemented `PlayCard(int index)` to remove a card from the hand and return it, with basic bounds checking.
  - **Response/Result:**  
    Players can now receive cards during initialization and during the game. They can play cards by index, and invalid indices safely return `null`.
  - **Your Evaluation:** done – The `Player` data model is simple and clear. Code level is beginner-to-intermediate, using basic collections and encapsulation.

**Iteration 5:**

- **Goal/Task/Rationale:** Create Developer Notebooks

- **What do you do?**
  - **Name**: Jesutofunmi Oyeleke
  - **Action**: I was responsible for creating the developer notebooks for both of us! It was a pretty straightforward process

**Iteration 6:**

- **Goal/Task/Rationale:** Create Slides

- **What do you do?**
  - **Name**: Jesutofunmi Oyeleke
  - **Action**: I was also responsible for creating the slides for our in-class presentation

**Iteration 7:**

- **Goal/Task/Rationale:** Add Ability to say "uno"

- **What do you do?**
  - **Name**: Jesutofunmi Oyeleke
  - **Action**: Updated Code to make sure each player said "uno" when they had one card left which is very important for the game
