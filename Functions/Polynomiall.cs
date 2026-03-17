using System;
using System.Collections.Generic;
using System.Linq;

namespace ComplexCalculator
{
    public class Polynomial
    {
        // Коэффициенты от младшего к старшему: [0] - x^0, [1] - x^1...
        public double[] Coeffs { get; }

        public Polynomial(double[] coeffs)
        {
            // Ограничиваем 4-й степенью (5 коэффициентов), лишнее отсекаем
            Coeffs = coeffs.Take(5).ToArray();
        }

        // Вычисление значения в точке x (метод Горнера)
        public double Evaluate(double x)
        {
            double result = 0;
            for (int i = Coeffs.Length - 1; i >= 0; i--)
            {
                result = result * x + Coeffs[i];
            }
            return result;
        }

        // Сложение
        public static Polynomial operator +(Polynomial p1, Polynomial p2)
        {
            int maxLen = Math.Max(p1.Coeffs.Length, p2.Coeffs.Length);
            double[] res = new double[maxLen];
            for (int i = 0; i < maxLen; i++)
            {
                double a = i < p1.Coeffs.Length ? p1.Coeffs[i] : 0;
                double b = i < p2.Coeffs.Length ? p2.Coeffs[i] : 0;
                res[i] = a + b;
            }
            return new Polynomial(res);
        }

        // Вычитание
        public static Polynomial operator -(Polynomial p1, Polynomial p2)
        {
            int maxLen = Math.Max(p1.Coeffs.Length, p2.Coeffs.Length);
            double[] res = new double[maxLen];
            for (int i = 0; i < maxLen; i++)
            {
                double a = i < p1.Coeffs.Length ? p1.Coeffs[i] : 0;
                double b = i < p2.Coeffs.Length ? p2.Coeffs[i] : 0;
                res[i] = a - b;
            }
            return new Polynomial(res);
        }

        // Умножение
        public static Polynomial operator *(Polynomial p1, Polynomial p2)
        {
            double[] res = new double[9]; // Макс степень 4+4=8 (9 коэфф)
            for (int i = 0; i < p1.Coeffs.Length; i++)
                for (int j = 0; j < p2.Coeffs.Length; j++)
                    res[i + j] += p1.Coeffs[i] * p2.Coeffs[j];
            return new Polynomial(res);
        }

        // Возведение в целую степень
        public Polynomial Power(int n)
        {
            Polynomial result = new Polynomial(new double[] { 1 });
            for (int i = 0; i < n; i++) result = result * this;
            return result;
        }

        public override string ToString()
        {
            List<string> parts = new List<string>();
            for (int i = Coeffs.Length - 1; i >= 0; i--)
            {
                if (Coeffs[i] == 0) continue;
                string term = Coeffs[i].ToString();
                if (i > 0) term += "x";
                if (i > 1) term += "^" + i;
                parts.Add(term);
            }
            return parts.Count == 0 ? "0" : string.Join(" + ", parts).Replace("+ -", "- ");
        }

        public static Polynomial Parse(string s)
        {
            var nums = s.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(double.Parse).ToArray();
            return new Polynomial(nums);
        }
    }
}