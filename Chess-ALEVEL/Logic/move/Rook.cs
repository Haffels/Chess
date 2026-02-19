namespace Game.Logic
{
    public class Rook : Move
    {
        private bool isWhite;

        public Rook(bool isWhite) { this.isWhite = isWhite; }

        public List<MoveInfo> GenerateLegalMoves(int[] board, int currentPos)
        {
            int[] directions = { MOVE_UP, MOVE_DOWN, MOVE_RIGHT, MOVE_LEFT };
            return SlideMoves(board, currentPos, directions, isWhite);
        }
    }
}
