namespace Game.Logic
{
    public static class Sq
    {
        public static bool TryParse(string input, out int index)
        {
            index = -1;
            
            if (input.Length != 2)
                return false;
            
            char file = char.ToLower(input[0]);
            char rank = input[1];
            
            if (file < 'a' || file > 'h')
                return false;
            if (rank < '1' || rank > '8')
                return false;
            
            index = (rank - '1') * 8 + (file - 'a');
            
            return true;
        }

        public static string ToAlgebraic(int index)
        {
            return $"{(char)('a' + index % 8)}{index / 8 + 1}";
        }
    }
}
