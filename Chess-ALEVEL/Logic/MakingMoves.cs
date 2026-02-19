using Game.Logic.Bot;

namespace Game.Logic
{
    public class MakingMoves : MainGame
    {
        public static char sideToMove = 'w';
        private static int enPassantSquare = -1;
        private static bool whiteKingMoved = false;
        private static bool blackKingMoved = false;
        private static bool whiteKingsideRookMoved = false;
        private static bool whiteQueensideRookMoved = false;
        private static bool blackKingsideRookMoved = false;
        private static bool blackQueensideRookMoved = false;
        private static Timer gameTimer = null;
        private static bool useTimer = false;

        public static void ResetGameState()
        {
            sideToMove = 'w';
            enPassantSquare = -1;
            whiteKingMoved = false;
            blackKingMoved = false;
            whiteKingsideRookMoved = false;
            whiteQueensideRookMoved = false;
            blackKingsideRookMoved = false;
            blackQueensideRookMoved = false;
            Game.ResetGameHistory();
        }

        public static bool CanCastleKingside(bool isWhite)
        {
            return isWhite ? !whiteKingMoved && !whiteKingsideRookMoved
                           : !blackKingMoved && !blackKingsideRookMoved;
        }

        public static bool CanCastleQueenside(bool isWhite)
        {
            return isWhite ? !whiteKingMoved && !whiteQueensideRookMoved
                           : !blackKingMoved && !blackQueensideRookMoved;
        }

        public static void InitializeTimer(bool enableTimer, int timePerSideInSeconds = 600)
        {
            useTimer = enableTimer;
            if (useTimer)
            {
                gameTimer = new Timer(timePerSideInSeconds);
                gameTimer.onTimeExpired += OnTimerExpired;
                Console.WriteLine($"Timer enabled: {timePerSideInSeconds / 60} minutes per side");
            }
        }

        private static void OnTimerExpired(char side)
        {
            Console.Clear();
            Console.WriteLine($"\n{(side == 'w' ? "White" : "Black")}'s time has expired!");
            Console.WriteLine($"{(side == 'w' ? "Black" : "White")} wins on time!");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        public static void HandleMoves(Board board)
        {
            if (useTimer && gameTimer != null) { gameTimer.DisplayTimers(); Console.WriteLine(); }

            Console.WriteLine($"It is {(sideToMove == 'w' ? "White" : "Black")}'s turn.");

            string result = Game.CheckGameState(sideToMove, board);
            if (result != "null")
            {
                if (useTimer && gameTimer != null) gameTimer.Stop();
                Console.WriteLine($"Game over: {result}");
                Console.ReadKey();
                return;
            }

            if (useTimer && gameTimer != null) gameTimer.Start(sideToMove);

            bool isBotTurn = false;
            if (MainGame.userGameMode == "1")
                isBotTurn = (userSide == 'w' && sideToMove == 'b') || (userSide == 'b' && sideToMove == 'w');
            else if (userGameMode == "3")
                isBotTurn = true;

            if (isBotTurn)
            {
                Move.MoveInfo botMove = FindBestMove.FindBestMoveNow(sideToMove, board);
                if (botMove != null)
                {
                    Console.WriteLine($"kenith moves from {botMove.from} to {botMove.to}");
                    ExecuteMove(board, botMove);
                    if (botMove.moveType == Move.MoveType.Promotion || botMove.moveType == Move.MoveType.PromotionCapture)
                    {
                        int colour = sideToMove == 'w' ? Pieces.WHITE : Pieces.BLACK;
                        board.gameBoard[botMove.to] = Pieces.QUEEN * colour;
                    }
                    if (useTimer && gameTimer != null) gameTimer.Stop();
                    sideToMove = sideToMove == 'w' ? 'b' : 'w';
                    if (userGameMode == "3") Thread.Sleep(500);
                }
                return;
            }

            Console.Write("piece to move: ");
            string fromInput = Console.ReadLine();
            if (!Sq.TryParse(fromInput, out int userPieceSelection))
            {
                Console.WriteLine("invalid"); Console.ReadKey(); return;
            }

            int usersPiece = board.gameBoard[userPieceSelection];
            if (usersPiece == Pieces.NO_PIECE)
            {
                Console.WriteLine("no piece on that square"); Console.ReadKey(); return;
            }
            if ((sideToMove == 'w' && !PieceHelpers.IsWhite(usersPiece)) ||
                (sideToMove == 'b' && !PieceHelpers.IsBlack(usersPiece)))
            {
                Console.WriteLine("wrong colour"); Console.ReadKey(); return;
            }

            var moves = Game.GetLegalMovesForPiece(sideToMove, board, userPieceSelection);
            if (moves.Count == 0) { Console.WriteLine("no legal moves"); Console.ReadKey(); return; }

            Console.WriteLine("legal moves:");
            for (int i = 0; i < moves.Count; i++)
                Console.WriteLine($"{Sq.ToAlgebraic(moves[i].from)} -> {Sq.ToAlgebraic(moves[i].to)} ({moves[i].moveType})");

            Console.Write("move to: ");
            string toInput = Console.ReadLine();
            if (!Sq.TryParse(toInput, out int toIndex))
            {
                Console.WriteLine("invalid"); Console.ReadKey(); return;
            }

            Move.MoveInfo selectedMove = null;
            for (int i = 0; i < moves.Count; i++)
            {
                if (moves[i].to == toIndex)
                {
                    selectedMove = moves[i];
                    i = moves.Count;
                }
            }

            if (selectedMove != null)
            {
                if (selectedMove.moveType == Move.MoveType.Promotion || selectedMove.moveType == Move.MoveType.PromotionCapture)
                    PromotePawn(board, selectedMove);
                else
                    ExecuteMove(board, selectedMove);

                if (useTimer && gameTimer != null) gameTimer.Stop();
                sideToMove = sideToMove == 'w' ? 'b' : 'w';
                board.PrintBoard(userSide);
            }
            else
            {
                Console.WriteLine("Illegal move"); Console.ReadKey();
            }
        }

        public static void ExecuteMove(Board board, Move.MoveInfo move)
        {
            int movingPiece  = board.gameBoard[move.from];
            int capturedPiece = board.gameBoard[move.to];
            bool isPawnMove  = Math.Abs(movingPiece) == Pieces.PAWN;
            bool isCapture   = capturedPiece != Pieces.NO_PIECE || move.moveType == Move.MoveType.EnPassant;

            if (Math.Abs(movingPiece) == Pieces.KING)
            {
                if (PieceHelpers.IsWhite(movingPiece)) whiteKingMoved = true;
                else blackKingMoved = true;
            }
            else if (Math.Abs(movingPiece) == Pieces.ROOK)
            {
                if      (move.from == 0)  whiteQueensideRookMoved = true;
                else if (move.from == 7)  whiteKingsideRookMoved  = true;
                else if (move.from == 56) blackQueensideRookMoved = true;
                else if (move.from == 63) blackKingsideRookMoved  = true;
            }

            if (enPassantSquare != -1) { board.gameBoard[enPassantSquare] = Pieces.NO_PIECE; enPassantSquare = -1; }

            if (move.moveType == Move.MoveType.Castle)
            {
                bool kingSide = move.to > move.from;
                board.gameBoard[move.to]   = movingPiece;
                board.gameBoard[move.from] = Pieces.NO_PIECE;
                int rookFrom = kingSide ? move.from + 3 : move.from - 4;
                int rookTo   = kingSide ? move.from + 1 : move.from - 1;
                board.gameBoard[rookTo]   = board.gameBoard[rookFrom];
                board.gameBoard[rookFrom] = Pieces.NO_PIECE;
                Game.RecordMove(board, move, isPawnMove, isCapture);
                return;
            }

            if (move.moveType == Move.MoveType.EnPassant)
            {
                int capturedPawnSquare = PieceHelpers.IsWhite(movingPiece) ? move.to - 8 : move.to + 8;
                board.gameBoard[capturedPawnSquare] = Pieces.NO_PIECE;
            }

            board.gameBoard[move.to]   = movingPiece;
            board.gameBoard[move.from] = Pieces.NO_PIECE;

            if (move.moveType == Move.MoveType.DoubleMove)
            {
                enPassantSquare = PieceHelpers.IsWhite(movingPiece) ? move.to - 8 : move.to + 8;
                board.gameBoard[enPassantSquare] = PieceHelpers.IsWhite(movingPiece)
                    ? Pieces.EN_PASSANT_MARKER : Pieces.BLACK * Pieces.EN_PASSANT_MARKER;
            }

            Game.RecordMove(board, move, isPawnMove, isCapture);
        }

        public static void PromotePawn(Board board, Move.MoveInfo move)
        {
            ExecuteMove(board, move);
            Console.WriteLine("Promoting Pawn Options \n1: Queen\n2: Rook\n3: Bishop\n4: Knight\nEnter your choice:");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": 
                case "queen":
                    board.gameBoard[move.to] = sideToMove == 'w' ? Pieces.QUEEN  : Pieces.BLACK * Pieces.QUEEN;  
                    break;
                case "2": 
                case "rook":
                    board.gameBoard[move.to] = sideToMove == 'w' ? Pieces.ROOK   : Pieces.BLACK * Pieces.ROOK;   
                    break;
                case "3": 
                case "bishop":
                    board.gameBoard[move.to] = sideToMove == 'w' ? Pieces.BISHOP : Pieces.BLACK * Pieces.BISHOP; 
                    break;
                case "4": 
                case "knight":
                    board.gameBoard[move.to] = sideToMove == 'w' ? Pieces.KNIGHT : Pieces.BLACK * Pieces.KNIGHT; 
                    break;
                default:
                    Console.WriteLine("auto promoting to Queen");
                    board.gameBoard[move.to] = sideToMove == 'w' ? Pieces.QUEEN  : Pieces.BLACK * Pieces.QUEEN;  
                    break;
            }
        }
    }
}
