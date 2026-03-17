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
        private TextBox txtInput4;
        private Label lblLogBase1, lblLogBase2, lblArg1, lblArg2;
        private Button btnPowerLog, btnChangeBase;
        private Button btnEvalPoly;
        private Button btnSolveSystem;
        private ComboBox cmbRootMethod;
        private Button btnFindRoot;
        private ComboBox cmbTimeUnit; // Выбор: Часы, Минуты, Секунды
        private Button btnTimeToFull; // Кнопка перевода в формат Ч:М:С

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

            bool isLog = (mode == "Логарифмы");

            // Управляем всеми полями
            lblArg1.Visible = isLog;
            lblLogBase1.Visible = isLog;
            lblArg2.Visible = isLog || isQuadratic; // lblArg2 используем как lblC для уравнений
            lblLogBase2.Visible = isLog;
            txtInput3.Visible = isLog || isQuadratic;
            txtInput4.Visible = isLog;
            btnPowerLog.Visible = isLog;
            btnChangeBase.Visible = isLog;
            
            if (isLog) {
                btnAdd.Enabled = btnSub.Enabled = btnMul.Enabled = btnDiv.Enabled = true;
                lblResult.Text = "Поля: 1-Арг1, 2-Осн1, 3-Арг2/Параметр, 4-Осн2";
            }

            bool isPoly = (mode == "Многочлены");
            btnEvalPoly.Visible = isPoly;

            if (isPoly) {
                lblResult.Text = "Вводите коэфф. через пробел (a0 a1 a2...)\nВ поле 2 вводите X или степень n";
                btnAdd.Enabled = btnSub.Enabled = btnMul.Enabled = true;
                btnDiv.Enabled = false; // Деление многочленов - сложная операция, часто опускается
            }

            bool isSystem = (mode == "Система уравнений");
            btnSolveSystem.Visible = isSystem;

            if (isSystem) {
                txtInput3.Visible = true; // Показываем третье поле
                lblA.Text = "1:"; lblB.Text = "2:"; lblC.Text = "3:";
                lblA.Visible = lblB.Visible = lblC.Visible = true;
                lblResult.Text = "Введите коэффициенты a b c d через пробел\nв каждой из трех строк.";
                btnAdd.Enabled = btnSub.Enabled = btnMul.Enabled = btnDiv.Enabled = false;
            }

            bool isRoot = (mode == "Поиск корней (f(x)=0)");
            cmbRootMethod.Visible = btnFindRoot.Visible = isRoot;
            txtInput3.Visible = isRoot;
            lblC.Visible = isRoot;

            if (isRoot) {
                lblA.Text = "Ф:"; 
                lblB.Text = "И:"; // Для Ньютона берем только 'a' как x0
                lblC.Text = "Точность (eps):";
                lblResult.Text = "Пример: x*x - 2 = 0\nИнтервал: 1 2\nТочность: 0,001";
            }

            bool isTime = (mode == "Интервалы времени");
            cmbTimeUnit.Visible = btnTimeToFull.Visible = isTime;

            if (isTime) {
                lblResult.Text = "Введите число в поле 1 (и 2 для суммы/разности)\nи выберите единицы измерения";
                btnAdd.Enabled = btnSub.Enabled = true;
                btnMul.Enabled = btnDiv.Enabled = false;
            }

            lblResult.Text = "Результат: ";
        }
        private void InitializeComponent()
        {
            this.Size = new Size(400, 550);

            // Метка результата
            lblResult = new Label { 
                Location = new Point(20, 200), 
                Width = 340, 
                Height = 80, 
                Text = "Результат: ",
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            this.Text = "Комплексный калькулятор";

            // Выбор режима
            cmbMode = new ComboBox { Location = new Point(20, 5), Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbMode.Items.AddRange(new string[] { "25-значные числа", "Десятичные дроби", "Комплексные числа", "Даты", "Обыкновенные дроби", "Квадратные уравнения", "Углы", "Выражения со скобками", "Системы счисления", "Логарифмы", "Многочлены", "Система уравнений", "Поиск корней (f(x)=0)", "Интервалы времени" });
            cmbMode.SelectedIndex = 0; // По умолчанию первый вариант
            this.Controls.Add(cmbMode);

            // ПОДПИСЫВАЕМСЯ НА СОБЫТИЕ СМЕНЫ ВЫБОРА
            cmbMode.SelectedIndexChanged += (s, e) => UpdateUIState();

            // Сдвинь остальные элементы чуть ниже, если нужно, 
            // или просто убедись, что они не перекрывают друг друга
            txtInput1 = new TextBox { Location = new Point(30, 40), Width = 330 };
            txtInput2 = new TextBox { Location = new Point(30, 80), Width = 330 };
            txtInput3 = new TextBox { Location = new Point(30, 120), Width = 330 };
            txtInput4 = new TextBox { Location = new Point(30, 160), Width = 330, Visible = false };

            lblA = new Label { Text = "a:", Location = new Point(5, 43), Width = 15 };
            lblB = new Label { Text = "b:", Location = new Point(5, 83), Width = 15 };
            lblC = new Label { Text = "c:", Location = new Point(5, 123), Width = 15 };

            // Подписи для логарифмов
            lblArg1 = new Label { Text = "Арг1:", Location = new Point(360, 43), Width = 80, Visible = false };
            lblLogBase1 = new Label { Text = "Осн1:", Location = new Point(360, 83), Width = 80, Visible = false };
            lblArg2 = new Label { Text = "Арг.2 / Степень / Нов.Осн:", Location = new Point(360, 123), Width = 150, Visible = false };
            lblLogBase2 = new Label { Text = "Осн2:", Location = new Point(360, 163), Width = 80, Visible = false };

            // Кнопки операций
            btnAdd = new Button { Text = "+", Location = new Point(20, 290), Width = 75 };
            btnSub = new Button { Text = "-", Location = new Point(105, 290), Width = 75 };
            btnMul = new Button { Text = "*", Location = new Point(190, 290), Width = 75 };
            btnDiv = new Button { Text = "/", Location = new Point(275, 290), Width = 75 };
            btnFlip = new Button { Text = "1/x (Flip)", Location = new Point(20, 320), Width = 75 };
            btnNegate = new Button { Text = "+/-", Location = new Point(110, 320), Width = 75 };
            btnPower = new Button { Text = "В степень", Location = new Point(20, 350), Width = 85 };
            btnConvert = new Button { Text = "Сменить форму", Location = new Point(110, 350), Width = 100 };
            btnSolve = new Button { Text = "Найти корни", Location = new Point(230, 350), Width = 100 };
            btnTrig = new Button { Text = "Sin/Cos/Tan", Location = new Point(20, 420), Width = 160, Visible = false };
            btnCalculateExpr = new Button { Text = "Вычислить выражение", Location = new Point(20, 420), Width = 180, Visible = false };
            btnConvertNS = new Button { Text = "Перевести", Location = new Point(20, 420), Width = 160, Visible = false };
            btnPowerLog = new Button { Text = "В степень", Location = new Point(20, 420), Width = 100, Visible = false };
            btnChangeBase = new Button { Text = "Сменить осн.", Location = new Point(130, 420), Width = 100, Visible = false };
            btnEvalPoly = new Button { Text = "Значение в точке X", Location = new Point(20, 420), Width = 180, Visible = false };
            btnSolveSystem = new Button { Text = "Решить систему 3х3", Location = new Point(20, 420), Width = 180, Visible = false };
            btnFindRoot = new Button { Text = "Найти корень", Location = new Point(210, 420), Width = 120, Visible = false };
            btnTimeToFull = new Button { Text = "В полный формат", Location = new Point(120, 420), Width = 150, Visible = false };

            // Кнопки управления
            btnClear = new Button { Text = "Сброс", Location = new Point(20, 460), Width = 160 };
            btnShowLog = new Button { Text = "Протокол (Лог)", Location = new Point(190, 460), Width = 160 };

            txtPower = new TextBox { Location = new Point(280, 320), Width = 40 };
            Label lblP = new Label { Text = "Степень:", Location = new Point(210, 325), Width = 70 };

            cmbComplexForm = new ComboBox { Location = new Point(20, 390), Width = 340, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbComplexForm.Items.AddRange(new string[] { "Алгебраическая", "Тригонометрическая", "Экспоненциальная" });
            cmbComplexForm.SelectedIndex = 0;

            lblFrom = new Label { Text = "Из системы:", Location = new Point(20, 155), Width = 100, Visible = false };
            cmbFromBase = new ComboBox { Location = new Point(120, 162), Width = 60, DropDownStyle = ComboBoxStyle.DropDownList, Visible = false };
            lblTo = new Label { Text = "В систему:", Location = new Point(200, 155), Width = 100, Visible = false };
            cmbToBase = new ComboBox { Location = new Point(300, 162), Width = 60, DropDownStyle = ComboBoxStyle.DropDownList, Visible = false };
            var bases = new object[] { 2, 3, 8, 9, 10, 16 };
            cmbFromBase.Items.AddRange(bases);
            cmbToBase.Items.AddRange(bases);
            cmbFromBase.SelectedIndex = 4; // 10
            cmbToBase.SelectedIndex = 0; // 2

            cmbRootMethod = new ComboBox { Location = new Point(20, 420), Width = 180, Visible = false, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbRootMethod.Items.AddRange(new string[] { "Половинного деления", "Метод Ньютона", "Метод итераций" });
            cmbRootMethod.SelectedIndex = 0;

            cmbTimeUnit = new ComboBox { Location = new Point(20, 420), Width = 80, Visible = false, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTimeUnit.Items.AddRange(new string[] { "Часы", "Минуты", "Секунды" });
            cmbTimeUnit.SelectedIndex = 2; // По умолчанию секунды

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
            btnPowerLog.Click += (s, e) => ExecuteOperation("log_pow");
            btnChangeBase.Click += (s, e) => ExecuteOperation("log_base");
            btnEvalPoly.Click += (s, e) => ExecuteOperation("poly_eval");
            btnSolveSystem.Click += (s, e) => ExecuteOperation("solve_system");
            btnFindRoot.Click += (s, e) => ExecuteOperation("find_root");
            btnTimeToFull.Click += (s, e) => ExecuteOperation("time_to_full");


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
            this.Controls.Add(lblArg1);
            this.Controls.Add(lblLogBase1);
            this.Controls.Add(lblArg2);
            this.Controls.Add(lblLogBase2);
            this.Controls.Add(txtInput4);
            this.Controls.Add(btnPowerLog);
            this.Controls.Add(btnChangeBase);
            this.Controls.Add(btnEvalPoly);
            this.Controls.Add(btnSolveSystem);
            this.Controls.Add(cmbRootMethod);
            this.Controls.Add(btnFindRoot);
            this.Controls.Add(cmbTimeUnit);
            this.Controls.Add(btnTimeToFull);
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
                    case "Интервалы времени":
                    {
                        string unit = cmbTimeUnit.Text;
                        var t1 = TimeInterval.Parse(txtInput1.Text, unit);

                        if (op == "time_to_full")
                        {
                            resultStr = $"Преобразовано:\n{t1.ToString()}\nВсего минут: {t1.ToMinutes():F2}\nВсего часов: {t1.ToHours():F2}";
                        }
                        else
                        {
                            var t2 = TimeInterval.Parse(txtInput2.Text, unit);
                            if (op == "+") resultStr = (t1 + t2).ToString();
                            if (op == "-") resultStr = (t1 - t2).ToString();
                        }
                        lblResult.Text = "Результат (" + mode + "):\n" + resultStr;
                        Logger.Log(txtInput1.Text, op, txtInput2.Text, resultStr);
                        return;
                    }
    
                    case "Поиск корней (f(x)=0)":
                    {
                        if (op == "find_root")
                        {
                            string func = txtInput1.Text;
                            double eps = double.Parse(txtInput3.Text);
                            
                            // Парсим интервал [a, b]
                            var interval = txtInput2.Text.Split(' ').Select(double.Parse).ToArray();
                            double a = interval[0];
                            double b = interval.Length > 1 ? interval[1] : a;
                    
                            double root = 0;
                            string method = cmbRootMethod.Text;
                    
                            if (method == "Половинного деления") root = RootFinder.Bisection(func, a, b, eps);
                            else if (method == "Метод Ньютона") root = RootFinder.Newton(func, a, eps);
                            else root = RootFinder.Iteration(func, a, eps);
                    
                            resultStr = $"Корень уравнения:\nx = {root:F6}\nМетод: {method}";
                        }
                        lblResult.Text = "Результат (" + mode + "):\n" + resultStr;
                        Logger.Log(txtInput1.Text, op, txtInput2.Text, resultStr);
                        return;
                    }
    
                    case "Система уравнений":
                    {
                        if (op == "solve_system")
                        {
                            // Парсим каждую строку в массив чисел
                            var row1 = txtInput1.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
                            var row2 = txtInput2.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
                            var row3 = txtInput3.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();

                            resultStr = SystemSolver.Solve(row1, row2, row3);
                        }
                        lblResult.Text = "Результат (" + mode + "):\n" + resultStr;
                        Logger.Log(txtInput1.Text, op, txtInput2.Text, resultStr);
                        return;
                    }
    
                    case "Многочлены":
                    {
                        var p1 = Polynomial.Parse(txtInput1.Text);
    
                        if (op == "poly_eval") {
                            double x = double.Parse(txtInput2.Text);
                            resultStr = $"P({x}) = {p1.Evaluate(x)}";
                        }
                        else if (op == "^") {
                            int n = int.Parse(txtInput2.Text);
                            resultStr = p1.Power(n).ToString();
                        }
                        else {
                            var p2 = Polynomial.Parse(txtInput2.Text);
                            if (op == "+") resultStr = (p1 + p2).ToString();
                            if (op == "-") resultStr = (p1 - p2).ToString();
                            if (op == "*") resultStr = (p1 * p2).ToString();
                        }
                        lblResult.Text = "Результат (" + mode + "):\n" + resultStr;
                        Logger.Log(txtInput1.Text, op, txtInput2.Text, resultStr);
                        return;
                    }

                    case "Логарифмы":
                    {
                        var log1 = new LogNumber(double.Parse(txtInput1.Text), double.Parse(txtInput2.Text));
    
                        if (op == "log_pow") {
                            double p = double.Parse(txtInput3.Text);
                            double val = Math.Pow(log1.GetValue(), p);
                            resultStr = $"({log1.ToString()})^{p} = {val:F4}";
                        }
                        else if (op == "log_base") {
                            double newB = double.Parse(txtInput3.Text);
                            var newLog = log1.ChangeBase(newB);
                            resultStr = $"Переход к основанию {newB}:\n{newLog.ToString()}";
                        }
                        else {
                            // Арифметика между двумя логарифмами
                            var log2 = new LogNumber(double.Parse(txtInput3.Text), double.Parse(txtInput4.Text));
                            double v1 = log1.GetValue();
                            double v2 = log2.GetValue();
                            double res = 0;

                            switch (op) {
                                case "+": res = v1 + v2; break;
                                case "-": res = v1 - v2; break;
                                case "*": res = v1 * v2; break;
                                case "/": res = v1 / v2; break;
                            }
                            resultStr = $"{v1:F4} {op} {v2:F4} = {res:F4}";
                        }
                        lblResult.Text = "Результат (" + mode + "):\n" + resultStr;
                        Logger.Log(txtInput1.Text, op, txtInput2.Text, resultStr);
                        return;
                    }
    
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

