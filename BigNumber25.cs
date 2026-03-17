using System;
using System.Numerics; // Потребуется ссылка на System.Runtime.Numerics

namespace ComplexCalculator
{
    // Класс для работы с 25-значными числами
    public class BigNumber25
    {
        private BigInteger _value;
        private const int MaxDigits = 25;

        public BigNumber25(string value)
        {
            if (!Validate(value))
                throw new ArgumentException($"Число должно содержать только цифры и быть не длиннее {MaxDigits} знаков.");
            
            _value = BigInteger.Parse(value);
        }

        private BigNumber25(BigInteger value)
        {
            _value = value;
        }

        public static bool Validate(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            // Убираем минус для проверки длины
            string clearValue = value.StartsWith("-") ? value.Substring(1) : value;
            return clearValue.Length <= MaxDigits && BigInteger.TryParse(value, out _);
        }

        // Перегрузка операторов
        public static BigNumber25 operator +(BigNumber25 a, BigNumber25 b) => new BigNumber25(a._value + b._value);
        public static BigNumber25 operator -(BigNumber25 a, BigNumber25 b) => new BigNumber25(a._value - b._value);
        public static BigNumber25 operator *(BigNumber25 a, BigNumber25 b) => new BigNumber25(a._value * b._value);
        public static BigNumber25 operator /(BigNumber25 a, BigNumber25 b) => new BigNumber25(a._value / b._value);
        public static BigNumber25 operator %(BigNumber25 a, BigNumber25 b) => new BigNumber25(a._value % b._value);

        public override string ToString() => _value.ToString();
    }
}