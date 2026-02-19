namespace Game.Logic
{
    public class Queen : Move
    {
        private bool isWhite;

        public Queen(bool isWhite) { this.isWhite = isWhite; }

        public List<MoveInfo> GenerateLegalMoves(int[] board, int currentPos)
        {
            int[] directions = { MOVE_UP, MOVE_DOWN, MOVE_RIGHT, MOVE_LEFT, MOVE_UP_RIGHT, MOVE_UP_LEFT, MOVE_DOWN_RIGHT, MOVE_DOWN_LEFT };
            return SlideMoves(board, currentPos, directions, isWhite);
        }
    }
}
