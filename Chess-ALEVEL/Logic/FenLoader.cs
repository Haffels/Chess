namespace Game.Logic
{
    public static class FenLoader
    {
        public static void ReadFenAndLoad(string fen, Board board)
        {
            var pieceTypeForFen = new Dictionary<char, int>()
            {
                ['k'] = Pieces.KING,  ['q'] = Pieces.QUEEN,  ['b'] = Pieces.BISHOP,
                ['r'] = Pieces.ROOK,  ['n'] = Pieces.KNIGHT, ['p'] = Pieces.PAWN
            };

            string fenBoard = fen.Split(' ')[0];
            int file = 0, 
                rank = 7;

            for (int i = 0; i < fenBoard.Length; i++)
            {
                char ch = fenBoard[i];
                if (ch == '/')
                {
                    file = Pieces.NO_PIECE;
                    rank--;
                }
                else if (char.IsDigit(ch))
                {
                    file += (int)char.GetNumericValue(ch);
                }
                else
                {
                    int colour = char.IsUpper(ch) ? Pieces.WHITE : Pieces.BLACK;
                    int piece = pieceTypeForFen[char.ToLower(ch)];
                    
                    board.gameBoard[rank * 8 + file] = piece * colour;
                    file++;
                }
            }
        }
    }
}
