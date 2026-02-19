namespace Game.Logic
{
    public class Bishop : Move
    {
        private bool isWhite;

        public Bishop(bool isWhite)
        {
            this.isWhite = isWhite;
        }

    public List<MoveInfo> GenerateLegalMoves(int[] board, int currentPos)
        {
            int[] directions = {MOVE_UP_RIGHT, MOVE_UP_LEFT, MOVE_DOWN_RIGHT, MOVE_DOWN_LEFT};
            return SlideMoves(board, currentPos, directions, isWhite);
        }
    }
}
