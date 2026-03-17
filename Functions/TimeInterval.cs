using System;

namespace ComplexCalculator
{
    public class TimeInterval
    {
        public long TotalSeconds { get; private set; }

        public TimeInterval(long h, long m, long s)
        {
            TotalSeconds = h * 3600 + m * 60 + s;
        }

        public TimeInterval(long totalSeconds)
        {
            TotalSeconds = totalSeconds;
        }

        public static TimeInterval operator +(TimeInterval a, TimeInterval b) => new TimeInterval(a.TotalSeconds + b.TotalSeconds);
        public static TimeInterval operator -(TimeInterval a, TimeInterval b) => new TimeInterval(Math.Abs(a.TotalSeconds - b.TotalSeconds));

        // Преобразования
        public double ToHours() => TotalSeconds / 3600.0;
        public double ToMinutes() => TotalSeconds / 60.0;
        
        public override string ToString()
        {
            long h = TotalSeconds / 3600;
            long m = (TotalSeconds % 3600) / 60;
            long s = TotalSeconds % 60;
            return $"{h}ч {m}м {s}с";
        }

        public static TimeInterval Parse(string s, string unit)
        {
            long val = long.Parse(s);
            switch (unit)
            {
                case "Часы": return new TimeInterval(val, 0, 0);
                case "Минуты": return new TimeInterval(0, val, 0);
                case "Секунды": return new TimeInterval(0, 0, val);
                default: return new TimeInterval(val);
            }
        }
    }
}