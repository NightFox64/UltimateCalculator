using System;
using System.Data;
using System.Globalization;

namespace ComplexCalculator
{
    public class RootFinder
    {
        // Вычисление f(x) путем замены "x" в строке на число
        private static double f(string func, double x)
        {
            string prepared = func.ToLower().Replace("x", $"({x.ToString(CultureInfo.InvariantCulture)})").Replace(",", ".");
            DataTable table = new DataTable();
            return Convert.ToDouble(table.Compute(prepared, ""));
        }

        // 1. Метод половинного деления
        public static double Bisection(string func, double a, double b, double eps)
        {
            if (f(func, a) * f(func, b) >= 0)
                throw new Exception("На концах отрезка функция должна иметь разные знаки!");

            double c = a;
            while ((b - a) / 2 > eps)
            {
                c = (a + b) / 2;
                if (f(func, c) == 0) break;
                else if (f(func, a) * f(func, c) < 0) b = c;
                else a = c;
            }
            return c;
        }

        // 2. Метод Ньютона (используем численную производную)
        public static double Newton(string func, double x0, double eps)
        {
            double x = x0;
            for (int i = 0; i < 1000; i++) // Ограничение итераций для защиты от зацикливания
            {
                double fx = f(func, x);
                // Численное нахождение производной f'(x) ≈ (f(x+h) - f(x))/h
                double h = 0.000001;
                double dfx = (f(func, x + h) - fx) / h;

                if (Math.Abs(dfx) < 1e-10) throw new Exception("Производная близка к нулю. Метод Ньютона не сходится.");

                double xNext = x - fx / dfx;
                if (Math.Abs(xNext - x) < eps) return xNext;
                x = xNext;
            }
            return x;
        }

        // 3. Метод итераций (упрощенная реализация x = x - f(x)*k)
        public static double Iteration(string func, double x0, double eps)
        {
            double x = x0;
            double k = 0.01; // Коэффициент релаксации
            for (int i = 0; i < 5000; i++)
            {
                double fx = f(func, x);
                double xNext = x - k * fx;
                if (Math.Abs(xNext - x) < eps) return xNext;
                x = xNext;
            }
            return x;
        }
    }
}