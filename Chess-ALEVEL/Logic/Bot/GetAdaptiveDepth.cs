namespace Game.Logic.Bot
{
    public class GetAdaptiveDepth
    {
        public static int GetDepth(Board board, int moveCount)
        {
            int depth = Kenith.MAX_DEPTH;
            
            if (moveCount <= 10)
                depth = 7;
            else if (moveCount <= 15)
                depth = 6;
            else if (moveCount <= 20)
                depth = 5;
            else
                depth = 4;

            return depth;
        }
    }
}
