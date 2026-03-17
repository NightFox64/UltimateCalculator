using System;
using System.Globalization;

namespace ComplexCalculator
{
    public class DateModule
    {
        // Допустимые форматы по заданию
        private static readonly string[] Formats = { 
            "dd.MM.yyyy", // Российский
            "dd/MM/yyyy", // Английский
            "MM/dd/yyyy"  // Американский
        };

        public static DateTime Parse(string input)
        {
            // TryParseExact проверяет дату на корректность (33 июня не пройдет)
            if (DateTime.TryParseExact(input, Formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            throw new Exception("Неверный формат даты или такой даты не существует (например, 33 июня). " +
                                "Используйте: дд.мм.гггг, дд/мм/гггг или мм/дд/гггг");
        }

        public static string GetFullDifference(DateTime d1, DateTime d2)
        {
            // Берем абсолютную разницу, чтобы не было отрицательных чисел
            DateTime start = d1 < d2 ? d1 : d2;
            DateTime end = d1 < d2 ? d2 : d1;

            TimeSpan span = end - start;

            // Расчет полных месяцев
            int months = (end.Year - start.Year) * 12 + end.Month - start.Month;
            // Если день окончания меньше дня начала, значит последний месяц еще не завершен
            if (end.Day < start.Day) months--;

            double weeks = span.TotalDays / 7.0;

            return $"Разница между датами:\n" +
                   $"- Месяцев: {Math.Max(0, months)}\n" +
                   $"- Недель: {weeks:F1}\n" +
                   $"- Дней: {span.TotalDays}\n" +
                   $"- Часов: {span.TotalHours}\n" +
                   $"- Минут: {span.TotalMinutes}\n" +
                   $"- Секунд: {span.TotalSeconds}";
        }
    }
}