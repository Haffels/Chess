namespace Game.Logic.Bot
{
    public class OrderMoves
    {
        public static List<Move.MoveInfo> OrderMovesList(List<Move.MoveInfo> moves, Board board)
        {
            int[] scores = new int[moves.Count];

            for (int i = 0; i < moves.Count; i++)
            {
                int score = 0;
                Move.MoveInfo m = moves[i];

                if (m.moveType == Move.MoveType.Castle)
                    score += 700;
                
                if (m.moveType == Move.MoveType.Promotion || m.moveType == Move.MoveType.PromotionCapture)
                    score += 800;

                if (m.to == 27 || m.to == 28 || m.to == 35 || m.to == 36)
                    score += 20;

                if (m.moveType == Move.MoveType.Capture || m.moveType == Move.MoveType.PromotionCapture)  
                {
                    int capturedPiece = Math.Abs(board.gameBoard[m.to]);
                    int attackingPiece = Math.Abs(board.gameBoard[m.from]);
                    int victimValue = Kenith.pieceValues.ContainsKey(capturedPiece) ? Kenith.pieceValues[capturedPiece]  : 0;
                    int attackerValue = Kenith.pieceValues.ContainsKey(attackingPiece) ? Kenith.pieceValues[attackingPiece] : 0;
                    
                    score += victimValue * 10 - attackerValue;
                }
                scores[i] = score;
            }
            
            //BUUBLE sort
            for (int i = 0; i < moves.Count - 1; i++)
            {
                for (int j = 0; j < moves.Count - 1 - i; j++)
                {
                    if (scores[j] < scores[j + 1])
                    {
                        int tempScore;
                        Move.MoveInfo tempMove;
                        
                        tempScore = scores[j];
                        scores[j] = scores[j + 1];
                        scores[j + 1] = tempScore;

                        tempMove = moves[j];
                        moves[j] = moves[j + 1];
                        moves[j + 1] = tempMove;
                    }
                }
            }
            return moves;
        }
    }
}
