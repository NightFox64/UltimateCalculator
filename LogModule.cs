using System;

namespace ComplexCalculator
{
    public class LogNumber
    {
        public double Argument { get; }
        public double Base { get; }

        public LogNumber(double argument, double b)
        {
            if (argument <= 0) throw new Exception("Аргумент логарифма должен быть > 0");
            if (b <= 0 || b == 1) throw new Exception("Основание должно быть > 0 и != 1");
            Argument = argument;
            Base = b;
        }

        // Вычисление значения логарифма: log_b(a) = ln(a) / ln(b)
        public double GetValue() => Math.Log(Argument) / Math.Log(Base);

        // Переход к новому основанию
        public LogNumber ChangeBase(double newBase) => new LogNumber(Argument, newBase);

        public override string ToString() => $"log_{Base}({Argument}) = {GetValue():F4}";
    }
}