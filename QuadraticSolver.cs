using System;

namespace ComplexCalculator
{
    public class QuadraticSolver
    {
        public static string Solve(double a, double b, double c)
        {
            if (a == 0) 
                return "Это не квадратное уравнение (a не может быть 0).";

            double d = b * b - 4 * a * c;

            if (d > 0)
            {
                double x1 = (-b + Math.Sqrt(d)) / (2 * a);
                double x2 = (-b - Math.Sqrt(d)) / (2 * a);
                return $"D = {d:F2}\nx1 = {x1:F4}\nx2 = {x2:F4}";
            }
            else if (d == 0)
            {
                double x = -b / (2 * a);
                return $"D = 0\nx = {x:F4}";
            }
            else
            {
                // Для красоты можем добавить расчет комплексных корней, 
                // раз уж у нас есть класс ComplexNumber, но обычно просят просто "корней нет"
                double realPart = -b / (2 * a);
                double imagPart = Math.Sqrt(-d) / (2 * a);
                return $"D = {d:F2}\nКорни комплексные:\nx1 = {realPart:F2} + {imagPart:F2}i\nx2 = {realPart:F2} - {imagPart:F2}i";
            }
        }
    }
}