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
    class Program
    {
        string player1 = "";
        string player2 = "";
        string c1p1 = "";
        string c1p1Upper = "";
        string c1p1FirstCoord = "";
        string c1p1SecondCoord = "";
        int c1p1FirstCoordInt;
        int c1p1SecondCoordInt;
        string c1p1FinalCoord = "";
        Coordinate coordinate;
        string c1p1ShipDirection = "";
        string c1p1ShipType = "";
        ShipDirection shipDirection;
        Ship c1p1ShipTypeFinal;
        ShipType shipType;
        bool valid = false;
        char[] c1p1PlaceShipArray = new char[2];
        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
        Board boardPlayer1 = new Board();
        Board boardPlayer2 = new Board();
        Board board = new Board();
        PlaceShipRequest placeShipRequestP1 = new PlaceShipRequest();

        static void Main(string[] args)
        {
            Program program = new Program();

            string[,] c1p1Board = program.battleshipBoard;
            string[,] c2p2Board = program.battleshipBoard;

            // Program flow starts here
            // Welcome message and get player 1 and player 2's names
            program.player1 = program.GetPlayerOneName();
            program.player2 = program.GetPlayerTwoName();

            Console.WriteLine("Excellent!  Press enter to get started!");
            Console.Clear();
            //These methods setup the boards for each player and they contain multiple other methods
            program.SetupBoards(program.boardPlayer1, program.player1, c1p1Board);
            program.SetupBoards(program.boardPlayer2, program.player2, c2p2Board);

            Console.Clear();
            Console.WriteLine("Ready to Play? Press Enter and let the battle BEGIN!");
            Console.ReadLine();
            Console.Clear();
            Console.ReadLine();       
        }

        public string GetPlayerOneName()
        {

            valid = false;
            while (!valid)
            {
                Console.WriteLine("Welcome to BATTLESHIP!!");
                Console.WriteLine("\nEnter Player 1's name: ");
                player1 = Console.ReadLine();
                Console.Clear();
                if (player1 == "")
                {
                    Console.WriteLine("You must type a name. Try again.");
                }
                else
                    valid = true;
            }
            Console.Clear();
            return player1;
        }

        public string GetPlayerTwoName()
        {
            valid = false;
            while (!valid)
            {
                Console.WriteLine("Welcome to BATTLESHIP!!");
                Console.WriteLine("\nNow enter Player 2's name: ");
                player2 = Console.ReadLine();
                Console.Clear();
                if (player2 == "")
                {
                    Console.WriteLine("You must type a name. Try again.");
                }
                else
                    valid = true;
            }
            Console.Clear();
            return player2;
        }

        // This is the base board that gets assigned to player 1 and player 2
        public string[,] battleshipBoard = new string[22, 22]
        {
            { "   ", "   ", " A ", "   ", " B ", "   ", " C ", "   ", " D ", "   ", " E ", "   ", " F ", "   ", " G ", "   ", " H ", "   ", " I ", "   ", " J ", "   "},
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

        public void PrintBoard(string[,] playerBoard)
        {
            // Prints board to the screen and is player-specific
            for (int j = 0; j < 22; j++)
            {
                for (int k = 0; k < 22; k++)
                {
                    Console.Write(playerBoard[j, k]);
                }
                Console.Write("\n");
            }
        }

        public void SetupBoards(Board boardPlayer, string player, string[,] playerBoard)
        {
            // Gets and preps ship locations from user
            for (int i = 0; i < 5; i++)
            {
                valid = false;
                while (!valid)
                {
                    PrintBoard(playerBoard);
                    Console.WriteLine("\n{0}, place ship {1}. ", player, i + 1);
                    // This method retrieves coordinates from the player
                    coordinate = GetCoordinate();
                    // These two methods clear the console and then display a new battleship board
                    Console.Clear();
                    PrintBoard(playerBoard);

                    // This method retrieves the direction the ship should be placed from the player
                    shipDirection = GetShipDirection();
                    Console.Clear();
                    PrintBoard(playerBoard);

                    // This method retrieves the type of ship from the player
                    shipType = GetShipType();

                    // Next we input the 3 parameters specified by the player into the PlaceShipRequest method
                    PlaceShipRequest c1p1PlaceShipRequest = new PlaceShipRequest()
                    {
                        Coordinate = coordinate,
                        Direction = shipDirection,
                        ShipType = shipType
                    };

                    // The ShipPlacement method then attempts to place ship onto board                    
                    ShipPlacement value = boardPlayer.PlaceShip(c1p1PlaceShipRequest);
                    if (value == ShipPlacement.NotEnoughSpace)
                    {
                        Console.WriteLine("Not enough space to place ship. Press enter and try again.");
                        Console.ReadLine();
                        valid = false;
                    }
                    else if (value == ShipPlacement.Overlap)
                    {
                        Console.WriteLine("This ship overlaps another. Press enter and try again.");
                        Console.ReadLine();
                        valid = false;
                    }
                    else
                        valid = true;
                }
            }             
        }

        public Coordinate GetCoordinate()
        {
            valid = false;
            while (valid == false)
            {
                Console.WriteLine("Enter coordinate (ex: B6): ");
                c1p1 = Console.ReadLine();
                c1p1Upper = c1p1.ToUpper();
                c1p1PlaceShipArray = c1p1Upper.ToCharArray();
                c1p1FirstCoord = c1p1PlaceShipArray[0].ToString();
                c1p1SecondCoord = c1p1PlaceShipArray[1].ToString();
                // think through whether these these if statements could be shortened somehow
                if (c1p1FirstCoord == "A")
                {
                    c1p1FirstCoord = "1";
                    valid = true;
                }
                else if (c1p1FirstCoord == "B")
                {
                    c1p1FirstCoord = "2";
                    valid = true;
                }
                else if (c1p1FirstCoord == "C")
                {
                    c1p1FirstCoord = "3";
                    valid = true;
                }
                else if (c1p1FirstCoord == "D")
                {
                    c1p1FirstCoord = "4";
                    valid = true;
                }
                else if (c1p1FirstCoord == "E")
                {
                    c1p1FirstCoord = "5";
                    valid = true;
                }
                else if (c1p1FirstCoord == "F")
                {
                    c1p1FirstCoord = "6";
                    valid = true;
                }
                else if (c1p1FirstCoord == "G")
                {
                    c1p1FirstCoord = "7";
                    valid = true;
                }
                else if (c1p1FirstCoord == "H")
                {
                    c1p1FirstCoord = "8";
                    valid = true;
                }
                else if (c1p1FirstCoord == "I")
                {
                    c1p1FirstCoord = "9";
                    valid = true;
                }
                else if (c1p1FirstCoord == "J")
                {
                    c1p1FirstCoord = "10";
                    valid = true;
                }
                else
                    Console.WriteLine("Not a valid coordinate. Try again.");
                // This could probably be shortened by using a for loop
                if (!(c1p1SecondCoord == "1" 
                    || c1p1SecondCoord == "2"
                    || c1p1SecondCoord == "3"
                    || c1p1SecondCoord == "4"
                    || c1p1SecondCoord == "5"
                    || c1p1SecondCoord == "6"
                    || c1p1SecondCoord == "7"
                    || c1p1SecondCoord == "8"
                    || c1p1SecondCoord == "9"
                    || c1p1SecondCoord == "10"))
                {
                    Console.WriteLine("Not a valid coordinate. Try again.");
                    valid = false;
                }
            }
            c1p1FirstCoordInt = Int32.Parse(c1p1FirstCoord);
            c1p1SecondCoordInt = Int32.Parse(c1p1SecondCoord);

            // Runs the converted user coordinate through the Coordinate method to get output necessary for
            // PlaceShipRequest Class
            Coordinate coordinate = new Coordinate(c1p1FirstCoordInt, c1p1SecondCoordInt);
            return coordinate;
            // Coordinate gets validated in the Board Class by the IsValidCoordinate method         
        }

        public ShipDirection GetShipDirection()
        {
            valid = false;
            while (valid == false)
            {
                // Gets and preps ship direction from user
                Console.WriteLine("\nEnter ship direction (Up, Down, Left, or Right)");
                c1p1ShipDirection = Console.ReadLine();
                c1p1ShipDirection = myTI.ToTitleCase(c1p1ShipDirection);
                if (!(c1p1ShipDirection == "Up"
                    || c1p1ShipDirection == "Down"
                    || c1p1ShipDirection == "Left"
                    || c1p1ShipDirection == "Right"))
                {
                    Console.WriteLine("Not a valid direction. Try again.");
                }
                else
                    valid = true;          
            }
            ShipDirection shipDirection = (ShipDirection)Enum.Parse(typeof(ShipDirection), c1p1ShipDirection);
            return shipDirection;
        }

        public ShipType GetShipType()
        {
            List<string> shipsAlreadyPlaced = new List<string>();
            // Gets and preps ship type from user
            valid = false;
            bool valid2 = false;

            while (valid == false)
            {
                while (valid2 == false)
                {
                    Console.WriteLine("\nEnter ship type.  Choices are: ");
                    Console.WriteLine("\nDestroyer\nCruiser\nSubmarine\nBattleship\nCarrier\n");
                    c1p1ShipType = Console.ReadLine();
                    c1p1ShipType = myTI.ToTitleCase(c1p1ShipType);

                    if (!(c1p1ShipType == "Destroyer"
                        || c1p1ShipType == "Cruiser"
                        || c1p1ShipType == "Submarine"
                        || c1p1ShipType == "Battleship"
                        || c1p1ShipType == "Carrier"))
                    {
                        Console.WriteLine("Not a valid ship type. Try again.");
                        valid2 = false;
                    }
                    else
                        valid2 = true;
                }   
                                   
                shipsAlreadyPlaced.Add(c1p1ShipType);
                valid = true;
            }

            // Use the ShipType enum here to prep the entry into proper PlaceShipRequest Class format
            ShipType shipType = (ShipType)Enum.Parse(typeof(ShipType), c1p1ShipType);
            c1p1ShipTypeFinal = ShipCreator.CreateShip(shipType); //NOT SURE I NEED THIS HERE!!  So far I haven't used it.
            return shipType;
        }
    }
}
