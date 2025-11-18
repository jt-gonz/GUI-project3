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

##### **Development Log**

**Iteration 1:**

- **Goal/Task/Rationale:** Set up Project Structure.

- **What do you do?**
  - **Name**: Jonathan Gonzalez
  - **Action**:

**Iteration 2:**

- **Goal/Task/Rationale:** UNO

- **What do you do?**
  - **Name**: Jonathan Gonzalez
  - **Action**:

**Iteration 3:**

- **Goal/Task/Rationale:** UNO

- **What do you do?**
  - **Name**: Jonathan Gonzalez
  - **Action**:

**Iteration 4:**

- **Goal/Task/Rationale:** UNO

- **What do you do?**
  - **Name**: Jonathan Gonzalez
  - **Action**:

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

## Program Entry Point (`Program` class / `Main` function)

- **File:** `project3/Program.cs`  
- **Class:** `Program`  
- **Main Function:** `public static void Main()`

### Development Log

- **Iteration 1:**
  - **Goal/Task/Rationale:** Create a simple entry point to start the UNO game and keep `Main` minimal and focused.
  - **What do you do?**  
    Implemented a `Program` class with a `Main` method inside the `UnoGame` namespace. The method instantiates a `UnoGame` object and calls `Play()` to run the game loop. Kept `Main` free of game logic to follow separation of concerns.
  - **Response/Result:**  
    Code compiles and the game starts correctly by invoking `UnoGame.Play()`. `Main` now serves as a thin wrapper around the core game engine.
  - **Your Evaluation:** done – The entry point is clear and idiomatic. The code level is appropriate for an intermediate C# course (simple but correctly structured).

- **Iteration 2:**
  - **Goal/Task/Rationale:** Document and review how the entry point fits into the overall system and ensure it’s easy to extend (e.g., later menus, options).
  - **What do you do?**  
    Reviewed the structure of `Program.Main` and considered potential extensions such as command-line options or a main menu before starting the game. No code change was required at this point.
  - **Response/Result:**  
    Confirmed that `Main` is sufficiently decoupled from the rest of the logic, so adding pre-game configuration later will be straightforward.
  - **Your Evaluation:** done – Current design is simple and maintainable; code level is beginner-to-intermediate but follows good design practices.

---

## `UnoGame` Class

- **File:** `project3/UnoGame.cs`  
- **Class:** `UnoGame`

### Development Log

- **Iteration 1:**
  - **Goal/Task/Rationale:** Implement the core UNO game engine: deck management, player turns, discard pile, direction of play, and special cards.
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

- **Iteration 2:**
  - **Goal/Task/Rationale:** Review and refine game state presentation and turn-handling logic for usability and correctness.
  - **What do you do?**  
    - Examined `DisplayGameState()` to ensure the current player and next player are clearly highlighted (using console colors and arrows).
    - Verified correct computation of `nextPlayerIndex` depending on `isClockwise`.
    - Checked that the number of cards per player is displayed consistently in a horizontal layout using box-drawing characters.
    - Considered potential edge cases (e.g., only 2 players, reversing direction, nearly empty deck).
  - **Response/Result:**  
    The output is readable and indicates turn order and direction. Logic for determining the next player in clockwise/counter-clockwise play appears correct.
  - **Your Evaluation:** done – The code level is intermediate; console UI is more advanced than strictly required and shows attention to user experience. Future work could further separate presentation from game logic for cleaner design.

---

## `Player` Class

- **File:** `project3/Player.cs`  
- **Class:** `Player`

### Development Log

- **Iteration 1:**
  - **Goal/Task/Rationale:** Represent a player and manage their hand of cards, including the ability to draw and play cards.
  - **What do you do?**  
    - Implemented the `Player` class with properties `Name`, `Hand` (as `List<Card>`), and `IsComputer`.
    - Wrote a constructor to initialize the player name, create an empty hand, and flag whether the player is computer-controlled.
    - Implemented `DrawCard(Card)` to add cards to the player’s hand.
    - Implemented `PlayCard(int index)` to remove a card from the hand and return it, with basic bounds checking.
  - **Response/Result:**  
    Players can now receive cards during initialization and during the game. They can play cards by index, and invalid indices safely return `null`.
  - **Your Evaluation:** done – The `Player` data model is simple and clear. Code level is beginner-to-intermediate, using basic collections and encapsulation.

- **Iteration 2:**
  - **Goal/Task/Rationale:** Improve card visualization for a player’s hand and highlight playable cards.
  - **What do you do?**  
    - Implemented `DisplayHand(List<int>? playableIndices = null)` to draw all cards horizontally using `Card.DisplayInlineLine` across multiple lines.
    - Added logic to visually mark playable cards with a green check mark and index, and non-playable cards in a neutral/black color.
    - Ensured that an empty hand prints a clear message ("No cards in hand.").
  - **Response/Result:**  
    The player hand display is more user-friendly, making it easy to see which cards can be played on the current top card.
  - **Your Evaluation:** done – The visualization is a nice UX enhancement and demonstrates intermediate-level console formatting skills. Further work could separate logic from presentation for testing.

---

## `Card` Class

- **File:** `project3/Card.cs`  
- **Class:** `Card`

### Development Log

- **Iteration 1:**
  - **Goal/Task/Rationale:** Create a data structure to represent UNO cards and determine if a card can be played on top of another card.
  - **What do you do?**  
    - Implemented the `Card` class with `Color` (of type `CardColor`) and `Value` (of type `CardValue`).
    - Implemented `CanPlayOn(Card topCard, CardColor? currentWildColor)` with the following logic:
      - Wild and Wild Draw Four cards are always playable.
      - If the top card is a wild and a wild color has been declared, matching is based on that declared color.
      - Otherwise, the card must match either the color or value of the top card.
  - **Response/Result:**  
    Card playability can be correctly computed relative to the top discard card and any active wild color.
  - **Your Evaluation:** done – The rule logic is accurate for basic UNO rules. Code level is intermediate, with clear handling of wild-card edge cases.

- **Iteration 2:**
  - **Goal/Task/Rationale:** Implement console-friendly visual representations of cards to improve the user interface.
  - **What do you do?**  
    - Implemented `GetConsoleColor()` to map `CardColor` to appropriate `ConsoleColor` values.
    - Implemented `GetValueSymbol()` to map `CardValue` entries to display symbols (`0`–`9`, skip symbol, reverse arrow, "+2", "W", "+4").
    - Implemented `DisplayCard(...)` to draw a full card using box-drawing characters with optional highlighting and card numbering.
    - Implemented `DisplayInline()` and `DisplayInlineLine(int lineNum)` to support horizontally-aligned card rendering used by `Player.DisplayHand`.
  - **Response/Result:**  
    Cards display with appropriate colors and symbols in the console, giving the game a more polished look.
  - **Your Evaluation:** done – The display code is somewhat advanced for a console project and demonstrates careful attention to formatting. Code level is intermediate-to-advanced for console output, though it could be refactored to reduce duplication between display methods.

---

## `CardValue` Enum

- **File:** `project3/CardValue.cs`  
- **Type:** `enum CardValue`

### Development Log

- **Iteration 1:**
  - **Goal/Task/Rationale:** Enumerate all possible UNO card values to ensure type safety and readability in the rest of the code.
  - **What do you do?**  
    Defined an `enum CardValue` including numbered values (`Zero` through `Nine`) and action values (`Skip`, `Reverse`, `DrawTwo`, `Wild`, `WildDrawFour`).
  - **Response/Result:**  
    The rest of the code can reference card values using descriptive names instead of magic numbers or strings, making it easier to reason about the rules.
  - **Your Evaluation:** done – Straightforward and appropriate use of an enum. Code level is beginner but correct and idiomatic.

- **Iteration 2:**
  - **Goal/Task/Rationale:** Integrate the enum consistently throughout card logic and display code.
  - **What do you do?**  
    Used `CardValue` throughout the `Card` and `UnoGame` classes for deck creation, rule checking, and display. Verified mapping between enum values and console symbols in `GetValueSymbol()`.
  - **Response/Result:**  
    All references to card values are unified under the enum, which simplifies maintenance and reduces bugs.
  - **Your Evaluation:** done – Enum usage is consistent and clear. The code level is appropriate and supports good design practices.

---

## `CardColor` Enum

- **File:** `project3/CardColor.cs`  
- **Type:** `enum CardColor`

### Development Log

- **Iteration 1:**
  - **Goal/Task/Rationale:** Define all possible card colors (including wild) for use in the UNO game.
  - **What do you do?**  
    Implemented an enum `CardColor` (or prepared the file for it) to represent `Red`, `Yellow`, `Green`, `Blue`, and `Wild`. This supports type-safe color handling and mapping to console colors.
  - **Response/Result:**  
    Game logic and display code can work with well-defined color values rather than raw strings or integers.
  - **Your Evaluation:** done – Code level is beginner but necessary for clean design. If the enum definition is still incomplete, that should be finalized to match how it is used in `Card` and `UnoGame`.

- **Iteration 2:**
  - **Goal/Task/Rationale:** Connect the color enum to card creation and UI logic.
  - **What do you do?**  
    - Used `CardColor` when constructing cards in `InitializeDeck()`.
    - Mapped `CardColor` values to `ConsoleColor` in `Card.GetConsoleColor()` for consistent rendering.
    - Verified that wild cards use `CardColor.Wild` and can be treated specially in rules and rendering.
  - **Response/Result:**  
    Color handling is consistent across the game, and wild cards are clearly distinguished from standard colored cards.
  - **Your Evaluation:** done – Integration is correct; overall color handling shows intermediate understanding of enums and UI mapping.
