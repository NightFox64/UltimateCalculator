using System;
using System.Collections.Generic;
using System.Linq;

namespace ComplexCalculator
{
    public class NumberSystemConverter
    {
        private const string Alphabet = "0123456789ABCDEF";

        // Главный метод: Перевод из любой системы в любую
        public static string Convert(string number, int fromBase, int toBase)
        {
            if (fromBase == toBase) return number;

            // 1. Переводим всё в десятичную систему (промежуточный этап)
            double decimalValue = ToDecimal(number.ToUpper(), fromBase);

            // 2. Из десятичной переводим в целевую
            return FromDecimal(decimalValue, toBase);
        }

        // Из N-ричной в 10-ричную
        private static double ToDecimal(string number, int fromBase)
        {
            string[] parts = number.Split(new[] { '.', ',' });
            double result = 0;

            // Целая часть
            string integerPart = parts[0];
            for (int i = 0; i < integerPart.Length; i++)
            {
                int value = Alphabet.IndexOf(integerPart[i]);
                if (value < 0 || value >= fromBase) throw new Exception($"Символ {integerPart[i]} недопустим для системы {fromBase}");
                result = result * fromBase + value;
            }

            // Дробная часть
            if (parts.Length > 1)
            {
                string fractionalPart = parts[1];
                for (int i = 0; i < fractionalPart.Length; i++)
                {
                    int value = Alphabet.IndexOf(fractionalPart[i]);
                    if (value < 0 || value >= fromBase) throw new Exception($"Символ {fractionalPart[i]} недопустим для системы {fromBase}");
                    result += value * Math.Pow(fromBase, -(i + 1));
                }
            }
            return result;
        }

        // Из 10-ричной в N-ричную
        private static string FromDecimal(double value, int toBase)
        {
            long integerPart = (long)Math.Floor(value);
            double fractionalPart = value - integerPart;

            // Перевод целой части
            string resInt = "";
            if (integerPart == 0) resInt = "0";
            while (integerPart > 0)
            {
                resInt = Alphabet[(int)(integerPart % toBase)] + resInt;
                integerPart /= toBase;
            }

            // Перевод дробной части (до 10 знаков точности)
            if (fractionalPart <= 0) return resInt;

            string resFrac = "";
            for (int i = 0; i < 10; i++)
            {
                fractionalPart *= toBase;
                int digit = (int)Math.Floor(fractionalPart);
                resFrac += Alphabet[digit];
                fractionalPart -= digit;
                if (fractionalPart == 0) break;
            }

            return resInt + "." + resFrac;
        }
    }
}