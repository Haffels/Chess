namespace Game.Logic
{
    public class Knight : Move
    {
        private bool isKnightWhite;
        private List<MoveInfo> legalMoves = new List<MoveInfo>();

        public Knight(bool isKnightWhite) { this.isKnightWhite = isKnightWhite; }

        public List<MoveInfo> GenerateLegalMoves(int[] board, int currentPos)
        {
            legalMoves.Clear();
            int[] directions = { -17, -15, -10, -6, 6, 10, 15, 17 };

            int currentRow = currentPos / 8;
            int currentCol = currentPos % 8;

            for (int i = 0; i < directions.Length; i++)
            {
                int pos = currentPos + directions[i];

                if (pos >= 0 && pos < 64)
                {
                    int newRow = pos / 8;
                    int newCol = pos % 8;
                    int dRow = Math.Abs(newRow - currentRow);
                    int dCol = Math.Abs(newCol - currentCol);

                    if ((dRow == 2 && dCol == 1) || (dRow == 1 && dCol == 2))
                    {
                        int piece = board[pos];
                        bool isOpponent = isKnightWhite ? piece < Pieces.NO_PIECE : piece > Pieces.NO_PIECE;

                        if (piece == Pieces.NO_PIECE)
                            legalMoves.Add(new MoveInfo(currentPos, pos, MoveType.Normal));
                        else if (isOpponent)
                            legalMoves.Add(new MoveInfo(currentPos, pos, MoveType.Capture));
                    }
                }
            }

            return legalMoves;
        }
    }
}
