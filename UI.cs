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
        private TextBox txtPower; // для степени
        private Button btnPower;
        private Button btnConvert;
        private ComboBox cmbComplexForm; // Выбор формы отображения
        private ComplexNumber lastComplexResult; 

        public CalculatorForm()
        {
            InitializeComponent();
        }

        private void UpdateUIState()
        {
            if (btnPower == null) return; // Защита от раннего вызова

            string mode = cmbMode.SelectedItem?.ToString() ?? "";

            // Комплексные кнопки
            bool isComplex = (mode == "Комплексные числа");
            // Включаем/выключаем элементы управления
            btnPower.Enabled = isComplex;
            btnConvert.Enabled = isComplex;
            cmbComplexForm.Enabled = isComplex;
            txtPower.Enabled = isComplex;

            // Кнопки операций
            bool isDate = (mode == "Даты");
            // Если это даты, то работают только кнопки "-" и "Сброс"
            btnAdd.Enabled = !isDate;
            btnMul.Enabled = !isDate;
            btnDiv.Enabled = !isDate;
            btnSub.Enabled = true; // Минус работает всегда

            // Опционально: меняем цвет фона, чтобы было нагляднее
            txtPower.BackColor = isComplex ? Color.White : Color.LightGray;

            // Очищаем результат при смене режима, чтобы не путать пользователя
            lblResult.Text = isDate ? "Введите две даты и нажмите '-'" : "Результат: ";
            lastComplexResult = null; 
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
            cmbMode.Items.AddRange(new string[] { "25-значные числа", "Десятичные дроби", "Комплексные числа", "Даты"});
            cmbMode.SelectedIndex = 0; // По умолчанию первый вариант
            this.Controls.Add(cmbMode);

            // ПОДПИСЫВАЕМСЯ НА СОБЫТИЕ СМЕНЫ ВЫБОРА
            cmbMode.SelectedIndexChanged += (s, e) => UpdateUIState();

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

            txtPower = new TextBox { Location = new Point(280, 280), Width = 40 };
            Label lblP = new Label { Text = "Степень:", Location = new Point(210, 285), Width = 70 };

            btnPower = new Button { Text = "В степень", Location = new Point(20, 280), Width = 85 };
            btnConvert = new Button { Text = "Сменить форму", Location = new Point(110, 280), Width = 100 };

            cmbComplexForm = new ComboBox { Location = new Point(20, 320), Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbComplexForm.Items.AddRange(new string[] { "Алгебраическая", "Тригонометрическая", "Экспоненциальная" });
            cmbComplexForm.SelectedIndex = 0;

            // Подписываемся на события клика
            btnAdd.Click += (s, e) => ExecuteOperation("+");
            btnSub.Click += (s, e) => ExecuteOperation("-");
            btnMul.Click += (s, e) => ExecuteOperation("*");
            btnDiv.Click += (s, e) => ExecuteOperation("/");
            btnPower.Click += (s, e) => ExecuteOperation("^");
            btnConvert.Click += (s, e) => ChangeResultForm();
            btnClear.Click += btnReset_Click;
            btnShowLog.Click += btnShowLog_Click;

            // ПОДПИСЫВАЕМСЯ НА СОБЫТИЕ СМЕНЫ ВЫБОРА
            cmbMode.SelectedIndexChanged += (s, e) => UpdateUIState();
            UpdateUIState();

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
            this.Controls.Add(txtPower);
            this.Controls.Add(lblP);
            this.Controls.Add(btnPower);
            this.Controls.Add(btnConvert);
            this.Controls.Add(cmbComplexForm);
        }

        private void ExecuteOperation(string op)
        {
            try
            {
                string resultStr = "";
                string mode = cmbMode.SelectedItem.ToString();

                if (op == "^" && mode != "Комплексные числа") return;

                if (mode == "Даты")
                {
                    if (op != "-")
                    {
                        MessageBox.Show("Для дат поддерживается только операция вычитания.");
                        return;
                    }
        
                    DateTime date1 = DateModule.Parse(txtInput1.Text);
                    DateTime date2 = DateModule.Parse(txtInput2.Text);
        
                    string resultText = DateModule.GetFullDifference(date1, date2);
                    lblResult.Text = resultText;
        
                    // Логируем (убираем переносы строк для файла)
                    Logger.Log(txtInput1.Text, "минус", txtInput2.Text, resultText.Replace("\n", " "));
                    return;
                }

                if (mode == "Комплексные числа")
                {
                    var c1 = ComplexNumber.Parse(txtInput1.Text);
                    ComplexNumber result = null;

                    if (op == "^") // Возведение в степень
                    {
                        int n = int.Parse(txtPower.Text);
                        result = c1.Power(n);
                    }
                    else
                    {
                        var c2 = ComplexNumber.Parse(txtInput2.Text);
                        switch (op)
                        {
                            case "+": result = c1 + c2; break;
                            case "-": result = c1 - c2; break;
                            case "*": result = c1 * c2; break;
                            case "/": result = c1 / c2; break;
                        }
                    }

                    lastComplexResult = result;
                    DisplayComplexResult();
                    Logger.Log(txtInput1.Text, op, txtInput2.Text, result.ToString(ComplexForm.Algebraic));
                    return;
                }

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

        private void ChangeResultForm()
        {
            if (lastComplexResult != null)
            {
                DisplayComplexResult();
            }
        }

        private void DisplayComplexResult()
        {
            ComplexForm form = (ComplexForm)cmbComplexForm.SelectedIndex;
            lblResult.Text = "Результат:\n" + lastComplexResult.ToString(form);
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

