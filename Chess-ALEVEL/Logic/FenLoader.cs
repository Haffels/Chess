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
                char newRank = fenBoard[i];
                if (newRank == '/')
                {
                    file = Pieces.NO_PIECE;
                    rank--;
                }
                else if (char.IsDigit(newRank))
                {
                    file += (int)char.GetNumericValue(newRank);
                }
                else
                {
                    int colour = char.IsUpper(newRank) ? Pieces.WHITE : Pieces.BLACK;
                    int piece = pieceTypeForFen[char.ToLower(newRank)];
                    
                    board.gameBoard[rank * 8 + file] = piece * colour;
                    file++;
                }
            }
        }
    }
}
