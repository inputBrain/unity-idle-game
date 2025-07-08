namespace Utils
{
    public static class RomanNumeralUtils
    {
        public static string ToRoman(int number)
        {
            return number switch
            {
                1  => "I",
                2  => "II",
                3  => "III",
                4  => "IV",
                5  => "V",
                6  => "VI",
                7  => "VII",
                8  => "VIII",
                9  => "IX",
                10 => "X",
                _ => number.ToString(),
            };
        }
    }
}