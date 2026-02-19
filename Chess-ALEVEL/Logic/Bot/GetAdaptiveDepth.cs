namespace Game.Logic.Bot
{
    public class GetAdaptiveDepth
    {
        public static int GetDepth(Board board, int moveCount)
        {
            int material = CalculateMaterial.GetTotalMaterial(board);
            
            if (moveCount <= 10) 
                return 7;
            else if (moveCount <= 20 && material <= 2500) 
                return 6;
            else if (moveCount <= 25) 
                return 5;
            else 
                return 4;
            
        }
    }
}
