namespace Game.Logic
{
    public class Board
    {
        public int[] gameBoard;

        public Board()
        {
            gameBoard = new int[64];
        }

        public Board Clone()
        {
            var copy = new Board();
            copy.gameBoard = (int[])this.gameBoard.Clone();
            return copy;
        }

        public void PrintBoard(char userSide)
        {
            int startRow = userSide == 'w' ? 7 : 0, 
                step = userSide == 'w' ? -1 : 1;

            for (int i = 0; i < 8; i++)
            {
                int row = startRow + i * step;
                
                Console.Write($"{row + 1} ");
                
                for (int col = 0; col < 8; col++)
                    Console.Write($" {UnicodePieces.ToChar(gameBoard[row * 8 + col])} ");
                Console.WriteLine();
            }
            Console.WriteLine("   a  b  c  d  e  f  g  h");
        }
    }
}
