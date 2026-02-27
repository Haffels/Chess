namespace Game.Logic.Bot
{
    public class Minimax
    {
        public static int RunMinimax(Board board, int depth, int alpha, int beta, char sideToMove)
        {
            var allMoves = Game.GenerateAllLegalMoves(sideToMove, board);

            if (allMoves.Count == 0)
            {
                if (Game.IsKingInCheck(sideToMove, board))
                    return -Kenith.CHECKMATE_SCORE + (Kenith.MAX_DEPTH - depth);
                return Kenith.STALEMATE_SCORE;
            }

            if (depth == 0)
                return Eval.EvaluatePosition(board, sideToMove);

            var orderedMoves = OrderMoves.OrderMovesList(allMoves, board);
            
            int maxScore = int.MinValue;
            
            bool prune = false;

            for (int i = 0; i < orderedMoves.Count; i++)
            {
                if (!prune)
                {
                    Board tempBoard  = board.Clone();
                    ApplyMove.ApplyMoveToBoard(tempBoard, orderedMoves[i]);
                    
                    //print every move
                    // Console.Clear();
                    // tempBoard.PrintBoard(sideToMove);
                    
                    char opponentSide = sideToMove == 'w' ? 'b' : 'w';
                    int score = -RunMinimax(tempBoard, depth - 1, -beta, -alpha, opponentSide);

                    if (score > maxScore) 
                        maxScore = score;
                    if (score > alpha)    
                        alpha = score;
                    
                    prune = (alpha >= beta) ? true : false;
                }
            }
            return maxScore;
        }
    }
}