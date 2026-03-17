using System;
using System.Collections.Generic;
using System.Text;

namespace ComplexCalculator
{
    public class RomanNumber
    {
        public int Value { get; private set; }

        public RomanNumber(int value)
        {
            if (value <= 0) throw new Exception("Римские числа должны быть > 0");
            Value = value;
        }

        // Перевод из Римской в Десятичную
        public static int RomanToDecimal(string roman)
        {
            roman = roman.ToUpper();
            var map = new Dictionary<char, int> { 
                {'I', 1}, {'V', 5}, {'X', 10}, {'L', 50}, {'C', 100}, {'D', 500}, {'M', 1000} 
            };
            int res = 0;
            for (int i = 0; i < roman.Length; i++)
            {
                if (i + 1 < roman.Length && map[roman[i]] < map[roman[i + 1]])
                    res -= map[roman[i]];
                else
                    res += map[roman[i]];
            }
            return res;
        }

        // Перевод из Десятичной в Римскую
        public static string DecimalToRoman(int number)
        {
            if (number < 1) return "N/A";
            int[] values = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            string[] symbols = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                while (number >= values[i]) { number -= values[i]; sb.Append(symbols[i]); }
            }
            return sb.ToString();
        }

        public override string ToString() => DecimalToRoman(Value) + $" ({Value})";
    }
}