namespace Game.Logic
{
    public class Pawn : Move
    {
        private bool isPawnWhite;
        private List<MoveInfo> legalMoves = new List<MoveInfo>();

        public Pawn(bool isPawnWhite) { this.isPawnWhite = isPawnWhite; }

        public List<MoveInfo> GenerateLegalMoves(int[] board, int currentPos)
        {
            legalMoves.Clear();

            int direction      = isPawnWhite ? MOVE_UP : MOVE_DOWN;
            int startRank      = isPawnWhite ? 1 : 6;
            int promotionRank  = isPawnWhite ? 7 : 0;
            int enPassantRank  = isPawnWhite ? 5 : 4;
            int currentFile    = currentPos % 8;
            int currentRank    = currentPos / 8;
            int diagonalLeft   = isPawnWhite ? MOVE_UP_LEFT  : MOVE_DOWN_LEFT;
            int diagonalRight  = isPawnWhite ? MOVE_UP_RIGHT : MOVE_DOWN_RIGHT;
            int captureLeft    = currentPos + diagonalLeft;
            int captureRight   = currentPos + diagonalRight;

            int forwardOne = currentPos + direction;
            if (forwardOne >= 0 && forwardOne < 64)
            {
                if (board[forwardOne] == Pieces.NO_PIECE)
                {
                    if (forwardOne / 8 == promotionRank)
                        legalMoves.Add(new MoveInfo(currentPos, forwardOne, MoveType.Promotion));
                    else
                        legalMoves.Add(new MoveInfo(currentPos, forwardOne, MoveType.Normal));
                }

                if (currentRank == startRank)
                {
                    int moveTwice = currentPos + direction * 2;
                    if (moveTwice >= 0 && moveTwice < 64 &&
                        board[forwardOne] == Pieces.NO_PIECE &&
                        board[moveTwice]  == Pieces.NO_PIECE)
                    {
                        legalMoves.Add(new MoveInfo(currentPos, moveTwice, MoveType.DoubleMove));
                    }
                }
            }

            if (currentRank == enPassantRank)
            {
                bool leftIsEnPassant  = isPawnWhite ? board[captureLeft]  == Pieces.BLACK * Pieces.EN_PASSANT_MARKER
                                                    : board[captureLeft]  == Pieces.EN_PASSANT_MARKER;
                bool rightIsEnPassant = isPawnWhite ? board[captureRight] == Pieces.BLACK * Pieces.EN_PASSANT_MARKER
                                                    : board[captureRight] == Pieces.EN_PASSANT_MARKER;

                if (currentFile > 0 && captureLeft >= 0 && captureLeft < 64 && leftIsEnPassant)
                    legalMoves.Add(new MoveInfo(currentPos, captureLeft, MoveType.EnPassant));

                if (currentFile < 7 && captureRight >= 0 && captureRight < 64 && rightIsEnPassant)
                    legalMoves.Add(new MoveInfo(currentPos, captureRight, MoveType.EnPassant));
            }

            bool leftIsOpponent = false;
            if (currentFile > 0 && captureLeft >= 0 && captureLeft < 64)
                leftIsOpponent = isPawnWhite ? board[captureLeft] < 0 : board[captureLeft] > 0;

            bool rightIsOpponent = false;
            if (currentFile < 7 && captureRight >= 0 && captureRight < 64)
                rightIsOpponent = isPawnWhite ? board[captureRight] < 0 : board[captureRight] > 0;


            if (currentFile > 0 && captureLeft >= 0 && captureLeft < 64 && leftIsOpponent)
            {
                if (captureLeft / 8 == promotionRank)
                    legalMoves.Add(new MoveInfo(currentPos, captureLeft, MoveType.PromotionCapture));
                else
                    legalMoves.Add(new MoveInfo(currentPos, captureLeft, MoveType.Capture));
            }

            if (currentFile < 7 && captureRight >= 0 && captureRight < 64 && rightIsOpponent)
            {
                if (captureRight / 8 == promotionRank)
                    legalMoves.Add(new MoveInfo(currentPos, captureRight, MoveType.PromotionCapture));
                else
                    legalMoves.Add(new MoveInfo(currentPos, captureRight, MoveType.Capture));
            }

            return legalMoves;
        }
    }
}
