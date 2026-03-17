using System;

namespace ComplexCalculator
{
    public class SystemSolver
    {
        // Вспомогательный метод для расчета определителя 3x3 (правило треугольника)
        private static double Det3x3(double[,] m)
        {
            return m[0, 0] * m[1, 1] * m[2, 2] + m[0, 1] * m[1, 2] * m[2, 0] + m[0, 2] * m[1, 0] * m[2, 1] -
                   m[0, 2] * m[1, 1] * m[2, 0] - m[0, 1] * m[1, 0] * m[2, 2] - m[0, 0] * m[1, 2] * m[2, 1];
        }

        public static string Solve(double[] eq1, double[] eq2, double[] eq3)
        {
            if (eq1.Length < 4 || eq2.Length < 4 || eq3.Length < 4)
                throw new Exception("В каждой строке должно быть 4 числа (a, b, c и свободный член d)");

            // Главная матрица коэффициентов
            double[,] A = {
                { eq1[0], eq1[1], eq1[2] },
                { eq2[0], eq2[1], eq2[2] },
                { eq3[0], eq3[1], eq3[2] }
            };

            double delta = Det3x3(A);

            if (Math.Abs(delta) < 1e-9)
                return "Система не имеет однозначного решения (определитель = 0)";

            // Матрицы для x, y, z (заменяем столбцы на свободные члены d1, d2, d3)
            double[] D = { eq1[3], eq2[3], eq3[3] };

            double[,] Ax = (double[,])A.Clone();
            for (int i = 0; i < 3; i++) Ax[i, 0] = D[i];

            double[,] Ay = (double[,])A.Clone();
            for (int i = 0; i < 3; i++) Ay[i, 1] = D[i];

            double[,] Az = (double[,])A.Clone();
            for (int i = 0; i < 3; i++) Az[i, 2] = D[i];

            double x = Det3x3(Ax) / delta;
            double y = Det3x3(Ay) / delta;
            double z = Det3x3(Az) / delta;

            return $"Результат:\nx = {x:F4}\ny = {y:F4}\nz = {z:F4}";
        }
    }
}