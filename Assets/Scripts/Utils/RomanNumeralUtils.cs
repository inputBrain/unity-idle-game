using System.Text;

namespace Utils
{
    public static class RomanNumeralUtils
    {
        public static string ToRoman(int number)
        {
            if (number <= 0)
                return string.Empty;

            var values = new[] {1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1};
            var numerals = new[] {"M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"};

            var result = new StringBuilder();
            for (var i = 0; i < values.Length && number > 0; i++)
            {
                while (number >= values[i])
                {
                    result.Append(numerals[i]);
                    number -= values[i];
                }
            }
            return result.ToString();
        }
    }
}
