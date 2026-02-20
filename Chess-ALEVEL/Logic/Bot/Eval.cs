namespace Game.Logic.Bot
{
    public class Eval
    {
        public static int EvaluatePosition(Board board, char sideToMove)
        {
            int score = 0, whitePieceCount = 0, blackPieceCount = 0;

            for (int i = 0; i < 64; i++)
            {
                int piece = board.gameBoard[i];

                if (piece != Pieces.NO_PIECE && Math.Abs(piece) != Pieces.EN_PASSANT_MARKER)
                {
                    int pieceType = Math.Abs(piece);
                    int pieceValue = Kenith.pieceValues.ContainsKey(pieceType) ? Kenith.pieceValues[pieceType] : 0;
                    int positionBonus = GetPositionBonus(pieceType, i, piece > 0);
                    int totalValue = pieceValue + positionBonus;

                    if (piece > 0)
                    {
                        score += totalValue;
                        whitePieceCount++;
                    }
                    else
                    {
                        score -= totalValue;
                        blackPieceCount++;
                    }
                }
            }

            int totalPieces = whitePieceCount + blackPieceCount;
            
            if (totalPieces > 24)
                score += EvaluateDevelopment(board);

            score += EvaluateKingSafety(board, 'w') - EvaluateKingSafety(board, 'b');
            score += EvaluatePawnStructure(board);
            score += EvaluateCastling(board);
            
            return sideToMove == 'w' ? score : -score;
        }

        public static int EvaluateCastling(Board board)
        {
            int score = 0;
            if (board.gameBoard[6] == Pieces.KING)
                score += 70;
            else if (board.gameBoard[2] == Pieces.KING)
                score += 60;
            if (board.gameBoard[62] == -Pieces.KING)
                score -= 70;
            else if (board.gameBoard[58] == -Pieces.KING)
                score -= 60;
            return score;
        }

        public static int EvaluateDevelopment(Board board)
        {
            int score = 0;
            if (board.gameBoard[1] == Pieces.KNIGHT)
                score -= 20;
            if (board.gameBoard[6] == Pieces.KNIGHT)
                score -= 20;
            if (board.gameBoard[57] == -Pieces.KNIGHT)
                score += 20;
            if (board.gameBoard[62] == -Pieces.KNIGHT)
                score += 20;
            if (board.gameBoard[2] == Pieces.BISHOP)
                score -= 15;
            if (board.gameBoard[5] == Pieces.BISHOP)
                score -= 15;
            if (board.gameBoard[58] == -Pieces.BISHOP)
                score += 15;
            if (board.gameBoard[61] == -Pieces.BISHOP)
                score += 15;
            return score;
        }

        public static int EvaluateKingSafety(Board board, char side)
        {
            int kingPiece = side == 'w' ? Pieces.KING : -Pieces.KING;
            int kingPos = Array.IndexOf(board.gameBoard, kingPiece);
            if (kingPos == -1)
                return -10000;

            int safety = 0;
            int[] adjacentSquares = {-9, -8, -7, -1, 1, 7, 8, 9};
            
            for (int i = 0; i < adjacentSquares.Length; i++)
            {
                int pos = kingPos + adjacentSquares[i];
                if (pos >= 0 && pos < 64)
                {
                    int piece = board.gameBoard[pos];
                    if (side == 'w' && piece > 0 && piece != Pieces.EN_PASSANT_MARKER)  safety += 5;
                    else if (side == 'b' && piece < 0 && piece != -Pieces.EN_PASSANT_MARKER) safety += 5;
                }
            }
            return safety;
        }

        public static int EvaluatePawnStructure(Board board)
        {
            int score = 0;
            for (int file = 0; file < 8; file++)
            {
                int whitePawns = 0;
                int blackPawns = 0;
                for (int rank = 0; rank < 8; rank++)
                {
                    int pos = rank * 8 + file;
                    if (board.gameBoard[pos] == Pieces.PAWN)
                        whitePawns++;
                    if (board.gameBoard[pos] == -Pieces.PAWN)
                        blackPawns++;
                }
                if (whitePawns > 1)
                    score -= (whitePawns - 1) * 15;
                if (blackPawns > 1)
                    score += (blackPawns - 1) * 15;
            }
            return score;
        }

        public static int GetPositionBonus(int pieceType, int position, bool isWhite)
        {
            int index = isWhite ? position : (63 - position);
            switch (pieceType)
            {
                case Pieces.PAWN:
                    return Kenith.pawnTable[index];
                case Pieces.KNIGHT:
                    return Kenith.knightTable[index];
                case Pieces.BISHOP:
                    return Kenith.bishopTable[index];
                case Pieces.ROOK:
                    return Kenith.rookTable[index];
                case Pieces.KING:
                    return Kenith.kingMiddleGameTable[index];
                default: 
                    return 0;
            }
        }
    }
}
