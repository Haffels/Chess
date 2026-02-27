namespace Game.Logic
{
    public class King : Move
    {
        private bool isKingWhite;
        private List<MoveInfo> legalMoves = new List<MoveInfo>();

        public King(bool isKingWhite)
        {
            this.isKingWhite = isKingWhite;
        }

        public List<MoveInfo> GenerateLegalMoves(int[] board, int currentPos)
        {
            legalMoves.Clear();
            int[] directions = {MOVE_DOWN_RIGHT, MOVE_DOWN_LEFT, MOVE_UP_RIGHT, MOVE_UP_LEFT, MOVE_UP, MOVE_DOWN, MOVE_RIGHT, MOVE_LEFT};

            int whiteKingStart = 4;
            int blackKingStart = 60;
            
            int currentRow = currentPos / 8;
            int currentCol = currentPos % 8;

            if (currentPos == (isKingWhite ? whiteKingStart : blackKingStart))
            {
                bool canCastleKingside = MakingMoves.CanCastleKingside(isKingWhite);
                bool canCastleQueenside = MakingMoves.CanCastleQueenside(isKingWhite);

                if (canCastleKingside)
                {
                    int kingSide1 = currentPos + MOVE_RIGHT;
                    int kingSide2 = currentPos + MOVE_RIGHT * 2;
                    int kingRookSquare = currentPos + MOVE_RIGHT * 3;

                    if (kingSide1 >= 0 && kingSide2 < 64 && kingRookSquare < 64)
                    {
                        if (board[kingSide1] == Pieces.NO_PIECE && board[kingSide2] == Pieces.NO_PIECE && Math.Abs(board[kingRookSquare]) == Pieces.ROOK)
                            legalMoves.Add(new MoveInfo(currentPos, kingSide2, MoveType.Castle));
                    }
                }

                if (canCastleQueenside)
                {
                    int queenSide1 = currentPos + MOVE_LEFT;
                    int queenSide2 = currentPos + MOVE_LEFT * 2;
                    int queenSide3 = currentPos + MOVE_LEFT * 3;
                    int queenRookSquare = currentPos + MOVE_LEFT * 4;

                    if (queenSide3 >= 0 && queenRookSquare >= 0)
                    {
                        if (board[queenSide1] == Pieces.NO_PIECE && board[queenSide2] == Pieces.NO_PIECE && board[queenSide3] == Pieces.NO_PIECE && Math.Abs(board[queenRookSquare]) == Pieces.ROOK)
                            legalMoves.Add(new MoveInfo(currentPos, queenSide2, MoveType.Castle));
                    }
                }
            }

            for (int i = 0; i < directions.Length; i++)
            {
                int pos = currentPos + directions[i];

                if (pos >= 0 && pos < 64)
                {
                    int newRow = pos / 8;
                    int newCol = pos % 8;

                    int differenceInRow = Math.Abs(newRow - currentRow);
                    int differenceInCol = Math.Abs(newCol - currentCol);
                    
                    if (differenceInRow <= 1 && differenceInCol <= 1 && (differenceInRow + differenceInCol) != 0)
                    {
                        int piece = board[pos];
                        bool isOpponent = isKingWhite ? piece < Pieces.NO_PIECE : piece > Pieces.NO_PIECE;

                        if (piece == Pieces.NO_PIECE)
                            legalMoves.Add(new MoveInfo(currentPos, pos, MoveType.Normal));
                        else if (isOpponent)
                            legalMoves.Add(new MoveInfo(currentPos, pos, MoveType.Capture));
                    }
                }
            }

            return legalMoves;
        }
    }
}
