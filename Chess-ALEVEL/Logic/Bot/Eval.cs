namespace Game.Logic.Bot
{
    public class Eval
    {
        public static int evaluatePosition(Board board, char sideToMove)
        {
            int score = 0;

            for (int i = 0; i < 64; i++)
            {
                int piece = board.gameBoard[i];
                bool isEmpty = piece == Pieces.noPiece || Math.Abs(piece) == Pieces.enPassantMarker;

                if (!isEmpty)
                {
                    int pieceType = Math.Abs(piece);
                    bool isWhite = piece > 0;

                    int pieceValue = Kenith.pieceValues.ContainsKey(pieceType) ? Kenith.pieceValues[pieceType] : 0;
                    int positionBonus = getPositionBonus(pieceType, i, isWhite);
                    int totalValue = pieceValue + positionBonus;

                    if (isWhite)
                        score += totalValue;
                    else
                        score -= totalValue;
                }
            }

            return sideToMove == 'w' ? score : -score;
        }

        private static int getPositionBonus(int pieceType, int position, bool isWhite)
        {
            int index = isWhite ? position : (63 - position);

            switch (pieceType)
            {
                case Pieces.pawn:
                    return Kenith.pawnTable[index];
                case Pieces.knight:
                    return Kenith.knightTable[index];
                default:
                    return 0;
            }
        }
    }
}
