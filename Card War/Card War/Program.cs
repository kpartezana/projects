using Card_War.Models;
using System;
using System.Collections.Generic;

namespace Card_War
{
    class Program
    {
        static void Main(string[] args)
        {
            string gameType;
            string gameTypeTitle;
            string player1Name;
            string player2Name;
            int warDownCardCount;
            int rankDownCardCount;
            int p1HandsWon = 0;
            int p1WarsWon = 0;
            int p2HandsWon = 0;
            int p2WarsWon = 0;
            int handsPlayed = 0;
            bool isGameOver = false;
            const int startingCards = 26;

            WinningPlayer winningPlayer = new WinningPlayer();

            while (true) // Game loop
            {
                // Print title screen
                PrintTitleScreen();

                // Print instruction screen 1 then 2 and get game type
                PrintInstructions1();
                const string validGameTypes = "123q";
                gameType = PrintInstructions2();
                while (!validGameTypes.Contains(gameType))
                {
                    gameType = PrintInstructions2();
                }
                if (gameType == "q")
                {
                    break;
                }
                gameTypeTitle = GetGameTypeTitle(gameType);
                warDownCardCount = GetWarCount(gameType);
                rankDownCardCount = warDownCardCount;

                // Get player names
                string[] bothPlayers = PlayerInput().Split(",");
                player1Name = bothPlayers[0];
                player2Name = bothPlayers[1];
                int namePad = Math.Max(player1Name.Length, player2Name.Length) + 1;

                // Create and shuffle deck
                Deck fullDeck = new Deck("full");
                fullDeck.Shuffle();

                // Deal the starting cards
                Deck p1Deck = new Deck("");
                Deck p2Deck = new Deck("");

                for (int i = 1; i <= startingCards; i++)
                {
                    p1Deck.AddOne(fullDeck.DealOne());
                    p2Deck.AddOne(fullDeck.DealOne());
                }

                // Play game till it's over or they ask to quit
                string eachHandEntry = "";
                while ((eachHandEntry != "q") && (isGameOver == false)) // Deal Loop
                {
                    PrintTitleLine(gameTypeTitle);
                    DisplayGameStats(player1Name,
                                     p1Deck.Count(),
                                     p1HandsWon,
                                     p1WarsWon,
                                     player2Name,
                                     p2Deck.Count(),
                                     p2HandsWon,
                                     p2WarsWon,
                                     handsPlayed,
                                     namePad) ;

                    // Play hand / ask for deal count
                    eachHandEntry = GetDealCount();

                    List<Card> tableCards = new List<Card>(); // List to hold cards on table
                    int compareIndex; // Index of first card to compare in List of tableCards

                    if (Int32.TryParse(eachHandEntry, out int dealCount)) // Play dealCount number of hands if not 'q'
                    {
                        for (int i = 1; i <= dealCount; i++) // Hand loop
                        {
                            // Play a number of hands
                            tableCards = DealHand(p1Deck, p2Deck, tableCards, winningPlayer, player1Name, player2Name);
                            if (winningPlayer.Name != "")
                            {
                                isGameOver = true;
                                break;
                            }
                            // handsPlayed++; // Used here, will count Wars as hands
                            compareIndex = 0;
                            PrintTitleLine(gameTypeTitle); // Print title
                            DisplayGameStats(player1Name,
                                             p1Deck.Count(),
                                             p1HandsWon,
                                             p1WarsWon,
                                             player2Name,
                                             p2Deck.Count(),
                                             p2HandsWon,
                                             p2WarsWon,
                                             handsPlayed,
                                             namePad); // Print game stats
                            DisplayHand(tableCards, handsPlayed, player1Name, player2Name, namePad); // Display the hand in progress


                            int compareResult = CardCompare(tableCards, compareIndex);
                            if (compareResult > 0) // Player 1 win
                            {
                                PlayerWin(p1Deck, tableCards);
                                p1HandsWon++;
                                // Console.WriteLine();
                                Console.WriteLine($"{player1Name} Wins the hand!");
                                System.Threading.Thread.Sleep(2000);
                            }
                            else if (compareResult < 0) // Player 2 win
                            {
                                PlayerWin(p2Deck, tableCards);
                                p2HandsWon++;
                                // Console.WriteLine();
                                Console.WriteLine($"{player2Name} Wins the hand!");
                                System.Threading.Thread.Sleep(2000);
                            }
                            else // War
                            {
                                int warCount = 0;
                                compareIndex = 0;
                                while (true) // War loop
                                {
                                    if (warDownCardCount == 0)
                                    {
                                        rankDownCardCount = Math.Min(tableCards[compareIndex].Rank, 10);
                                        if (tableCards[compareIndex].Rank == 14)
                                        {
                                            rankDownCardCount = 1; // Make Ace as a 1 rank for rank card war
                                        }
                                    }

                                    tableCards = DealWar(p1Deck, p2Deck, tableCards, rankDownCardCount, winningPlayer, player1Name, player2Name);
                                    warCount++;
                                    if (winningPlayer.Name != "")
                                    {
                                        isGameOver = true;
                                        break;
                                    }

                                    compareIndex += (rankDownCardCount + 1) * 2;
                                    DisplayWar(tableCards, compareIndex, rankDownCardCount, player1Name, player2Name, namePad, warCount);
                                    compareResult = CardCompare(tableCards, compareIndex);
                                    if (compareResult > 0) // Player 1 win
                                    {
                                        PlayerWin(p1Deck, tableCards);
                                        p1WarsWon++;
                                        Console.WriteLine();
                                        Console.WriteLine($"{player1Name} Wins the War!");
                                        System.Threading.Thread.Sleep(4000);
                                        Console.WriteLine("Temp Entry Required"); Console.ReadKey();
                                        break;
                                    }
                                    else if (compareResult < 0) // Player 2 win
                                    {
                                        PlayerWin(p2Deck, tableCards);
                                        p2WarsWon++;
                                        Console.WriteLine();
                                        Console.WriteLine($"{player2Name} Wins the War!");
                                        System.Threading.Thread.Sleep(4000);
                                        Console.WriteLine("Temp Entry Required"); Console.ReadKey();

                                        break;
                                    }

                                    if (isGameOver == true)
                                    {
                                        break;
                                    }
                                } // War loop End

                                if (isGameOver == true)
                                {
                                    break;
                                }
                            }

                            handsPlayed++; // Used here will not count Wars as hands
                        } // Hand loop End
                    }
                    if (isGameOver == true)
                    {
                        break;
                    }
                } // Deal loop End

                if (winningPlayer.Name != "")
                {
                    GameIsOver(winningPlayer);
                }
                break; // Program exit
            } // Game loop End
        }

        private static void DisplayHand(List<Card> tableCards,
                                        int handsPlayed,
                                        string player1Name,
                                        string player2Name,
                                        int namePad)
        {
            Console.WriteLine();
            Console.WriteLine($"Hand {handsPlayed + 1} in progress:");
            Console.WriteLine($"{player1Name.PadRight(namePad)}: {tableCards[0].Title}");
            Console.WriteLine($"{player2Name.PadRight(namePad)}: {tableCards[1].Title}");
            Console.WriteLine();
        }

        private static void DisplayWar(List<Card> tableCards,
                                       int compareIndex,
                                       int warDownCardCount,
                                       string player1Name,
                                       string player2Name,
                                       int namePad,
                                       int warCount)
        {
            Console.WriteLine();
            Console.WriteLine($"War: Battle# {warCount}");
            Console.WriteLine($"{player1Name.PadRight(namePad)}: Down Cards- {warDownCardCount}? War Cards: {tableCards[compareIndex].Title}");
            Console.WriteLine($"{player2Name.PadRight(namePad)}: Down Cards- {warDownCardCount}? War Cards: {tableCards[compareIndex + 1].Title}");
        }

        private static void PlayerWin(Deck playerDeck, List<Card> wonCards)
        {
            foreach (Card card in wonCards)
            {
                playerDeck.AddOne(card);
            }
            wonCards.Clear();
        }

        private static List<Card> DealHand(Deck p1Deck,
                                           Deck p2Deck,
                                           List<Card> tableCards,
                                           WinningPlayer winningPlayer,
                                           string player1Name,
                                           string player2Name)
        {
            if (p1Deck.Count() > 0)
            {
                tableCards.Add(p1Deck.DealOne());
            }
            else
            {
                winningPlayer.Name = player2Name;
            }

            if (p2Deck.Count() > 0)
            {
                tableCards.Add(p2Deck.DealOne());
            }
            else
            {
                winningPlayer.Name = player1Name;
            }

            if ((p1Deck.Count() == 0) && (p2Deck.Count() == 0))
            {
                winningPlayer.Name = "Draw";
            }

            return tableCards;
        }

        private static List<Card> DealWar(Deck p1Deck,
                                          Deck p2Deck,
                                          List<Card> tableCards,
                                          int warDownCardCount,
                                          WinningPlayer winningPlayer,
                                          string player1Name,
                                          string player2Name)
        {
            for (int i = 1; i <= warDownCardCount + 1; i++)
            {
                if (p1Deck.Count() > 0)
                {
                    tableCards.Add(p1Deck.DealOne());
                }
                else
                {
                    winningPlayer.Name = player2Name;
                }

                if (p2Deck.Count() > 0)
                {
                    tableCards.Add(p2Deck.DealOne());
                }
                else
                {
                    winningPlayer.Name = player1Name;
                }

                if ((p1Deck.Count() == 0) && (p2Deck.Count() == 0))
                {
                    winningPlayer.Name = "Draw";
                }

                if (winningPlayer.Name != "")
                {
                    break;
                }

            }
            return tableCards;
        }


        private static int CardCompare(List<Card> tableCards, int compareIndex)
        {
            return tableCards[compareIndex].Rank - tableCards[compareIndex + 1].Rank;
        }
        private static void AnyKeyContinue()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            Console.Clear();
        }

        private static void PrintTitleScreen()
        {
            Console.WriteLine("Card War!  v1.0");
            Console.WriteLine("Program by KP. Tech Elevator .NET Cohort 12");

            AnyKeyContinue();
        }

        private static void PrintTitleLine(string gameTypeTitle)
        {
            Console.Clear();
            Console.WriteLine("Card War in progress!");
            Console.WriteLine(value: gameTypeTitle);
        }

        private static string GetDealCount()
        {
            Console.WriteLine();
            Console.Write("Enter a number of hands to deal or 'Q' to quit: ");

            string input = Console.ReadLine().Trim().ToLower();
            return input;
        }
        private static void PrintInstructions1()
        {
            Console.WriteLine("Card War!");
            Console.WriteLine();
            Console.WriteLine("Card War is a card game between two players. Each player");
            Console.WriteLine("turns up one card from the top of their pile. The player");
            Console.WriteLine("with the highest card takes both cards and adds them to");
            Console.WriteLine("to the bottom of their pile. If the cards are equal in");
            Console.WriteLine("rank it is War!");
            Console.WriteLine();
            Console.WriteLine("In the event of War, each player places a number of cards");
            Console.WriteLine("face down and the next face up. The player who has the");
            Console.WriteLine("highest card wins all of the cards. If the War cards are");
            Console.WriteLine("equal, the process is repeated.");
            Console.WriteLine();
            Console.WriteLine("The first player to run out of cards loses. If both");
            Console.WriteLine("players run out at the same time, the game is a draw.");
            Console.WriteLine();

            AnyKeyContinue();
        }

        private static string PrintInstructions2()
        {
            Console.Clear();
            Console.WriteLine("Card War!");
            Console.WriteLine();
            Console.WriteLine("There are three ways to play. When War is declared:");
            Console.WriteLine();
            Console.WriteLine("1) Each player places 1 card face down.");
            Console.WriteLine("2) Each player places 3 cards face down.");
            Console.WriteLine("3) Each player places a number of cards equal");
            Console.WriteLine("   to the rank of the War cards. (Ace = 1, 3 = 3,");
            Console.WriteLine("   and face cards = 10.");
            Console.WriteLine();
            Console.Write("   Please choose how you want to play. Q to quit: ");

            string input = Console.ReadLine().Trim().ToLower();
            return input;
        }

        private static string PlayerInput()
        {
            Console.Clear();
            Console.WriteLine("Card War!");
            Console.WriteLine();
            Console.Write("Enter the name of Player 1: ");
            string p1Input = Console.ReadLine().Trim();
            Console.WriteLine();
            Console.Write("Enter the name of Player 2: ");
            string p2Input = Console.ReadLine().Trim();
            return p1Input + "," + p2Input;
        }

        private static string GetGameTypeTitle(string gameType)
        {
            if (gameType == "1")
            {
                return "One Card War";
            }
            else if (gameType == "2")
            {
                return "3 Card War"; 
            }
            else if (gameType == "3")
            {
                return "Rank Card War";
            }
            else
            {
                // We did not get a valid value
                throw new ArgumentException("Invalid game type");
            }
        }

        private static int GetWarCount(string gameType)
        {
            if (gameType == "3")
            {
                return 0; // Deal equal to rank value
            }
            else if (gameType == "2")
            {
                return 3; // Deal three
            }
            else
            {
                return 1; // Deal one
            }
        }

        private static void DisplayGameStats(string player1Name,
                                             int p1CardCount,
                                             int p1HandsWon,
                                             int p1WarsWon,
                                             string player2Name,
                                             int p2CardCount,
                                             int p2HandsWon,
                                             int p2WarsWon,
                                             int handsPlayed,
                                             int namePad)
        {
            Console.WriteLine();
            Console.WriteLine($"Hands Played: {handsPlayed}");
            Console.WriteLine($"{player1Name.PadRight(namePad)}: {p1CardCount} Cards, {p1HandsWon} Hands Won, {p1WarsWon} Wars Won.");
            Console.WriteLine($"{player2Name.PadRight(namePad)}: {p2CardCount} Cards, {p2HandsWon} Hands Won, {p2WarsWon} Wars Won.");
        }

        private static void GameIsOver(WinningPlayer winningPlayer)
        {
            Console.WriteLine();

            if (winningPlayer.Name != "Draw")
            {
                Console.WriteLine($"*** {winningPlayer.Name} Wins the game!");
            }
            else
            {
                Console.WriteLine($"*** This game is a Draw!");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

    }
}
