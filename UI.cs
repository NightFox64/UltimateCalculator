using System;
using System.Collections;
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
        private Button btnFlip, btnNegate;
        private TextBox txtInput3;
        private Label lblA, lblB, lblC;
        private Button btnSolve;
        private Button btnTrig;
        private Button btnCalculateExpr;
        private ComboBox cmbFromBase, cmbToBase;
        private Label lblFrom, lblTo;
        private Button btnConvertNS;

        public CalculatorForm()
        {
            InitializeComponent();
        }

        private void UpdateUIState()
        {
            if (btnAdd == null) return; // Защита

            string mode = cmbMode.SelectedItem?.ToString() ?? "";

            // 1. Режимы, где нужны ВСЕ базовые операции (+, -, *, /)
            // Это всё, кроме Дат.
            bool standardOps = (mode != "Даты");
            btnAdd.Enabled = standardOps;
            btnMul.Enabled = standardOps;
            btnDiv.Enabled = standardOps;
            btnSub.Enabled = true; // Минус нужен всем (даже датам)

            // 2. Комплексные числа
            bool isComplex = (mode == "Комплексные числа");
            btnPower.Enabled = isComplex;
            btnConvert.Enabled = isComplex;
            cmbComplexForm.Enabled = isComplex;
            txtPower.Enabled = isComplex;

            // 3. Обыкновенные дроби
            bool isFraction = (mode == "Обыкновенные дроби");
            btnFlip.Enabled = isFraction;
            btnNegate.Enabled = isFraction;

            bool isQuadratic = (mode == "Квадратные уравнения");
            txtInput3.Visible = isQuadratic;
            lblC.Visible = isQuadratic;
            btnSolve.Visible = isQuadratic;
            // Для уравнений стандартные + - * / не нужны
            if (isQuadratic) {
                btnAdd.Enabled = btnSub.Enabled = btnMul.Enabled = btnDiv.Enabled = false;
            }

            bool isAngle = (mode == "Углы");
            btnTrig.Visible = isAngle;
            // Для углов умножение и деление обычно идет на ЧИСЛО, а не на другой угол.
            // Но для простоты оставим кнопки активными.

            bool isExpr = (mode == "Выражения со скобками");
            btnCalculateExpr.Visible = isExpr;

            if (isExpr)
            {
                txtInput2.Visible = false; // Второе поле не нужно
                lblB.Visible = false;
                // Выключаем стандартные кнопки, так как всё выражение в одном поле
                btnAdd.Enabled = btnSub.Enabled = btnMul.Enabled = btnDiv.Enabled = false;
                lblResult.Text = "Введите выражение в первое поле\n(например: (2+2)*2 )";
            }
            else
            {
                // Возвращаем видимость для других режимов (кроме Квадратных уравнений)
                if (mode != "Квадратные уравнения") 
                {
                    txtInput2.Visible = true;
                    lblB.Visible = true;
                }
            }

            bool isNS = (mode == "Системы счисления");
            lblFrom.Visible = cmbFromBase.Visible = isNS;
            lblTo.Visible = cmbToBase.Visible = isNS;
            btnConvertNS.Visible = isNS;

            if (isNS)
            {
                txtInput2.Visible = false; // Нам не нужно второе текстовое поле
                lblB.Visible = false;
                btnAdd.Enabled = btnSub.Enabled = btnMul.Enabled = btnDiv.Enabled = false;
                lblResult.Text = "Введите число в первое поле\nи выберите системы счисления";
            }

            lblResult.Text = "Результат: ";
        }
        private void InitializeComponent()
        {
            this.Size = new Size(400, 550);

            // Метка результата
            lblResult = new Label { 
                Location = new Point(20, 160), 
                Width = 340, 
                Height = 80, 
                Text = "Результат: ",
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            this.Text = "Комплексный калькулятор";

            // Выбор режима
            cmbMode = new ComboBox { Location = new Point(20, 5), Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbMode.Items.AddRange(new string[] { "25-значные числа", "Десятичные дроби", "Комплексные числа", "Даты", "Обыкновенные дроби", "Квадратные уравнения", "Углы", "Выражения со скобками", "Системы счисления" });
            cmbMode.SelectedIndex = 0; // По умолчанию первый вариант
            this.Controls.Add(cmbMode);

            // ПОДПИСЫВАЕМСЯ НА СОБЫТИЕ СМЕНЫ ВЫБОРА
            cmbMode.SelectedIndexChanged += (s, e) => UpdateUIState();

            // Сдвинь остальные элементы чуть ниже, если нужно, 
            // или просто убедись, что они не перекрывают друг друга
            txtInput1 = new TextBox { Location = new Point(30, 40), Width = 330 };
            txtInput2 = new TextBox { Location = new Point(30, 80), Width = 330 };
            txtInput3 = new TextBox { Location = new Point(30, 120), Width = 330 };

            lblA = new Label { Text = "a:", Location = new Point(5, 43), Width = 15 };
            lblB = new Label { Text = "b:", Location = new Point(5, 83), Width = 15 };
            lblC = new Label { Text = "c:", Location = new Point(5, 123), Width = 15 };

            // Кнопки операций
            btnAdd = new Button { Text = "+", Location = new Point(20, 250), Width = 75 };
            btnSub = new Button { Text = "-", Location = new Point(105, 250), Width = 75 };
            btnMul = new Button { Text = "*", Location = new Point(190, 250), Width = 75 };
            btnDiv = new Button { Text = "/", Location = new Point(275, 250), Width = 75 };
            btnFlip = new Button { Text = "1/x (Flip)", Location = new Point(20, 280), Width = 75 };
            btnNegate = new Button { Text = "+/-", Location = new Point(110, 280), Width = 75 };
            btnPower = new Button { Text = "В степень", Location = new Point(20, 310), Width = 85 };
            btnConvert = new Button { Text = "Сменить форму", Location = new Point(110, 310), Width = 100 };
            btnSolve = new Button { Text = "Найти корни", Location = new Point(230, 310), Width = 100 };
            btnTrig = new Button { Text = "Sin/Cos/Tan", Location = new Point(20, 380), Width = 160, Visible = false };
            btnCalculateExpr = new Button { Text = "Вычислить выражение", Location = new Point(20, 380), Width = 180, Visible = false };
            btnConvertNS = new Button { Text = "Перевести", Location = new Point(20, 380), Width = 160, Visible = false };

            // Кнопки управления
            btnClear = new Button { Text = "Сброс", Location = new Point(20, 410), Width = 160 };
            btnShowLog = new Button { Text = "Протокол (Лог)", Location = new Point(190, 410), Width = 160 };

            txtPower = new TextBox { Location = new Point(280, 280), Width = 40 };
            Label lblP = new Label { Text = "Степень:", Location = new Point(210, 285), Width = 70 };

            cmbComplexForm = new ComboBox { Location = new Point(20, 350), Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbComplexForm.Items.AddRange(new string[] { "Алгебраическая", "Тригонометрическая", "Экспоненциальная" });
            cmbComplexForm.SelectedIndex = 0;

            lblFrom = new Label { Text = "Из системы:", Location = new Point(20, 115), Width = 100, Visible = false };
            cmbFromBase = new ComboBox { Location = new Point(120, 122), Width = 60, DropDownStyle = ComboBoxStyle.DropDownList, Visible = false };
            lblTo = new Label { Text = "В систему:", Location = new Point(200, 115), Width = 100, Visible = false };
            cmbToBase = new ComboBox { Location = new Point(300, 122), Width = 60, DropDownStyle = ComboBoxStyle.DropDownList, Visible = false };
            var bases = new object[] { 2, 3, 8, 9, 10, 16 };
            cmbFromBase.Items.AddRange(bases);
            cmbToBase.Items.AddRange(bases);
            cmbFromBase.SelectedIndex = 4; // 10
            cmbToBase.SelectedIndex = 0; // 2

            // Подписываемся на события клика
            btnAdd.Click += (s, e) => ExecuteOperation("+");
            btnSub.Click += (s, e) => ExecuteOperation("-");
            btnMul.Click += (s, e) => ExecuteOperation("*");
            btnDiv.Click += (s, e) => ExecuteOperation("/");
            btnPower.Click += (s, e) => ExecuteOperation("^");
            btnConvert.Click += (s, e) => ChangeResultForm();
            btnClear.Click += btnReset_Click;
            btnShowLog.Click += btnShowLog_Click;
            btnFlip.Click += (s, e) => ExecuteFractionAction("flip");
            btnNegate.Click += (s, e) => ExecuteFractionAction("negate");
            btnSolve.Click += (s, e) => ExecuteOperation("solve");
            btnTrig.Click += (s, e) => ExecuteOperation("trig");
            btnCalculateExpr.Click += (s, e) => ExecuteOperation("eval_expr");
            btnConvertNS.Click += (s, e) => ExecuteOperation("convert_ns");


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
            this.Controls.Add(btnFlip);
            this.Controls.Add(btnNegate);
            this.Controls.Add(lblA);
            this.Controls.Add(lblB);
            this.Controls.Add(lblC);
            this.Controls.Add(txtInput3);
            this.Controls.Add(btnSolve);
            this.Controls.Add(btnTrig);
            this.Controls.Add(btnCalculateExpr);
            this.Controls.Add(lblFrom);
            this.Controls.Add(cmbFromBase);
            this.Controls.Add(lblTo);
            this.Controls.Add(cmbToBase);
            this.Controls.Add(btnConvertNS);
        }

        private void ExecuteFractionAction(string action)
        {
            try
            {
                var f1 = CommonFraction.Parse(txtInput1.Text);
                CommonFraction res = null;

                if (action == "flip") res = f1.Flip();
                if (action == "negate") res = f1.Negate();

                txtInput1.Text = res.ToString(); // Записываем результат обратно в первое поле
                lblResult.Text = "Действие выполнено: " + res.ToString();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ExecuteOperation(string op)
        {
            try
            {
                string resultStr = "";
                string mode = cmbMode.SelectedItem.ToString();

                if (op == "^" && mode != "Комплексные числа") return;

                switch (mode)
                {
                    case "Системы счисления":
                    {
                        if (op == "convert_ns")
                        {
                            string input = txtInput1.Text;
                            int fromB = int.Parse(cmbFromBase.SelectedItem.ToString());
                            int toB = int.Parse(cmbToBase.SelectedItem.ToString());
                    
                            resultStr = NumberSystemConverter.Convert(input, fromB, toB);
                        }
                        lblResult.Text = "Результат (" + mode + "):\n" + resultStr;
                        Logger.Log(txtInput1.Text, op, txtInput2.Text, resultStr);
                        return;     
                    }
    
                    case "Выражения со скобками":
                    {
                        if (op == "eval_expr")
                        {
                            string expr = txtInput1.Text;

                            // 1. Сначала проверяем
                            ExpressionModule.Validate(expr);

                            // 2. Если проверка прошла - считаем
                            resultStr = ExpressionModule.Evaluate(expr);
                        }
                        lblResult.Text = "Результат (" + mode + "):\n" + resultStr;
                        Logger.Log(txtInput1.Text, op, txtInput2.Text, resultStr);
                        return; 
                    }
                    case "Углы":
                    {
                        var ang1 = AngleModule.Parse(txtInput1.Text);
                        if (op == "trig")
                        {
                            resultStr = ang1.GetTrig();
                        }
                        else
                        {
                            // Арифметика
                            if (op == "+" || op == "-")
                            {
                                var ang2 = AngleModule.Parse(txtInput2.Text);
                                if (op == "+") resultStr = (ang1 + ang2).ToString();
                                else resultStr = (ang1 - ang2).ToString();
                            }
                            else // Умножение/деление угла на число
                            {
                                double val = double.Parse(txtInput2.Text);
                                if (op == "*") resultStr = (ang1 * val).ToString();
                                else resultStr = (ang1 / val).ToString();
                            }
                        }
                        lblResult.Text = "Результат (" + mode + "):\n" + resultStr;
                        Logger.Log(txtInput1.Text, op, txtInput2.Text, resultStr);
                        return;
                    }

                    case "Квадратные уравнения":
                    {
                        if (op == "solve")
                        {
                            double a = double.Parse(txtInput1.Text);
                            double b = double.Parse(txtInput2.Text);
                            double c = double.Parse(txtInput3.Text);

                            resultStr = QuadraticSolver.Solve(a, b, c);
                        }

                        lblResult.Text = "Результат (" + mode + "):\n" + resultStr;
                        Logger.Log(txtInput1.Text, op, txtInput2.Text, resultStr);
                        return;
                    }

                    case "Обыкновенные дроби":
                    {
                        var f1 = CommonFraction.Parse(txtInput1.Text);
                        var f2 = CommonFraction.Parse(txtInput2.Text);
                        CommonFraction res = null;

                        switch (op)
                        {
                            case "+": res = f1 + f2; break;
                            case "-": res = f1 - f2; break;
                            case "*": res = f1 * f2; break;
                            case "/": res = f1 / f2; break;
                        }

                        lblResult.Text = "Результат:\n" + res.ToString();
                        Logger.Log(txtInput1.Text, op, txtInput2.Text, res.ToString());
                        return;
                    }

                    case "Даты":
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
                        lblResult.Text = "Результат (" + mode + "):\n" + resultStr;
                        return;
                    }

                    case "Комплексные числа":
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

                    case "25-значные числа":
                    {
                        if (!BigNumber25.Validate(txtInput1.Text) || !BigNumber25.Validate(txtInput2.Text))
                            throw new Exception("Неверный формат 25-значного числа.");

                        var n1 = new BigNumber25(txtInput1.Text);
                        var n2 = new BigNumber25(txtInput2.Text);

                        if (op == "+") resultStr = (n1 + n2).ToString();
                        else if (op == "-") resultStr = (n1 - n2).ToString();
                        else if (op == "*") resultStr = (n1 * n2).ToString();
                        else if (op == "/") resultStr = (n1 / n2).ToString();

                        lblResult.Text = "Результат (" + mode + "):\n" + resultStr;
                        Logger.Log(txtInput1.Text, op, txtInput2.Text, resultStr);
                        return;
                    }

                    case "Десятичные дроби":
                    {
                        var d1 = new DecimalFraction(txtInput1.Text);
                        var d2 = new DecimalFraction(txtInput2.Text);

                        if (op == "+") resultStr = (d1 + d2).ToString();
                        else if (op == "-") resultStr = (d1 - d2).ToString();
                        else if (op == "*") resultStr = (d1 * d2).ToString();
                        else if (op == "/") resultStr = (d1 / d2).ToString();

                        lblResult.Text = "Результат (" + mode + "):\n" + resultStr;
                        Logger.Log(txtInput1.Text, op, txtInput2.Text, resultStr);
                        return;
                    }
                } 
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

