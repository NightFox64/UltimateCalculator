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
        private Button btnMod, btnToDecimal, btnToRoman;

        public CalculatorForm()
        {
            InitializeComponent();
        }

        private void UpdateUIState()
        {
            if (btnAdd == null) return; // Защита от раннего вызова
        
            string mode = cmbMode.SelectedItem?.ToString() ?? "";
        
            // === ШАГ 1: СБРОС ВСЕГО В СТАНДАРТНОЕ СОСТОЯНИЕ ===
            // Поля ввода
            txtInput1.Visible = true;
            txtInput2.Visible = true;
            txtInput3.Visible = false;
            txtInput4.Visible = false;
        
            // Метки (Labels) - возвращаем стандартные имена
            lblA.Visible = true; lblA.Text = "Число 1:";
            lblB.Visible = true; lblB.Text = "Число 2:";
            lblC.Visible = false; lblC.Text = "Число 3:";
            lblLogBase2.Visible = false; // Метка для 4-го поля
        
            // Стандартные кнопки операций
            btnAdd.Enabled = true;
            btnSub.Enabled = true;
            btnMul.Enabled = true;
            btnDiv.Enabled = true;
        
            // Скрываем все спец. кнопки (по умолчанию)
            btnPower.Visible = btnConvert.Visible = cmbComplexForm.Visible = false;
            btnFlip.Visible = btnNegate.Visible = false;
            btnSolve.Visible = false;
            btnTrig.Visible = false;
            btnCalculateExpr.Visible = false;
            btnConvertNS.Visible = lblFrom.Visible = cmbFromBase.Visible = lblTo.Visible = cmbToBase.Visible = false;
            btnPowerLog.Visible = btnChangeBase.Visible = false;
            btnEvalPoly.Visible = false;
            btnSolveSystem.Visible = false;
            cmbRootMethod.Visible = btnFindRoot.Visible = false;
            cmbTimeUnit.Visible = btnTimeToFull.Visible = false;
            btnMod.Visible = btnToDecimal.Visible = btnToRoman.Visible = false;
            txtPower.Visible = false;
        
            // Подсказка по умолчанию
            lblResult.Text = "Результат: ";
        
            // === ШАГ 2: ВКЛЮЧАЕМ СПЕЦИФИКУ ДЛЯ КАЖДОГО РЕЖИМА ===
            switch (mode)
            {
                case "25-значные числа":
                case "Десятичные дроби":
                    // Здесь всё стандартно, ничего менять не нужно
                    break;
        
                case "Комплексные числа":
                    btnPower.Visible = btnConvert.Visible = cmbComplexForm.Visible = true;
                    btnPower.Enabled = btnConvert.Enabled = true; // Убеждаемся что активны
                    txtPower.Visible = true;
                    txtPower.Enabled = true;
                    lblResult.Text = "Формат: a+bi (например 1+i или 5-3i)";
                    break;
        
                case "Даты":
                    btnAdd.Enabled = btnMul.Enabled = btnDiv.Enabled = false;
                    lblResult.Text = "Форматы: dd.MM.yyyy, dd/MM/yyyy, MM/dd/yyyy.\nИспользуйте '-' для разницы.";
                    break;
        
                case "Обыкновенные дроби":
                    btnFlip.Visible = btnNegate.Visible = true;
                    btnFlip.Enabled = btnNegate.Enabled = true;
                    lblResult.Text = "Формат: числитель/знаменатель (например 1/2)";
                    break;
        
                case "Квадратные уравнения":
                    txtInput3.Visible = true; lblC.Visible = true; btnSolve.Visible = true;
                    lblA.Text = "a:"; lblB.Text = "b:"; lblC.Text = "c:";
                    btnAdd.Enabled = btnSub.Enabled = btnMul.Enabled = btnDiv.Enabled = false;
                    lblResult.Text = "Введите коэффициенты и нажмите 'Найти корни'";
                    break;
        
                case "Углы":
                    btnTrig.Visible = true;
                    lblResult.Text = "Формат: Градусы Минуты Секунды (через пробел)";
                    break;
        
                case "Выражения со скобками":
                    txtInput2.Visible = false; lblB.Visible = false;
                    btnCalculateExpr.Visible = true;
                    btnAdd.Enabled = btnSub.Enabled = btnMul.Enabled = btnDiv.Enabled = false;
                    lblResult.Text = "Пример: (2+2)*2. Вводите в первое поле.";
                    break;
        
                case "Системы счисления":
                    txtInput2.Visible = false; lblB.Visible = false;
                    lblFrom.Visible = cmbFromBase.Visible = lblTo.Visible = cmbToBase.Visible = btnConvertNS.Visible = true;
                    btnAdd.Enabled = btnSub.Enabled = btnMul.Enabled = btnDiv.Enabled = false;
                    break;
        
                case "Логарифмы":
                    txtInput3.Visible = txtInput4.Visible = true;
                    lblC.Visible = true; lblLogBase2.Visible = true;
                    btnPowerLog.Visible = btnChangeBase.Visible = true;
                    lblA.Text = "Арг 1:"; lblB.Text = "Осн 1:"; 
                    lblC.Text = "Арг 2 / Пар:"; lblLogBase2.Text = "Осн 2:";
                    break;
        
                case "Многочлены":
                    btnEvalPoly.Visible = true;
                    btnDiv.Enabled = false;
                    lblResult.Text = "Коэфф. через пробел (a0 a1 a2...). Поле 2: X или Степень.";
                    break;
        
                case "Система уравнений":
                    txtInput3.Visible = true; lblC.Visible = true; btnSolveSystem.Visible = true;
                    lblA.Text = "Ур 1:"; lblB.Text = "Ур 2:"; lblC.Text = "Ур 3:";
                    btnAdd.Enabled = btnSub.Enabled = btnMul.Enabled = btnDiv.Enabled = false;
                    break;
        
                case "Поиск корней (f(x)=0)":
                    txtInput3.Visible = true; lblC.Visible = true;
                    cmbRootMethod.Visible = btnFindRoot.Visible = true;
                    lblA.Text = "Ф-ция:"; lblB.Text = "Интервал:"; lblC.Text = "Точность:";
                    btnAdd.Enabled = btnSub.Enabled = btnMul.Enabled = btnDiv.Enabled = false;
                    break;
        
                case "Интервалы времени":
                    cmbTimeUnit.Visible = btnTimeToFull.Visible = true;
                    btnMul.Enabled = btnDiv.Enabled = false;
                    break;
        
                case "Римские цифры":
                    btnMod.Visible = btnToDecimal.Visible = btnToRoman.Visible = true;
                    lblResult.Text = "Вводите числа (X, V, I...) или (1, 5, 10...)";
                    break;
            }
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
            cmbMode.Items.AddRange(new string[] { "25-значные числа", "Десятичные дроби", "Комплексные числа", "Даты", "Обыкновенные дроби", "Квадратные уравнения", "Углы", "Выражения со скобками", "Системы счисления", "Логарифмы", "Многочлены", "Система уравнений", "Поиск корней (f(x)=0)", "Интервалы времени", "Римские цифры" });
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
            btnConvert = new Button { Text = "Сменить форму", Location = new Point(110, 350), Width = 110 };
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
            btnMod = new Button { Text = "mod (%)", Location = new Point(240, 420), Width = 75, Visible = false };
            btnToDecimal = new Button { Text = "В 10-тичную", Location = new Point(20, 420), Width = 100, Visible = false };
            btnToRoman = new Button { Text = "В Римскую", Location = new Point(130, 420), Width = 100, Visible = false };

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
            btnMod.Click += (s, e) => ExecuteOperation("%");
            btnToDecimal.Click += (s, e) => ExecuteOperation("to_dec");
            btnToRoman.Click += (s, e) => ExecuteOperation("to_rom");


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
            this.Controls.Add(btnMod);
            this.Controls.Add(btnToDecimal);
            this.Controls.Add(btnToRoman);
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
                    case "Римские цифры":
                    {
                        if (op == "to_dec")
                        {
                            int val = RomanNumber.RomanToDecimal(txtInput1.Text);
                            resultStr = $"Десятичное значение: {val}";
                        }
                        else if (op == "to_rom")
                        {
                            if (int.TryParse(txtInput1.Text, out int val))
                            {
                                resultStr = $"Римское значение: {RomanNumber.DecimalToRoman(val)}";
                            }
                        }
                        else
                        {
                            // Парсим ввод (можно вводить и римские, и обычные числа)
                            int v1 = int.TryParse(txtInput1.Text, out int n1) ? n1 : RomanNumber.RomanToDecimal(txtInput1.Text);
                            int v2 = int.TryParse(txtInput2.Text, out int n2) ? n2 : RomanNumber.RomanToDecimal(txtInput2.Text);
                            int resVal = 0;
                            switch (op)
                            {
                                case "+": resVal = v1 + v2; break;
                                case "-": resVal = v1 - v2; break;
                                case "*": resVal = v1 * v2; break;
                                case "/": 
                                    int div = v1 / v2;
                                    int rem = v1 % v2;
                                    resultStr = $"Целое: {RomanNumber.DecimalToRoman(div)} ({div})\nОстаток: {RomanNumber.DecimalToRoman(rem)} ({rem})";
                                    break;
                            }
                            if (op != "/") resultStr = RomanNumber.DecimalToRoman(resVal) + $" ({resVal})";
                        }
                        lblResult.Text = "Результат (" + mode + "):\n" + resultStr;
                        Logger.Log(txtInput1.Text, op, txtInput2.Text, resultStr);
                        return;
                    }
    
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

