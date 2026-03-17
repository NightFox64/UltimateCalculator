using System;
using System.Windows.Forms;

namespace ComplexCalculator
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Запускаем наше главное окно
            Application.Run(new CalculatorForm()); 
        }
    }
}