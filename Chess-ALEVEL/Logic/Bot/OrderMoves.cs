namespace Game.Logic.Bot
{
    public class OrderMoves
    {
        public static List<Move.moveInfo> orderMoves(List<Move.moveInfo> moves, Board board)
        {
            return moves.OrderByDescending(m =>
                m.moveType == Move.MoveType.Capture || m.moveType == Move.MoveType.PromotionCapture ? 1 : 0
            ).ToList();
        }
    }
}
