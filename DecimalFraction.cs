using System;
using System.Globalization;

namespace ComplexCalculator
{
    public class DecimalFraction
    {
        private decimal _value;

        public DecimalFraction(string value)
        {
            // Используем инвариантную культуру (точка как разделитель), 
            // чтобы не было проблем с запятыми/точками
            if (!decimal.TryParse(value.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out _value))
                throw new ArgumentException("Некорректный формат десятичной дроби.");
        }

        private DecimalFraction(decimal value)
        {
            _value = value;
        }

        // Перегрузка операторов
        public static DecimalFraction operator +(DecimalFraction a, DecimalFraction b) => new DecimalFraction(a._value + b._value);
        public static DecimalFraction operator -(DecimalFraction a, DecimalFraction b) => new DecimalFraction(a._value - b._value);
        public static DecimalFraction operator *(DecimalFraction a, DecimalFraction b) => new DecimalFraction(a._value * b._value);
        public static DecimalFraction operator /(DecimalFraction a, DecimalFraction b)
        {
            if (b._value == 0) throw new DivideByZeroException("Деление на ноль в десятичных дробях.");
            return new DecimalFraction(a._value / b._value);
        }

        public override string ToString() => _value.ToString(CultureInfo.InvariantCulture);
    }
}