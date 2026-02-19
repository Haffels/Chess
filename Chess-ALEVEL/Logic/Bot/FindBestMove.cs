namespace Game.Logic.Bot
{
    public class FindBestMove
    {
        public static Move.MoveInfo FindBestMoveNow(char sideToMove, Board board)
        {
            var allMoves = Game.GenerateAllLegalMoves(sideToMove, board);
            if (allMoves.Count == 0) return null;

            int searchDepth  = GetAdaptiveDepth.GetDepth(board, allMoves.Count);
            var orderedMoves = OrderMoves.OrderMovesList(allMoves, board);

            Move.MoveInfo bestMove = null;
            int bestScore = int.MinValue;
            int alpha     = int.MinValue;
            int beta      = int.MaxValue;

            for (int i = 0; i < orderedMoves.Count; i++)
            {
                Board tempBoard   = board.Clone();
                ApplyMove.ApplyMoveToBoard(tempBoard, orderedMoves[i]);
                char opponentSide = sideToMove == 'w' ? 'b' : 'w';
                int score = -Minimax.RunMinimax(tempBoard, searchDepth - 1, -beta, -alpha, opponentSide);

                if (score > bestScore) { bestScore = score; bestMove = orderedMoves[i]; }
                if (score > alpha)     alpha = score;
            }

            return bestMove;
        }
    }
}
