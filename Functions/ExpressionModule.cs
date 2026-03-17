using System;
using System.Data; // Нужно для DataTable
using System.Collections.Generic;

namespace ComplexCalculator
{
    public class ExpressionModule
    {
        // 1. Проверка правильности выражения
        public static void Validate(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new Exception("Выражение пустое");

            // Проверка баланса скобок
            Stack<char> brackets = new Stack<char>();
            foreach (char c in expression)
            {
                if (c == '(') brackets.Push(c);
                else if (c == ')')
                {
                    if (brackets.Count == 0) throw new Exception("Лишняя закрывающая скобка ')'");
                    brackets.Pop();
                }
            }
            if (brackets.Count > 0) throw new Exception("Не закрыта открывающая скобка '('");

            // Проверка на пустые скобки ()
            if (expression.Contains("()")) throw new Exception("Выражение содержит пустые скобки");
            
            // Проверка на недопустимые символы
            string validChars = "0123456789+-*/()., ";
            foreach (char c in expression)
            {
                if (validChars.IndexOf(c) == -1)
                    throw new Exception($"Недопустимый символ в выражении: {c}");
            }
        }

        // 2. Вычисление
        public static string Evaluate(string expression)
        {
            try
            {
                // Заменяем запятые на точки для корректной работы движка
                string prepared = expression.Replace(",", ".");
                
                // Используем DataTable как математический движок
                DataTable table = new DataTable();
                var result = table.Compute(prepared, "");
                return Convert.ToDouble(result).ToString("F4");
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при вычислении. Проверьте правильность знаков.");
            }
        }
    }
}