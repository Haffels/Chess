namespace Game.Logic.Bot
{
    public class ApplyMove
    {
        public static void ApplyMoveToBoard(Board board, Move.MoveInfo move)
        {
            int movingPiece = board.gameBoard[move.from];

            for (int i = 0; i < 64; i++)
            {
                if (Math.Abs(board.gameBoard[i]) == Pieces.EN_PASSANT_MARKER)
                    board.gameBoard[i] = Pieces.NO_PIECE;
            }

            if (move.moveType == Move.MoveType.Castle)
            {
                bool kingSide = move.to > move.from;
                
                board.gameBoard[move.to] = movingPiece;
                board.gameBoard[move.from] = Pieces.NO_PIECE;
                
                int rookFrom = kingSide ? move.from + 3 : move.from - 4;
                int rookTo = kingSide ? move.from + 1 : move.from - 1;
                
                board.gameBoard[rookTo] = board.gameBoard[rookFrom];
                board.gameBoard[rookFrom] = Pieces.NO_PIECE;
                
                return;
            }

            if (move.moveType == Move.MoveType.EnPassant)
            {
                int capturedPawnSquare = PieceHelpers.IsWhite(movingPiece) ? move.to - 8 : move.to + 8;
                
                board.gameBoard[capturedPawnSquare] = Pieces.NO_PIECE;
            }

            if (move.moveType == Move.MoveType.Promotion || move.moveType == Move.MoveType.PromotionCapture)
            {
                int colour = PieceHelpers.IsWhite(movingPiece) ? Pieces.WHITE : Pieces.BLACK;
                
                board.gameBoard[move.to] = Pieces.QUEEN * colour;
                board.gameBoard[move.from] = Pieces.NO_PIECE;
                return;
            }

            board.gameBoard[move.to] = movingPiece;
            board.gameBoard[move.from] = Pieces.NO_PIECE;

            if (move.moveType == Move.MoveType.DoubleMove)
            {
                int enPassantSquare = PieceHelpers.IsWhite(movingPiece) ? move.to - 8 : move.to + 8;
                
                board.gameBoard[enPassantSquare] = PieceHelpers.IsWhite(movingPiece) ? Pieces.EN_PASSANT_MARKER : Pieces.BLACK * Pieces.EN_PASSANT_MARKER;
            }
        }
    }
}
