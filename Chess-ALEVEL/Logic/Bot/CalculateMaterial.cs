namespace Game.Logic.Bot
{
    public class CalculateMaterial
    {
        public static int GetTotalMaterial(Board board)
        {
            int total = 0;

            for (int i = 0; i < 64; i++)
            {
                int piece = board.gameBoard[i];

                if (piece != Pieces.NO_PIECE && Math.Abs(piece) != Pieces.EN_PASSANT_MARKER)
                {
                    int pieceType = Math.Abs(piece);

                    if (Kenith.pieceValues.ContainsKey(pieceType) && pieceType != Pieces.KING)
                        total += Kenith.pieceValues[pieceType];
                }
            }
            
            return total;
        }
    }
}