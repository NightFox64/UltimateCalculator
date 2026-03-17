using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComplexCalculator
{
    public class CalculatorForm : Form
    {
        // Объявляем элементы управления
        private TextBox txtInput1;
        private TextBox txtInput2;
        private Label lblResult;
        private Button btnAdd, btnSub, btnMul, btnDiv, btnClear, btnShowLog;
        private ComboBox cmbMode;

        public CalculatorForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Настройки окна
            this.Text = "Калькулятор 25-значных чисел";
            this.Size = new Size(400, 450);

            // Поля ввода
            txtInput1 = new TextBox { Location = new Point(20, 20), Width = 340 };
            txtInput2 = new TextBox { Location = new Point(20, 60), Width = 340 };
            
            // Метка результата
            lblResult = new Label { 
                Location = new Point(20, 100), 
                Width = 340, 
                Height = 60, 
                Text = "Результат: ",
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            // Кнопки операций
            btnAdd = new Button { Text = "+", Location = new Point(20, 180), Width = 75 };
            btnSub = new Button { Text = "-", Location = new Point(105, 180), Width = 75 };
            btnMul = new Button { Text = "*", Location = new Point(190, 180), Width = 75 };
            btnDiv = new Button { Text = "/", Location = new Point(275, 180), Width = 75 };

            // Кнопки управления
            btnClear = new Button { Text = "Сброс", Location = new Point(20, 230), Width = 160 };
            btnShowLog = new Button { Text = "Протокол (Лог)", Location = new Point(190, 230), Width = 160 };

            // Подписываемся на события клика
            btnAdd.Click += (s, e) => ExecuteOperation("+");
            btnSub.Click += (s, e) => ExecuteOperation("-");
            btnMul.Click += (s, e) => ExecuteOperation("*");
            btnDiv.Click += (s, e) => ExecuteOperation("/");
            btnClear.Click += btnReset_Click;
            btnShowLog.Click += btnShowLog_Click;

            // Добавляем элементы на форму
            this.Controls.Add(txtInput1);
            this.Controls.Add(txtInput2);
            this.Controls.Add(lblResult);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnSub);
            this.Controls.Add(btnMul);
            this.Controls.Add(btnDiv);
            this.Controls.Add(btnClear);
            this.Controls.Add(btnShowLog);
        }

        private void ExecuteOperation(string op)
        {
            try
            {
                // Валидация
                if (!BigNumber25.Validate(txtInput1.Text) || !BigNumber25.Validate(txtInput2.Text))
                {
                    string error = "Ошибка: Введите числа (до 25 знаков)";
                    MessageBox.Show(error);
                    Logger.LogError(error);
                    return;
                }

                var n1 = new BigNumber25(txtInput1.Text);
                var n2 = new BigNumber25(txtInput2.Text);
                BigNumber25 result = null;

                switch (op)
                {
                    case "+": result = n1 + n2; break;
                    case "-": result = n1 - n2; break;
                    case "*": result = n1 * n2; break;
                    case "/": 
                        if (txtInput2.Text == "0") throw new DivideByZeroException("Деление на ноль!");
                        result = n1 / n2; 
                        break;
                }

                lblResult.Text = "Результат:\n" + result.ToString();
                Logger.Log(txtInput1.Text, op, txtInput2.Text, result.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
                Logger.LogError(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtInput1.Clear();
            txtInput2.Clear();
            lblResult.Text = "Результат: ";
        }

        private void btnShowLog_Click(object sender, EventArgs e)
        {
            // Показываем содержимое файла лога
            MessageBox.Show(Logger.GetLog(), "Протокол работы");
        }
    }
}