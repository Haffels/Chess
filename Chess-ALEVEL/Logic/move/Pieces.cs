namespace Game.Logic
{
    public static class Pieces
    {
        public const int NO_PIECE = 0;
        public const int PAWN = 1;
        public const int KNIGHT = 2;
        public const int BISHOP = 3;
        public const int QUEEN = 4;
        public const int ROOK = 5;
        public const int KING = 6;

        public const int EN_PASSANT_MARKER = 10;

        public const int WHITE = 1;
        public const int BLACK = -1;
    }

    public static class UnicodePieces
    {
        public static char ToChar(int piece)
        {
            return piece switch
            {
                Pieces.BLACK * Pieces.PAWN   => '♙',
                Pieces.BLACK * Pieces.KNIGHT => '♘',
                Pieces.BLACK * Pieces.BISHOP => '♗',
                Pieces.BLACK * Pieces.ROOK   => '♖',
                Pieces.BLACK * Pieces.QUEEN  => '♕',
                Pieces.BLACK * Pieces.KING   => '♔',
                Pieces.PAWN   => '♟',
                Pieces.KNIGHT => '♞',
                Pieces.BISHOP => '♝',
                Pieces.ROOK   => '♜',
                Pieces.QUEEN  => '♛',
                Pieces.KING   => '♚',
                _ => '•'
            };
        }
    }

    public static class PieceHelpers
    {
        public static bool IsWhite(int piece) => piece > 0;
        public static bool IsBlack(int piece) => piece < 0;
    }
}
