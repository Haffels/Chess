namespace Game.Logic.Bot
{
    public class GetAdaptiveDepth
    {
        public static int GetDepth(Board board, int moveCount)
        {
            int material = CalculateMaterial.GetTotalMaterial(board);
            int depth = Kenith.MAX_DEPTH;
            
            if (material <= 4500) 
                depth += 1;
            if (material <= 2500) 
                depth += 1;
            if (material <= 1500) 
                depth += 2;
            
            if (moveCount <= 10)
                depth += 2;

            return Math.Min(depth, Kenith.MAX_DEPTH + 2);
        }
    }
}
