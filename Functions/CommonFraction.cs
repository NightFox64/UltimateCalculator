using System;

namespace ComplexCalculator
{
    public class CommonFraction
    {
        public long Numerator { get; private set; }   // Числитель
        public long Denominator { get; private set; } // Знаменатель

        public CommonFraction(long num, long den)
        {
            if (den == 0) throw new DivideByZeroException("Знаменатель не может быть нулевым.");
            
            // Убеждаемся, что минус всегда в числителе
            if (den < 0) { num = -num; den = -den; }
            
            Numerator = num;
            Denominator = den;
            Simplify();
        }

        // Сокращение дроби
        private void Simplify()
        {
            long n = Math.Abs(Numerator);
            long d = Math.Abs(Denominator);
            while (d != 0) { n %= d; long t = n; n = d; d = t; } // НОД
            
            Numerator /= n;
            Denominator /= n;
        }

        // Операции
        public static CommonFraction operator +(CommonFraction a, CommonFraction b)
            => new CommonFraction(a.Numerator * b.Denominator + b.Numerator * a.Denominator, a.Denominator * b.Denominator);

        public static CommonFraction operator -(CommonFraction a, CommonFraction b)
            => new CommonFraction(a.Numerator * b.Denominator - b.Numerator * a.Denominator, a.Denominator * b.Denominator);

        public static CommonFraction operator *(CommonFraction a, CommonFraction b)
            => new CommonFraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);

        public static CommonFraction operator /(CommonFraction a, CommonFraction b)
            => new CommonFraction(a.Numerator * b.Denominator, a.Denominator * b.Numerator);

        // Специальные функции по заданию
        public CommonFraction Flip() => new CommonFraction(Denominator, Numerator); // Обмен местами
        public CommonFraction Negate() => new CommonFraction(-Numerator, Denominator); // Смена знака

        public override string ToString() => $"{Numerator}/{Denominator}";

        public static CommonFraction Parse(string s)
        {
            s = s.Trim();
            if (s.Contains("/"))
            {
                var parts = s.Split('/');
                return new CommonFraction(long.Parse(parts[0]), long.Parse(parts[1]));
            }
            // Если ввели целое число (например "5"), превращаем в "5/1"
            return new CommonFraction(long.Parse(s), 1);
        }
    }
}