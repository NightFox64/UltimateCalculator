using System;
using System.Numerics;

namespace ComplexCalculator
{
    public enum ComplexForm { Algebraic, Trigonometric, Exponential }

    public class ComplexNumber
    {
        public double Re { get; }
        public double Im { get; }

        public ComplexNumber(double re, double im)
        {
            Re = re;
            Im = im;
        }

        // Модуль (r)
        public double Magnitude => Math.Sqrt(Re * Re + Im * Im);
        // Аргумент (угол в радианах)
        public double Phase => Math.Atan2(Im, Re);

        // Статические методы для операций
        public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b) 
            => new ComplexNumber(a.Re + b.Re, a.Im + b.Im);

        public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b) 
            => new ComplexNumber(a.Re - b.Re, a.Im - b.Im);

        public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b) 
            => new ComplexNumber(a.Re * b.Re - a.Im * b.Im, a.Re * b.Im + a.Im * b.Re);

        public static ComplexNumber operator /(ComplexNumber a, ComplexNumber b)
        {
            double denom = b.Re * b.Re + b.Im * b.Im;
            if (denom == 0) throw new DivideByZeroException("Деление на комплексный ноль.");
            return new ComplexNumber((a.Re * b.Re + a.Im * b.Im) / denom, (a.Im * b.Re - a.Re * b.Im) / denom);
        }

        // Возведение в целую степень (Формула Муавра)
        public ComplexNumber Power(int n)
        {
            double rN = Math.Pow(Magnitude, n);
            double phiN = Phase * n;
            return new ComplexNumber(rN * Math.Cos(phiN), rN * Math.Sin(phiN));
        }

        // Метод для красивого вывода в разных формах
        public string ToString(ComplexForm form)
        {
            switch (form)
            {
                case ComplexForm.Trigonometric:
                    return $"{Magnitude:F2}(cos({Phase:F2}) + i*sin({Phase:F2}))";
                case ComplexForm.Exponential:
                    return $"{Magnitude:F2} * e^(i*{Phase:F2})";
                default: // Algebraic
                    return $"{Re:F2} {(Im >= 0 ? "+" : "-")} {Math.Abs(Im):F2}i";
            }
        }

        // Парсинг строки "a+bi"
        public static ComplexNumber Parse(string s)
        {
            s = s.Replace(" ", "").Replace("I", "i");
        
            // Если в строке вообще нет 'i', это чисто действительное число
            if (!s.Contains("i"))
            {
                return new ComplexNumber(double.Parse(s), 0);
            }
        
            // Обработка случая, когда введено только 'i' или '-i'
            if (s == "i") return new ComplexNumber(0, 1);
            if (s == "-i") return new ComplexNumber(0, -1);
        
            double re = 0;
            double im = 0;
        
            // Ищем последний плюс или минус, который разделяет Re и Im части
            // (но не тот минус, что стоит в самом начале числа)
            int splitIndex = -1;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if ((s[i] == '+' || s[i] == '-') && i > 0)
                {
                    splitIndex = i;
                    break;
                }
            }
        
            if (splitIndex == -1) // Значит, есть только мнимая часть (например "5i" или "-i")
            {
                im = ParseImaginaryPart(s);
                re = 0;
            }
            else // Есть обе части (например "1+i" или "-2-3i")
            {
                string rePart = s.Substring(0, splitIndex);
                string imPart = s.Substring(splitIndex); // сохраняем знак (+ или -)
        
                re = double.Parse(rePart);
                im = ParseImaginaryPart(imPart);
            }
        
            return new ComplexNumber(re, im);
        }
        
        // Вспомогательный метод для обработки мнимой части ("+i", "-5i", "i" и т.д.)
        private static double ParseImaginaryPart(string part)
        {
            part = part.Replace("i", "");
            if (part == "" || part == "+") return 1;
            if (part == "-") return -1;
            return double.Parse(part);
        }
    }
}