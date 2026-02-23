namespace Game.Logic
{
    public static class Sq
    {
        public static int TryParse(string input)
        {
            if (input.Length != 2) 
                return -1;
            
            char file = char.ToLower(input[0]);
            char rank = input[1];
            
            if (file < 'a' || file > 'h') 
                return -1;
            if (rank < '1' || rank > '8') 
                return -1;
            
            return (rank - '1') * 8 + (file - 'a');
        }

        public static string ToAlgebraic(int index)
        {
            return $"{(char)('a' + index % 8)}{index / 8 + 1}";
        }
    }
}
