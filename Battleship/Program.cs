using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShip.BLL.GameLogic;
using BattleShip.BLL.Requests;
using BattleShip.BLL.Ships;
using System.Globalization;
using BattleShip.BLL.Responses;

namespace BattleShip.UI
{
    public class Player
    {
        public Board internalBoard { get; set; }
        public string playerName { get; set; }
        public string[,] displayBoard { get; set; }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            bool playGame = true;
            Program program = new Program();

            while (playGame)
            {


                Player Player1 = new Player()
                {
                    displayBoard = program.MakeNewDisplayBoard(),
                    internalBoard = new Board()

                };

                Player Player2 = new Player()
                {
                    displayBoard = program.MakeNewDisplayBoard(),
                    internalBoard = new Board()
                };

                Player1 = GetPlayerName("1", Player1);
                Player2 = GetPlayerName("2", Player2);

                //These methods setup the boards for each player and they contain multiple other methods
                Player1 = SetupBoards(Player1);
                Player2 = SetupBoards(Player2);


                Gameplay(program, Player1, Player2);
                playGame = NewGame();
            }

        }

        private static bool NewGame()
        {
            bool playGame = true;
            Console.WriteLine("Do you want to play again? Enter Y/N. ");
            string response = Console.ReadLine();
            response = response.ToUpper();
            if (response == "Y")
            {
                return playGame;
            }
            else
            {
                playGame = false;
                return playGame;
            }
        }

        private static void Gameplay(Program program, Player Player1, Player Player2)
        {
            string[,] shotboard1 = program.MakeNewDisplayBoard();
            string[,] shotboard2 = program.MakeNewDisplayBoard();

            for (int i = 0; i < 100; i++)
            {
                here:
                PrintBoard(shotboard1);
                Console.WriteLine($"{Player1.playerName} please enter shot: ");
                Coordinate firstShot = GetCoordinate(Player2);
                Console.Clear();
                FireShotResponse f = Player2.internalBoard.FireShot(firstShot);

                int x1 = firstShot.XCoordinate;
                int y1 = firstShot.YCoordinate;

                if (f.ShotStatus == ShotStatus.Hit)
                {
                    shotboard1[y1 * 2, x1 * 2] = " H ";
                    PrintBoard(shotboard1);
                    Console.WriteLine($"You hit {Player2.playerName}'s {f.ShipImpacted}!");
                    Console.ReadLine();
                    Console.Clear();
                }

                if (f.ShotStatus == ShotStatus.Miss)
                {

                    shotboard1[y1 * 2, x1 * 2] = " M ";
                    PrintBoard(shotboard1);
                    Console.WriteLine("You hit the ocean, try again next turn.");
                    Console.ReadLine();
                    Console.Clear();
                }

                if (f.ShotStatus == ShotStatus.Duplicate)
                {
                    Console.WriteLine("Oops, you already fired at this coordinate.  Please try again.");
                    PrintBoard(shotboard1);
                    Console.ReadLine();
                    Console.Clear();
                    goto here;
                }


                if (f.ShotStatus == ShotStatus.HitAndSunk)
                {
                    shotboard1[y1 * 2, x1 * 2] = " H ";
                    PrintBoard(shotboard1);
                    Console.WriteLine($"BOOM!!  You sunk {Player2.playerName}'s {f.ShipImpacted}!!");
                    Console.ReadLine();
                    Console.Clear();
                }

                if (f.ShotStatus == ShotStatus.Victory)
                {
                    Console.WriteLine($"You rule this ocean!! You have taken {Player2.playerName} prisoner and won the game!");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                }
                there:
                PrintBoard(shotboard2);
                Console.WriteLine($"{Player2.playerName} please enter shot: ");
                Coordinate secondShot = GetCoordinate(Player1);
                Console.Clear();
                FireShotResponse f2 = Player1.internalBoard.FireShot(secondShot);

                int x2 = secondShot.XCoordinate;
                int y2 = secondShot.YCoordinate;

                if (f2.ShotStatus == ShotStatus.Hit)
                {
                    shotboard2[y2 * 2, x2 * 2] = " H ";
                    PrintBoard(shotboard2);
                    Console.WriteLine($"You hit {Player1.playerName}'s {f2.ShipImpacted}!");
                    Console.ReadLine();
                    Console.Clear();
                }

                if (f2.ShotStatus == ShotStatus.Miss)
                {
                    shotboard2[y2 * 2, x2 * 2] = " M ";
                    PrintBoard(shotboard2);
                    Console.WriteLine("You hit the ocean, try again next turn.");
                    Console.ReadLine();
                    Console.Clear();
                }

                if (f2.ShotStatus == ShotStatus.Duplicate)
                {
                    Console.WriteLine("Oops, you already fired at this coordinate.  Please try again.");
                    PrintBoard(shotboard2);
                    Console.ReadLine();
                    Console.Clear();
                    goto there;
                }


                if (f2.ShotStatus == ShotStatus.HitAndSunk)
                {
                    shotboard2[y2 * 2, x2 * 2] = " H ";
                    PrintBoard(shotboard2);
                    Console.WriteLine($"BOOM!!  You sunk {Player1.playerName}'s {f2.ShipImpacted}!!");
                    Console.ReadLine();
                    Console.Clear();
                }

                if (f2.ShotStatus == ShotStatus.Victory)
                {
                    Console.WriteLine($"You rule this ocean!! You have taken {Player1.playerName} prisoner and won the game!");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                }
            }

        }

        public static Player GetPlayerName(string playerNumber, Player player)
        {
            bool valid = false;
            while (!valid)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t\t\tWelcome to BATTLESHIP!!");
                Console.ResetColor();
                Console.WriteLine("\nEnter Player " + playerNumber + "'s name: ");
                player.playerName = Console.ReadLine();
                Console.Clear();
                if (player.playerName == "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You must type a name. Try again.");
                    Console.ResetColor();
                }
                else
                    valid = true;
            }
            Console.Clear();

            return player;
        }

        public string[,] MakeNewDisplayBoard()
        {
            // This is the base board that gets assigned to player 1 and player 2
            string[,] battleshipBoard = new string[22, 22]
            {
                { "   ", "   ", " A ", "   ", " B ", "   ", " C ", "   ", " D ", "   ", " E ", "   ", " F ", "   ", " G ", "    ", "H ", "   ", " I ", "   ", " J ", "   "},
                { "   ", " --", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "--"},
                { " 1 ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | "},
                { "   ", " --", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "--"},
                { " 2 ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | "},
                { "   ", " --", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "--"},
                { " 3 ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | "},
                { "   ", " --", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "--"},
                { " 4 ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | "},
                { "   ", " --", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "--"},
                { " 5 ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | "},
                { "   ", " --", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "--"},
                { " 6 ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | "},
                { "   ", " --", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "--"},
                { " 7 ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | "},
                { "   ", " --", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "--"},
                { " 8 ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | "},
                { "   ", " --", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "--"},
                { " 9 ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | "},
                { "   ", " --", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "--"},
                { " 10", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | ", "   ", " | "},
                { "   ", " --", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "---", "--"},
            };
            return battleshipBoard;
        }


        public static void PrintBoard(string[,] playerBoard)
        {

            // Prints board to the screen and is player-specific
            for (int j = 0; j < 22; j++)
            {
                for (int k = 0; k < 22; k++)
                {

                    var Hit = playerBoard[j, k];

                    if (Hit == " H ")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(playerBoard[j, k]);
                        Console.ResetColor();
                    }

                    else if (Hit == " M ")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(playerBoard[j, k]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(playerBoard[j, k]);
                    }
                }
                Console.Write("\n");
            }
        }

        public static Player SetupBoards(Player player)
        {
            ShipType shipType;
            Coordinate coordinate;
            ShipDirection shipDirection;

            // Gets and preps ship locations from user.  The for loop ensures no more than 5 ships 
            // get placed onto board.
            PrintBoard(player.displayBoard);

            for (int i = 0; i < 5; i++)
            {
                bool valid = false;
                while (!valid)
                {
                    //PrintBoard(playerBoard);
                    Console.WriteLine("{0}, place ship {1}. ", player.playerName, i + 1);

                    // This method retrieves the type of ship from the player
                    shipType = GetShipType(player);

                    // This method retrieves coordinates from the player
                    coordinate = GetCoordinate(player);

                    // This method retrieves the direction the ship should be placed from the player
                    shipDirection = GetShipDirection(player);

                    // Next we input the 3 parameters specified by the player into the PlaceShipRequest method
                    PlaceShipRequest PlaceShipRequest = new PlaceShipRequest()
                    {
                        Coordinate = coordinate,
                        Direction = shipDirection,
                        ShipType = shipType
                    };

                    // The ShipPlacement method then attempts to place ship onto board                    
                    ShipPlacement value = player.internalBoard.PlaceShip(PlaceShipRequest);
                    if (value == ShipPlacement.NotEnoughSpace)
                    {
                        Console.Clear();
                        PrintBoard(player.displayBoard);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Not enough space to place ship. Press enter and try again.");
                        Console.ResetColor();
                        Console.ReadLine();
                        Console.Clear();
                        valid = false;
                    }
                    else if (value == ShipPlacement.Overlap)
                    {
                        Console.Clear();
                        PrintBoard(player.displayBoard);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("This ship overlaps another. Press enter and try again.");
                        Console.ResetColor();
                        Console.ReadLine();
                        valid = false;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Ship was succesfully placed! Press Enter to continue.");
                        Console.ResetColor();
                        player = PlaceShipOnBoard(player, i);
                        Console.ReadLine();
                        valid = true;
                    }
                }
            }
            Console.Clear();
            return player;
        }

        public static Coordinate GetCoordinate(Player player)
        {
            bool valid = false;
            string newCoord = "";
            string newCoordUpperCase = "";
            char[] newCoordUpperCaseArray = new char[3];
            string firstCoordFullyPrepped = "";
            string secondCoordFullyPrepped = "";
            int firstCoordInt;
            int secondCoordInt;

            while (valid == false)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Enter coordinate (ex: B6): ");
                Console.ResetColor();
                newCoord = Console.ReadLine();

                if (newCoord == null || newCoord == "")
                {
                    valid = false;
                }
                else if (!(newCoord.Length == 2 || newCoord.Length == 3))
                {
                    valid = false;
                }
                else
                {
                    newCoordUpperCase = newCoord.ToUpper();
                    newCoordUpperCaseArray = newCoordUpperCase.ToCharArray();
               
                    firstCoordFullyPrepped = newCoordUpperCaseArray[0].ToString();
                }
                // This 'if' statement tests whether the number entered by the user is either a single
                // or double digit and handles each accordingly
                if (newCoordUpperCaseArray.Length == 2)
                {
                    secondCoordFullyPrepped = newCoordUpperCaseArray[1].ToString();
                }
                else 
                {
                    secondCoordFullyPrepped = newCoordUpperCaseArray[1].ToString() + newCoordUpperCaseArray[2].ToString();
                }

                // think through whether these these if statements could be shortened somehow
                if (firstCoordFullyPrepped == "A")
                {
                    firstCoordFullyPrepped = "1";
                    valid = true;
                }
                else if (firstCoordFullyPrepped == "B")
                {
                    firstCoordFullyPrepped = "2";
                    valid = true;
                }
                else if (firstCoordFullyPrepped == "C")
                {
                    firstCoordFullyPrepped = "3";
                    valid = true;
                }
                else if (firstCoordFullyPrepped == "D")
                {
                    firstCoordFullyPrepped = "4";
                    valid = true;
                }
                else if (firstCoordFullyPrepped == "E")
                {
                    firstCoordFullyPrepped = "5";
                    valid = true;
                }
                else if (firstCoordFullyPrepped == "F")
                {
                    firstCoordFullyPrepped = "6";
                    valid = true;
                }
                else if (firstCoordFullyPrepped == "G")
                {
                    firstCoordFullyPrepped = "7";
                    valid = true;
                }
                else if (firstCoordFullyPrepped == "H")
                {
                    firstCoordFullyPrepped = "8";
                    valid = true;
                }
                else if (firstCoordFullyPrepped == "I")
                {
                    firstCoordFullyPrepped = "9";
                    valid = true;
                }
                else if (firstCoordFullyPrepped == "J")
                {
                    firstCoordFullyPrepped = "10";
                    valid = true;
                }
                else
                    Console.Clear();
                PrintBoard(player.displayBoard);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not a valid coordinate. Try again.");
                Console.ResetColor();
                // This could probably be shortened by using a for loop
                if (!(secondCoordFullyPrepped == "1"
                    || secondCoordFullyPrepped == "2"
                    || secondCoordFullyPrepped == "3"
                    || secondCoordFullyPrepped == "4"
                    || secondCoordFullyPrepped == "5"
                    || secondCoordFullyPrepped == "6"
                    || secondCoordFullyPrepped == "7"
                    || secondCoordFullyPrepped == "8"
                    || secondCoordFullyPrepped == "9"
                    || secondCoordFullyPrepped == "10"))
                {
                    Console.Clear();
                    PrintBoard(player.displayBoard);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Not a valid coordinate. Try again.");
                    Console.ResetColor();
                    valid = false;
                }
            }
            firstCoordInt = Int32.Parse(firstCoordFullyPrepped);
            secondCoordInt = Int32.Parse(secondCoordFullyPrepped);
            Console.Clear();
            PrintBoard(player.displayBoard);
            // Runs the converted user coordinate through the Coordinate method to get output necessary for
            // PlaceShipRequest Class.
            // Coordinate gets validated in the Board Class by the IsValidCoordinate method
            Coordinate coordinate = new Coordinate(firstCoordInt, secondCoordInt);
            return coordinate;
        }

        public static ShipDirection GetShipDirection(Player player)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            bool valid = false;
            string ShipDirection = "";

            while (valid == false)
            {
                // Gets and preps ship direction from user
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Enter ship direction (Up, Down, Left, or Right)");
                Console.ResetColor();
                ShipDirection = Console.ReadLine();
                if (ShipDirection == null || ShipDirection == "")
                {
                    valid = false;
                }
                ShipDirection = myTI.ToTitleCase(ShipDirection);
                if (!(ShipDirection == "Up"
                    || ShipDirection == "Down"
                    || ShipDirection == "Left"
                    || ShipDirection == "Right"))
                {
                    Console.Clear();
                    PrintBoard(player.displayBoard);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Not a valid direction. Try again.");
                    Console.ResetColor();
                }
                else
                {
                    valid = true;
                }
            }

            ShipDirection shipDirection = (ShipDirection)Enum.Parse(typeof(ShipDirection), ShipDirection);
            Console.Clear();
            return shipDirection;
        }

        public static ShipType GetShipType(Player player)
        {
            bool valid = false;
            bool valid2 = false;
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            string ShipType = "";
            List<string> shipsAlreadyPlaced = new List<string>();

            // Gets and preps ship type from user
            while (valid == false)
            {
                while (valid2 == false)
                {
                    Console.WriteLine("\nEnter number for type of ship.  Choices are: ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[1] Destroyer  {takes up 2 spaces}\n[2] Cruiser {3 spaces}" +
                                    "\n[3] Submarine {3 spaces}\n[4] Battleship {4 spaces}" +
                                    "\n[5] Carrier {5 spaces}");
                    Console.ResetColor();
                    ShipType = Console.ReadLine();

                    if(ShipType == null || ShipType == "")
                    {
                        valid2 = false;
                    }

                    switch (ShipType)

                    {
                        case "1":
                            ShipType = "Destroyer";
                            break;
                        case "2":
                            ShipType = "Cruiser";
                            break;
                        case "3":
                            ShipType = "Submarine";
                            break;
                        case "4":
                            ShipType = "Battleship";
                            break;
                        case "5":
                            ShipType = "Carrier";
                            break;
                    }

                    if (!(ShipType == "Destroyer"
                        || ShipType == "Cruiser"
                        || ShipType == "Submarine"
                        || ShipType == "Battleship"
                        || ShipType == "Carrier"))
                    {
                        Console.Clear();
                        PrintBoard(player.displayBoard);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Not a valid ship type. Try again.");
                        Console.ResetColor();
                        valid2 = false;
                    }
                    else
                        valid2 = true;

                    foreach (var ship in shipsAlreadyPlaced)
                    {
                        if (ship == ShipType)
                        {
                            Console.Clear();
                            PrintBoard(player.displayBoard);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This ship has already been placed. Try again.");
                            Console.ResetColor();
                            valid2 = false;
                        }
                        else
                            valid2 = true;
                    }
                }

                shipsAlreadyPlaced.Add(ShipType);
                valid = true;
            }

            // Use the ShipType enum here to prep the entry into proper PlaceShipRequest Class format
            ShipType shipType = (ShipType)Enum.Parse(typeof(ShipType), ShipType);
            Console.Clear();
            PrintBoard(player.displayBoard);
            return shipType;
        }

        public static Player PlaceShipOnBoard(Player player, int i)
        {
            int x1;
            int y1;

            switch (player.internalBoard._ships[i].ShipName)
            {
                case "Destroyer":
                    for (int j = 0; j < 2; j++)
                    {
                        y1 = player.internalBoard._ships[i].BoardPositions[j].XCoordinate;
                        x1 = player.internalBoard._ships[i].BoardPositions[j].YCoordinate;
                        player.displayBoard[x1 * 2, y1 * 2] = " D ";
                    }
                    break;

                case "Cruiser":
                    for (int j = 0; j < 3; j++)
                    {
                        y1 = player.internalBoard._ships[i].BoardPositions[j].XCoordinate;
                        x1 = player.internalBoard._ships[i].BoardPositions[j].YCoordinate;
                        player.displayBoard[x1 * 2, y1 * 2] = " Cr";
                    }
                    break;

                case "Submarine":
                    for (int j = 0; j < 3; j++)
                    {
                        y1 = player.internalBoard._ships[i].BoardPositions[j].XCoordinate;
                        x1 = player.internalBoard._ships[i].BoardPositions[j].YCoordinate;
                        player.displayBoard[x1 * 2, y1 * 2] = " S ";
                    }
                    break;

                case "Battleship":
                    for (int j = 0; j < 4; j++)
                    {
                        y1 = player.internalBoard._ships[i].BoardPositions[j].XCoordinate;
                        x1 = player.internalBoard._ships[i].BoardPositions[j].YCoordinate;
                        player.displayBoard[x1 * 2, y1 * 2] = " B ";
                    }
                    break;

                case "Carrier":
                    for (int j = 0; j < 5; j++)
                    {
                        y1 = player.internalBoard._ships[i].BoardPositions[j].XCoordinate;
                        x1 = player.internalBoard._ships[i].BoardPositions[j].YCoordinate;
                        player.displayBoard[x1 * 2, y1 * 2] = " Ca";
                    }
                    break;
            }

            PrintBoard(player.displayBoard);
            return player;
        }
    }
}
