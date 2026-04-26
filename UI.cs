using System;
using System.Drawing;
using System.Windows.Forms;

namespace ComplexCalculator
{
    public class CalculatorForm : Form
    {
        private TextBox txtInput1, txtInput2;
        private Label lblInput1, lblInput2, lblResult;
        private Button btnAdd, btnSub, btnMul, btnDiv, btnClear, btnShowLog;
        private Button btnToDecimal, btnToRoman;
        private ComboBox cmbMode;

        public CalculatorForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Калькулятор";
            this.Size = new Size(420, 320);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            cmbMode = new ComboBox {
                Location = new Point(20, 15), Width = 360,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMode.Items.AddRange(new string[] { "Натуральные числа (до 25 знаков)", "Десятичные дроби", "Римские цифры" });
            cmbMode.SelectedIndex = 0;

            lblInput1 = new Label { Text = "Число 1:", Location = new Point(20, 55), Width = 70 };
            txtInput1 = new TextBox { Location = new Point(95, 52), Width = 285 };

            lblInput2 = new Label { Text = "Число 2:", Location = new Point(20, 90), Width = 70 };
            txtInput2 = new TextBox { Location = new Point(95, 87), Width = 285 };

            lblResult = new Label {
                Location = new Point(20, 125), Width = 360, Height = 60,
                Text = "Результат:",
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            btnAdd = new Button { Text = "+", Location = new Point(20, 195), Width = 75 };
            btnSub = new Button { Text = "−", Location = new Point(105, 195), Width = 75 };
            btnMul = new Button { Text = "×", Location = new Point(190, 195), Width = 75 };
            btnDiv = new Button { Text = "÷", Location = new Point(275, 195), Width = 75 };

            btnToDecimal = new Button { Text = "→ Десятичное", Location = new Point(20, 235), Width = 130, Visible = false };
            btnToRoman   = new Button { Text = "→ Римское",    Location = new Point(160, 235), Width = 130, Visible = false };

            btnClear   = new Button { Text = "Сброс",   Location = new Point(20, 235), Width = 160 };
            btnShowLog = new Button { Text = "Протокол", Location = new Point(190, 235), Width = 160 };

            cmbMode.SelectedIndexChanged += (s, e) => UpdateUI();

            btnAdd.Click += (s, e) => Calc("+");
            btnSub.Click += (s, e) => Calc("-");
            btnMul.Click += (s, e) => Calc("*");
            btnDiv.Click += (s, e) => Calc("/");
            btnToDecimal.Click += (s, e) => Calc("to_dec");
            btnToRoman.Click   += (s, e) => Calc("to_rom");
            btnClear.Click += (s, e) => { txtInput1.Clear(); txtInput2.Clear(); lblResult.Text = "Результат:"; };
            btnShowLog.Click += (s, e) => MessageBox.Show(Logger.GetLog(), "Протокол работы");

            foreach (var c in new System.Windows.Forms.Control[] {
                cmbMode, lblInput1, txtInput1, lblInput2, txtInput2, lblResult,
                btnAdd, btnSub, btnMul, btnDiv,
                btnToDecimal, btnToRoman, btnClear, btnShowLog
            }) this.Controls.Add(c);

            UpdateUI();
        }

        private void UpdateUI()
        {
            string mode = cmbMode.SelectedItem.ToString();
            bool isRoman = mode == "Римские цифры";

            // Кнопки конвертации — только для римских
            btnToDecimal.Visible = isRoman;
            btnToRoman.Visible   = isRoman;

            // Кнопки управления сдвигаем вниз если есть доп. кнопки
            int ctrlY = isRoman ? 275 : 235;
            btnClear.Location   = new Point(20, ctrlY);
            btnShowLog.Location = new Point(190, ctrlY);

            this.Height = isRoman ? 360 : 320;

            // Подсказки
            lblResult.Text = mode switch {
                "Натуральные числа (до 25 знаков)" => "Результат:",
                "Десятичные дроби"                 => "Результат:",
                "Римские цифры"                    => "Вводите римские (XIV) или арабские (14):",
                _ => "Результат:"
            };

            lblInput2.Visible = true;
            txtInput2.Visible = true;
        }

        private void Calc(string op)
        {
            try
            {
                string mode = cmbMode.SelectedItem.ToString();
                string result = "";

                switch (mode)
                {
                    case "Натуральные числа (до 25 знаков)":
                    {
                        if (!BigNumber25.Validate(txtInput1.Text) || !BigNumber25.Validate(txtInput2.Text))
                            throw new Exception("Введите целые числа не длиннее 25 знаков.");
                        var n1 = new BigNumber25(txtInput1.Text);
                        var n2 = new BigNumber25(txtInput2.Text);
                        result = op switch {
                            "+" => (n1 + n2).ToString(),
                            "-" => (n1 - n2).ToString(),
                            "*" => (n1 * n2).ToString(),
                            "/" => (n1 / n2).ToString(),
                            _ => ""
                        };
                        break;
                    }

                    case "Десятичные дроби":
                    {
                        var d1 = new DecimalFraction(txtInput1.Text);
                        var d2 = new DecimalFraction(txtInput2.Text);
                        result = op switch {
                            "+" => (d1 + d2).ToString(),
                            "-" => (d1 - d2).ToString(),
                            "*" => (d1 * d2).ToString(),
                            "/" => (d1 / d2).ToString(),
                            _ => ""
                        };
                        break;
                    }

                    case "Римские цифры":
                    {
                        if (op == "to_dec")
                        {
                            int val = RomanNumber.RomanToDecimal(txtInput1.Text);
                            result = val.ToString();
                        }
                        else if (op == "to_rom")
                        {
                            if (!int.TryParse(txtInput1.Text, out int val))
                                val = RomanNumber.RomanToDecimal(txtInput1.Text);
                            result = RomanNumber.DecimalToRoman(val);
                        }
                        else
                        {
                            int v1 = int.TryParse(txtInput1.Text, out int a) ? a : RomanNumber.RomanToDecimal(txtInput1.Text);
                            int v2 = int.TryParse(txtInput2.Text, out int b) ? b : RomanNumber.RomanToDecimal(txtInput2.Text);
                            int res = op switch {
                                "+" => v1 + v2,
                                "-" => v1 - v2,
                                "*" => v1 * v2,
                                "/" => v1 / v2,
                                _ => 0
                            };
                            result = $"{RomanNumber.DecimalToRoman(res)} ({res})";
                        }
                        break;
                    }
                }

                lblResult.Text = "Результат: " + result;
                Logger.Log(txtInput1.Text, op, txtInput2.Text, result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
                Logger.LogError(ex.Message);
            }
        }
    }
}
