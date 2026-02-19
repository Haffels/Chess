namespace Game.Logic
{
    public class Game
    {
        private static List<string> positionHistory = new List<string>();
        private static int halfMoveClock;

        public static void ResetGameHistory()
        {
            positionHistory.Clear();
            halfMoveClock = 0;
        }

        public static void RecordMove(Board board, Move.MoveInfo move, bool isPawnMove, bool isCapture)
        {
            if (isPawnMove || isCapture) halfMoveClock = 0;
            else halfMoveClock++;
            positionHistory.Add(BoardToString(board));
        }

        private static string BoardToString(Board board)
        {
            string result = "";
            for (int i = 0; i < board.gameBoard.Length; i++)
            {
                if (i > 0) result += ",";
                result += board.gameBoard[i].ToString();
            }
            return result;
        }

        public static string CheckGameState(char sideToMove, Board board)
        {
            string winner = "null";
            bool kingInCheck = IsKingInCheck(sideToMove, board);
            var moves = GenerateAllLegalMoves(sideToMove, board);

            if (moves.Count == 0)
                winner = kingInCheck ? (sideToMove == 'w' ? "black wins by checkmate" : "white wins by checkmate") : "draw by stalemate";
            else if (CheckFiftyMoveRule())
                winner = "draw by fifty move rule";
            else if (CheckThreeFoldRepetition())
                winner = "draw by threefold repetition";
            else if (CheckInsufficientMaterial(board))
                winner = "draw by insufficient material";

            return winner;
        }

        public static List<Move.MoveInfo> GenerateAllLegalMoves(char sideToMove, Board board)
        {
            var allMoves = new List<Move.MoveInfo>();

            for (int i = 0; i < board.gameBoard.Length; i++)
            {
                int piece = board.gameBoard[i];
                if (piece == Pieces.NO_PIECE) continue;

                bool isWhitePiece = piece > 0;
                bool isOwnPiece = (sideToMove == 'w' && isWhitePiece) || (sideToMove == 'b' && !isWhitePiece);

                if (isOwnPiece)
                {
                    var pieceMoves = GetLegalMovesForPiece(sideToMove, board, i);
                    for (int j = 0; j < pieceMoves.Count; j++)
                        allMoves.Add(pieceMoves[j]);
                }
            }

            return allMoves;
        }

        public static bool IsKingInCheck(char sideToMove, Board board)
        {
            int kingPiece = sideToMove == 'w' ? Pieces.WHITE * Pieces.KING : Pieces.BLACK * Pieces.KING;
            int kingPosition = Array.IndexOf(board.gameBoard, kingPiece);
            if (kingPosition == -1) return true;

            char opponentSide = sideToMove == 'w' ? 'b' : 'w';

            for (int i = 0; i < board.gameBoard.Length; i++)
            {
                int piece = board.gameBoard[i];
                if (piece == Pieces.NO_PIECE) continue;

                bool isWhitePiece = piece > 0;
                bool isOpponentPiece = (opponentSide == 'w' && isWhitePiece) || (opponentSide == 'b' && !isWhitePiece);

                if (isOpponentPiece)
                {
                    var pseudoMoves = MovePieces.GetLegalMoves(board.gameBoard, i);
                    for (int j = 0; j < pseudoMoves.Count; j++)
                    {
                        if (pseudoMoves[j].to == kingPosition)
                            return true;
                    }
                }
            }

            return false;
        }

        public static bool WillKingBeInCheck(char sideToMove, Board board, Move.MoveInfo move)
        {
            Board tempBoard = board.Clone();
            tempBoard.gameBoard[move.to] = tempBoard.gameBoard[move.from];
            tempBoard.gameBoard[move.from] = Pieces.NO_PIECE;
            return IsKingInCheck(sideToMove, tempBoard);
        }

        public static List<Move.MoveInfo> GetLegalMovesForPiece(char sideToMove, Board board, int pieceIndex)
        {
            var moves = MovePieces.GetLegalMoves(board.gameBoard, pieceIndex);
            var legalMoves = new List<Move.MoveInfo>();

            for (int i = 0; i < moves.Count; i++)
            {
                var move = moves[i];
                if (move.moveType == Move.MoveType.Castle)
                {
                    if (IsKingInCheck(sideToMove, board)) continue;

                    int passThroughSquare = move.to > move.from ? move.from + 1 : move.from - 1;

                    Board tempBoard1 = board.Clone();
                    tempBoard1.gameBoard[passThroughSquare] = tempBoard1.gameBoard[move.from];
                    tempBoard1.gameBoard[move.from] = Pieces.NO_PIECE;

                    Board tempBoard2 = board.Clone();
                    tempBoard2.gameBoard[move.to] = tempBoard2.gameBoard[move.from];
                    tempBoard2.gameBoard[move.from] = Pieces.NO_PIECE;

                    if (!IsKingInCheck(sideToMove, tempBoard1) && !IsKingInCheck(sideToMove, tempBoard2))
                        legalMoves.Add(move);
                }
                else
                {
                    if (!WillKingBeInCheck(sideToMove, board, move))
                        legalMoves.Add(move);
                }
            }

            return legalMoves;
        }

        public static bool CheckFiftyMoveRule() => halfMoveClock >= 100;

        public static bool CheckThreeFoldRepetition()
        {
            if (positionHistory.Count < 3) return false;
            string current = positionHistory[positionHistory.Count - 1];
            int count = 0;
            for (int i = 0; i < positionHistory.Count; i++)
            {
                if (positionHistory[i] == current)
                {
                    count++;
                    if (count >= 3) return true;
                }
            }
            return false;
        }

        public static bool CheckInsufficientMaterial(Board board)
        {
            int wKnights = 0, bKnights = 0, wBishops = 0, bBishops = 0;
            int wPawns = 0, bPawns = 0, wRooks = 0, bRooks = 0, wQueens = 0, bQueens = 0;

            for (int i = 0; i < 64; i++)
            {
                int piece = board.gameBoard[i];
                int type  = Math.Abs(piece);
                if (piece > 0)
                {
                    switch (type)
                    {
                        case Pieces.PAWN:   wPawns++;   break;
                        case Pieces.KNIGHT: wKnights++; break;
                        case Pieces.BISHOP: wBishops++; break;
                        case Pieces.ROOK:   wRooks++;   break;
                        case Pieces.QUEEN:  wQueens++;  break;
                    }
                }
                else if (piece < 0)
                {
                    switch (type)
                    {
                        case Pieces.PAWN:   bPawns++;   break;
                        case Pieces.KNIGHT: bKnights++; break;
                        case Pieces.BISHOP: bBishops++; break;
                        case Pieces.ROOK:   bRooks++;   break;
                        case Pieces.QUEEN:  bQueens++;  break;
                    }
                }
            }

            if (wPawns > 0 || bPawns > 0 || wRooks > 0 || bRooks > 0 || wQueens > 0 || bQueens > 0)
                return false;
            if (wKnights == 0 && wBishops == 0 && bKnights == 0 && bBishops == 0) return true;
            if ((wKnights == 1 && wBishops == 0 && bKnights == 0 && bBishops == 0) ||
                (bKnights == 1 && bBishops == 0 && wKnights == 0 && wBishops == 0)) return true;
            if ((wBishops == 1 && wKnights == 0 && bKnights == 0 && bBishops == 0) ||
                (bBishops == 1 && bKnights == 0 && wKnights == 0 && wBishops == 0)) return true;
            return false;
        }
    }
}
