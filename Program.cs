using System;

namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialize square map (todo: user defined dimensions) and prepare for user input cells
            int maxDimension = 10;
            bool[,] map = new bool[maxDimension, maxDimension];
            string combinedInput = "";
            bool ready = false;
            // allow user to type in starting cells
            while (ready == false)
            {
                Console.WriteLine("Enter coordinates in format 'x, y' or 'done'");
                string userInput = Console.ReadLine();
                if (userInput == "done")
                {
                    ready = true;
                }
                else
                {
                    combinedInput = AddInput(userInput, combinedInput, maxDimension);
                }
            }
            // edge case for no cells
            if (combinedInput.Length == 0)
            {
                return;
            }
            // trim trailing semicolon
            combinedInput = combinedInput.Remove(combinedInput.Length - 1);
            // build map from input
            ParseInput(combinedInput, map);
            // show initial map
            ShowMap(map, maxDimension);
            // let's play the game!
            while (true)
            {
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("_____________________________");
                map = NextTurn(map, maxDimension);
                ShowMap(map, maxDimension);
            }
        }

        // add each user inputted cell
        public static string AddInput(string userInput, string combinedInput, int maxDimension)
        {
            // use formatted user input and if valid cell, add to combined i
            try
            {
                string[] inputArray = userInput.Split(", ");
                if (Int32.Parse(inputArray[0]) < maxDimension && Int32.Parse(inputArray[1]) < maxDimension)
                {
                    return combinedInput + userInput + ";";
                }
                return combinedInput;
            }
            catch
            {
                return combinedInput;
            }
        }

        // convert combined user input into true (alive) values on the map
        public static bool[,] ParseInput(string combinedInput, bool[,] map)
        {
            string[] input = combinedInput.Split(";");
            for (int i = 0; i < input.Length; i++)
            {
                string[] square = input[i].Split(", ");
                int x = Int32.Parse(square[0]);
                int y = Int32.Parse(square[1]);
                map[x, y] = true;
            }
            return map;
        }

        public static void ShowMap(bool[,] map, int maxDimension)
        {
            for (int i = 0; i < maxDimension; i++)
            {
                for (int j = 0; j < maxDimension; j++)
                {
                    if (map[i, j] == true)
                    {
                        Console.Write(string.Format("{0} ", "X"));
                    }
                    else
                    {
                        Console.Write(string.Format("{0} ", "_"));
                    }
                    
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }

        // given a map and cell, calculate number of alive neighbors
        public static int AliveNeighbors(int x, int y, int maxDimension, bool[,] map)
        {
            int count = 0;
            int xMinus = x - 1;
            int xPlus = x + 1;
            int yMinus = y - 1;
            int yPlus = y + 1;
            if (xMinus < 0)
            {
                xMinus = maxDimension - 1;
            }
            if (xPlus >= maxDimension)
            {
                xPlus = 0;
            }
            if (yMinus < 0)
            {
                yMinus = maxDimension - 1;
            }
            if (yPlus >= maxDimension)
            {
                yPlus = 0;
            }
            bool[] neighbors = {map[xMinus, yMinus], map[xMinus, y], map[xMinus, yPlus], map[x, yMinus], map[x, yPlus], map[xPlus, yMinus], map[xPlus, y], map[xPlus, yPlus]};
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] == true)
                {
                    count++;
                }
            }
            return count;
        }

        // given an alive cell and map, determine whether that cell will remain alive
        public static bool WillStayAlive(int x, int y, int maxDimension, bool[,] map)
        {
            int count = AliveNeighbors(x, y, maxDimension, map);
            return (count == 2 || count == 3);
        }

        // given an empty/dead cell and map, determine whether that cell will be alive next turn
        public static bool WillBecomeAlive(int x, int y, int maxDimension, bool[,] map)
        {
            int count = AliveNeighbors(x, y, maxDimension, map);
            return (count == 3);
        }

        // calculate this turn and whether cells will be dead/alive; produce and return new map
        public static bool[,] NextTurn(bool[,] map, int maxDimension)
        {
            bool[,] newMap = new bool[maxDimension, maxDimension];
            for (int i = 0; i < maxDimension; i++)
            {
                for (int j = 0; j < maxDimension; j++)
                {
                    if (map[i, j] == true)
                    {
                        newMap[i, j] = WillStayAlive(i, j, maxDimension, map);
                    }
                    else
                    {
                        newMap[i, j] = WillBecomeAlive(i, j, maxDimension, map);
                    }
                    
                }
            }
            return newMap;
        }
    }
}
