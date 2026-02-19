namespace Game.Logic
{
    public static class MovePieces
    {
        public static List<Move.MoveInfo> GetLegalMoves(int[] board, int position)
        {
            int piece = board[position];
            bool isWhite = piece > 0;

            return Math.Abs(piece) switch
            {
                Pieces.PAWN   => new Pawn(isWhite).GenerateLegalMoves(board, position),
                Pieces.ROOK   => new Rook(isWhite).GenerateLegalMoves(board, position),
                Pieces.KNIGHT => new Knight(isWhite).GenerateLegalMoves(board, position),
                Pieces.BISHOP => new Bishop(isWhite).GenerateLegalMoves(board, position),
                Pieces.QUEEN  => new Queen(isWhite).GenerateLegalMoves(board, position),
                Pieces.KING   => new King(isWhite).GenerateLegalMoves(board, position),
                _ => new List<Move.MoveInfo>()
            };
        }
    }
}
