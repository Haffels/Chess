namespace Game.Logic
{
    public class MainGame
    {
        public static char userSide = ' ';
        public static string userGameMode;

        public static void Main()
        {
            string startFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            // string startFen = "7k/5Q2/6K1/8/8/8/8/8 w - - 0 1";
            Board newBoard = new Board();
            FenLoader.ReadFenAndLoad(startFen, newBoard);

            Console.WriteLine("Pick game mode:\n1: Player vs Bot\n2: Player vs Player\n3: Bot vs Bot");
            userGameMode = Console.ReadLine();

            Console.WriteLine("Pick a side w or b: ");
            userSide = Console.ReadKey().KeyChar;
            Console.WriteLine();

            Console.WriteLine("Use timer? (y/n): ");
            char useTimerChoice = Console.ReadKey().KeyChar;
            Console.WriteLine();

            bool useTimer = useTimerChoice == 'y' || useTimerChoice == 'Y';
            int timePerSide = 600;

            if (useTimer)
            {
                Console.WriteLine("Enter time per side in minutes (default 10): ");
                string timeInput = Console.ReadLine();
                int minutes = Convert.ToInt32(timeInput);
                if (minutes > 0)
                    timePerSide = minutes * 60;
                else
                    Console.WriteLine("invalid");
            }

            MakingMoves.CreateTimer(useTimer, timePerSide);

            while (true)
            {
                Console.Clear();
                newBoard.PrintBoard(userSide);
                MakingMoves.HandleMoves(newBoard);
            }
        }
    }
}
