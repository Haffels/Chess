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

            if (currentPos == (isKingWhite ? whiteKingStart : blackKingStart))
            {
                bool canCastleKingside = MakingMoves.CanCastleKingside(isKingWhite);
                bool canCastleQueenside = MakingMoves.CanCastleQueenside(isKingWhite);

                if (canCastleKingside)
                {
                    int kSide1 = currentPos + MOVE_RIGHT;
                    int kSide2 = currentPos + MOVE_RIGHT * 2;
                    int kRookSquare = currentPos + MOVE_RIGHT * 3;

                    if (kSide1 >= 0 && kSide2 < 64 && kRookSquare < 64)
                    {
                        if (board[kSide1] == Pieces.NO_PIECE && board[kSide2] == Pieces.NO_PIECE && Math.Abs(board[kRookSquare]) == Pieces.ROOK)
                            legalMoves.Add(new MoveInfo(currentPos, kSide2, MoveType.Castle));
                    }
                }

                if (canCastleQueenside)
                {
                    int qSide1 = currentPos + MOVE_LEFT;
                    int qSide2 = currentPos + MOVE_LEFT * 2;
                    int qSide3 = currentPos + MOVE_LEFT * 3;
                    int qRookSquare = currentPos + MOVE_LEFT * 4;

                    if (qSide3 >= 0 && qRookSquare >= 0)
                    {
                        if (board[qSide1] == Pieces.NO_PIECE && board[qSide2] == Pieces.NO_PIECE && board[qSide3] == Pieces.NO_PIECE && Math.Abs(board[qRookSquare]) == Pieces.ROOK)
                            legalMoves.Add(new MoveInfo(currentPos, qSide2, MoveType.Castle));
                    }
                }
            }

            for (int i = 0; i < directions.Length; i++)
            {
                int dir = directions[i];
                int pos = currentPos + dir;

                if (pos >= 0 && pos < 64)
                {
                    int currentRow = currentPos / 8;
                    int newRow = pos / 8;
                    bool wraps = Math.Abs(newRow - currentRow) > 1 && (dir == MOVE_RIGHT || dir == MOVE_LEFT);

                    if (!wraps)
                    {
                        int piece      = board[pos];
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
