namespace Game.Logic.Bot
{
    public class Minimax
    {
        public static int minimax(Board board, int depth, int alpha, int beta, char sideToMove)
        {
            string gameState = Game.CheckGameState(sideToMove, board);
            if (gameState != "null")
            {
                if (gameState.Contains("wins"))
                    return -Kenith.CHECKMATE_SCORE + (Kenith.MAX_DEPTH - depth);
                return Kenith.STALEMATE_SCORE;
            }

            if (depth == 0)
                return Eval.evaluatePosition(board, sideToMove);

            var allMoves = Game.GenerateAllLegalMoves(sideToMove, board);
            var orderedMoves = OrderMoves.orderMoves(allMoves, board);

            int maxScore = int.MinValue;
            bool prune = false;

            foreach (var move in orderedMoves)
            {
                if (!prune)
                {
                    Board tempBoard = board.Clone();
                    ApplyMove.applyMove(tempBoard, move);

                    char opponentSide = sideToMove == 'w' ? 'b' : 'w';
                    int score = -minimax(tempBoard, depth - 1, -beta, -alpha, opponentSide);

                    maxScore = Math.Max(maxScore, score);
                    alpha = Math.Max(alpha, score);

                    prune = alpha >= beta;
                }
            }

            return maxScore;
        }
    }
}
