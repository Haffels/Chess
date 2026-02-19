namespace Game.Logic
{
    public class Move
    {
        protected const int MOVE_UP = 8;
        protected const int MOVE_DOWN = -8;
        protected const int MOVE_RIGHT = 1;
        protected const int MOVE_LEFT = -1;
        protected const int MOVE_UP_RIGHT = 9;
        protected const int MOVE_UP_LEFT = 7;
        protected const int MOVE_DOWN_RIGHT = -7;
        protected const int MOVE_DOWN_LEFT = -9;

        public enum MoveType
        {
            Normal,
            Capture,
            Castle,
            EnPassant,
            Promotion,
            DoubleMove,
            PromotionCapture
        }

        public class MoveInfo
        {
            public int from;
            public int to;
            public MoveType moveType;

            public MoveInfo(int from, int to, MoveType moveType)
            {
                this.from = from;
                this.to = to;
                this.moveType = moveType;
            }
        }

        private bool IsOpponent(int piece, bool isWhite)
        {
            return isWhite ? piece < 0 : piece > 0;
        }

        protected List<MoveInfo> SlideMoves(int[] board, int currentPos, int[] directions, bool isWhite)
        {
            var moves = new List<MoveInfo>();

            for (int d = 0; d < directions.Length; d++)
            {
                int dir = directions[d];
                int pos = currentPos;
                int next = pos + dir;
                
                bool blocked = false;

                while (!blocked && next >= 0 && next < 64 &&
                       Math.Abs((next % 8) - (pos % 8)) <= 1 &&
                       Math.Abs((next / 8) - (pos / 8)) <= 1)
                {
                    int piece = board[next];
                    if (piece == Pieces.NO_PIECE)
                    {
                        moves.Add(new MoveInfo(currentPos, next, MoveType.Normal));
                        pos = next;
                        next += dir;
                    }
                    else
                    {
                        if (IsOpponent(piece, isWhite))
                            moves.Add(new MoveInfo(currentPos, next, MoveType.Capture));
                        blocked = true;
                    }
                }
            }
            
            return moves;
        }
    }
}
