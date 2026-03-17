using System.IO;

namespace ComplexCalculator
{
    public static class Logger
    {
        private const string LogFileName = "protocol.txt";

        public static void Log(string input1, string op, string input2, string result)
        {
            string entry = $"{DateTime.Now}: {input1} {op} {input2} = {result}";
            File.AppendAllLines(LogFileName, new[] { entry });
        }

        public static void LogError(string message)
        {
            File.AppendAllLines(LogFileName, new[] { $"{DateTime.Now}: ОШИБКА: {message}" });
        }

        public static string GetLog()
        {
            if (!File.Exists(LogFileName)) return "Протокол пуст.";
            return File.ReadAllText(LogFileName);
        }
    }
}