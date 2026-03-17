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

            this.Text = "Комплексный калькулятор";

            // Выбор режима
            cmbMode = new ComboBox { Location = new Point(20, 5), Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbMode.Items.AddRange(new string[] { "25-значные числа", "Десятичные дроби" });
            cmbMode.SelectedIndex = 0; // По умолчанию первый вариант
            this.Controls.Add(cmbMode);

            // Сдвинь остальные элементы чуть ниже, если нужно, 
            // или просто убедись, что они не перекрывают друг друга
            txtInput1 = new TextBox { Location = new Point(20, 40), Width = 340 };
            txtInput2 = new TextBox { Location = new Point(20, 80), Width = 340 };

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
                string resultStr = "";
                string mode = cmbMode.SelectedItem.ToString();
        
                if (mode == "25-значные числа")
                {
                    if (!BigNumber25.Validate(txtInput1.Text) || !BigNumber25.Validate(txtInput2.Text))
                        throw new Exception("Неверный формат 25-значного числа.");
        
                    var n1 = new BigNumber25(txtInput1.Text);
                    var n2 = new BigNumber25(txtInput2.Text);
                    
                    if (op == "+") resultStr = (n1 + n2).ToString();
                    else if (op == "-") resultStr = (n1 - n2).ToString();
                    else if (op == "*") resultStr = (n1 * n2).ToString();
                    else if (op == "/") resultStr = (n1 / n2).ToString();
                }
                else if (mode == "Десятичные дроби")
                {
                    var d1 = new DecimalFraction(txtInput1.Text);
                    var d2 = new DecimalFraction(txtInput2.Text);
        
                    if (op == "+") resultStr = (d1 + d2).ToString();
                    else if (op == "-") resultStr = (d1 - d2).ToString();
                    else if (op == "*") resultStr = (d1 * d2).ToString();
                    else if (op == "/") resultStr = (d1 / d2).ToString();
                }
        
                lblResult.Text = "Результат (" + mode + "):\n" + resultStr;
                Logger.Log(txtInput1.Text, op, txtInput2.Text, resultStr);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
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