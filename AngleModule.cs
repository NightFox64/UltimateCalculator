using System;

namespace ComplexCalculator
{
    public class AngleModule
    {
        public double TotalSeconds { get; private set; }

        public AngleModule(int deg, int min, int sec)
        {
            TotalSeconds = deg * 3600 + min * 60 + sec;
        }

        private AngleModule(double totalSeconds)
        {
            TotalSeconds = totalSeconds;
        }

        // Парсинг формата "45 10 30" (градусы минуты секунды)
        public static AngleModule Parse(string s)
        {
            var p = s.Split(new[] { ' ', '.', ',' }, StringSplitOptions.RemoveEmptyEntries);
            int d = p.Length > 0 ? int.Parse(p[0]) : 0;
            int m = p.Length > 1 ? int.Parse(p[1]) : 0;
            int s_val = p.Length > 2 ? int.Parse(p[2]) : 0;
            return new AngleModule(d, m, s_val);
        }

        public static AngleModule operator +(AngleModule a, AngleModule b) => new AngleModule(a.TotalSeconds + b.TotalSeconds);
        public static AngleModule operator -(AngleModule a, AngleModule b) => new AngleModule(a.TotalSeconds - b.TotalSeconds);
        public static AngleModule operator *(AngleModule a, double scalar) => new AngleModule(a.TotalSeconds * scalar);
        public static AngleModule operator /(AngleModule a, double scalar) => new AngleModule(a.TotalSeconds / scalar);

        // Тригонометрия (нужно переводить в радианы)
        public double ToRadians() => (TotalSeconds / 3600.0) * (Math.PI / 180.0);
        
        public string GetTrig()
        {
            double rad = ToRadians();
            return $"sin: {Math.Sin(rad):F4}\ncos: {Math.Cos(rad):F4}\ntan: {Math.Tan(rad):F4}";
        }

        public override string ToString()
        {
            int total = (int)Math.Abs(TotalSeconds);
            int d = total / 3600;
            int m = (total % 3600) / 60;
            int s = total % 60;
            return $"{(TotalSeconds < 0 ? "-" : "")}{d}° {m}' {s}''";
        }
    }
}