using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace idz9_math_logic
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string EvaluateLogicalExpression(string expression)
        {
            // Удаляем все пробелы для упрощения анализа
            expression = expression.Replace(" ", "");

            // Проверяем корректность выражения
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentException("Выражение не может быть пустым.");

            // Заменяем 1 и 0 на true и false для упрощения вычислений
            expression = expression.Replace("1", "true").Replace("0", "false");

            string result = EvaluateExpression(expression).ToString();

            result.Replace("true", "1").Replace("True", "1").Replace("false", "0").Replace("False", "0");

            // Рекурсивно вычисляем выражение
            return result;
        }

        private bool EvaluateExpression(string expression)
        {
            // Обработка скобок
            if (expression.Contains('('))
            {
                int openBracket = expression.LastIndexOf('(');
                int closeBracket = expression.IndexOf(')', openBracket);

                if (closeBracket == -1)
                    throw new ArgumentException("Неверное выражение: отсутствует закрывающая скобка.");

                string subExpression = expression.Substring(openBracket + 1, closeBracket - openBracket - 1);
                bool subResult = EvaluateExpression(subExpression);
                string newExpression = expression.Substring(0, openBracket) + subResult.ToString().ToLower() + expression.Substring(closeBracket + 1);
                return EvaluateExpression(newExpression);
            }

            // Обработка NOT
            if (expression.Contains('!'))
            {
                int notIndex = expression.IndexOf('!');
                bool operand = EvaluateExpression(expression.Substring(notIndex + 1));
                return !operand;
            }

            // Обработка эквивалентности (== или ↔)
            if (expression.Contains("==") || expression.Contains("↔"))
            {
                string op = expression.Contains("==") ? "==" : "↔";
                int opIndex = expression.IndexOf(op);
                bool left = EvaluateExpression(expression.Substring(0, opIndex));
                bool right = EvaluateExpression(expression.Substring(opIndex + op.Length));
                return left == right;
            }

            // Обработка импликации (=> или →)
            if (expression.Contains("=>") || expression.Contains("→"))
            {
                string op = expression.Contains("=>") ? "=>" : "→";
                int opIndex = expression.IndexOf(op);
                bool left = EvaluateExpression(expression.Substring(0, opIndex));
                bool right = EvaluateExpression(expression.Substring(opIndex + op.Length));
                return !left || right; // Импликация: A → B эквивалентно !A || B
            }

            // Обработка AND
            if (expression.Contains("&&"))
            {
                int andIndex = expression.IndexOf("&&");
                bool left = EvaluateExpression(expression.Substring(0, andIndex));
                bool right = EvaluateExpression(expression.Substring(andIndex + 2));
                return left && right;
            }

            // Обработка OR
            if (expression.Contains("||"))
            {
                int orIndex = expression.IndexOf("||");
                bool left = EvaluateExpression(expression.Substring(0, orIndex));
                bool right = EvaluateExpression(expression.Substring(orIndex + 2));
                return left || right;
            }

            // Если выражение состоит из одного значения
            if (bool.TryParse(expression, out bool value))
                return value;

            throw new ArgumentException("Неверное выражение: неизвестный формат.");
        }

        
        // Функция для получения всех переменых из выражения
        // Переменные обозначаются только большими английскими буквами
        private List<string> ExtractVariables(string expression)
        {
            // Ищем все уникальные переменные (A, B, C и т.д.)
            var variables = new HashSet<string>();
            foreach (var ch in expression)
            {
                if (char.IsLetter(ch) && char.IsUpper(ch)) // Предполагаем, что переменные — это заглавные буквы
                {
                    variables.Add(ch.ToString());
                }
            }
            return variables.OrderBy(v => v).ToList();
        }

        // Класс для хранения строки для таблицы истинности
        public class Str1()
        {
            public string first { get; set; } = "";
            public string second {  get; set; } = "";
            public string third {  get; set; } = "";
            public string fourth {  get; set; } = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string expression = textBoxE2.Text;

            // Только x, y и z 
            List<string> expression_list = new List<string>();
            List<List<string>> table_list = new List<List<string>>();
            List<string> table_list1 = new List<string>();

            List<string> variables = new List<string>();
            variables = ExtractVariables(expression);

            // Если в выражении две переменные
            if (variables.Count == 2)
            {
                MessageBox.Show("Две переменные в выражении");
                List<Str1> sttttt = new List<Str1>();

                // 0 0
                string exp1 = expression.Replace('A', '0');
                exp1 = exp1.Replace('B', '0');
                Str1 str11 = new Str1 { first = "0", second = "0", fourth = EvaluateLogicalExpression(exp1).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str11);

                // 0 1
                string exp2 = expression.Replace('A', '0');
                exp2 = exp2.Replace('B', '1');
                Str1 str22 = new Str1 { first = "0", second = "1", fourth = EvaluateLogicalExpression(exp2).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str22);

                // 1 0 
                string exp3 = expression.Replace('A', '1');
                exp3 = exp3.Replace('B', '0');
                Str1 str33 = new Str1 { first = "1", second = "0", fourth = EvaluateLogicalExpression(exp3).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str33);

                // 1 1
                string exp4 = expression.Replace('A', '1');
                exp4 = exp4.Replace('B', '1');
                Str1 str44 = new Str1 { first = "1", second = "1", fourth = EvaluateLogicalExpression(exp4).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str44);

                TruthTableDataGrid.ItemsSource = sttttt;
            }
            // Если в выражении три переменные
            else if (variables.Count == 3)
            {
                List<Str1> sttttt = new List<Str1>();
                MessageBox.Show("Три переменные в выражении");
                // 0 0 0
                string exp1 = expression.Replace('A', '0');
                exp1 = exp1.Replace('B', '0');
                exp1 = exp1.Replace('C', '0');
                Str1 str11 = new Str1 { first = "0", second = "0", third = "0", fourth = EvaluateLogicalExpression(exp1).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str11);

                // 0 0 1
                List<string> vr2 = new List<string>();
                string exp2 = expression.Replace('A', '1');
                exp2 = exp2.Replace('B', '0');
                exp2 = exp2.Replace('C', '0');
                Str1 str22 = new Str1 { first = "1", second = "0", third = "0", fourth = EvaluateLogicalExpression(exp2).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str22);

                // 0 1 0
                List<string> vr3 = new List<string>();
                string exp3 = expression.Replace('A', '0');
                exp3 = exp3.Replace('B', '1');
                exp3 = exp3.Replace('C', '0');
                Str1 str33 = new Str1 { first = "0", second = "1", third = "0", fourth = EvaluateLogicalExpression(exp3).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str33);

                // 1 0 0
                List<string> vr4 = new List<string>();
                string exp4 = expression.Replace('A', '0');
                exp4 = exp4.Replace('B', '0');
                exp4 = exp4.Replace('C', '1');
                Str1 str44 = new Str1 { first = "0", second = "0", third = "1", fourth = EvaluateLogicalExpression(exp4).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str44);

                // 1 1 0
                List<string> vr5 = new List<string>();
                string exp5 = expression.Replace('A', '1');
                exp5 = exp5.Replace('B', '1');
                exp5 = exp5.Replace('C', '0');
                Str1 str55 = new Str1 { first = "1", second = "1", third = "0", fourth = EvaluateLogicalExpression(exp5).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str55);

                // 1 0 1
                List<string> vr6 = new List<string>();
                string exp6 = expression.Replace('A', '1');
                exp6 = exp6.Replace('B', '0');
                exp6 = exp6.Replace('C', '1');
                Str1 str66 = new Str1 { first = "1", second = "0", third = "1", fourth = EvaluateLogicalExpression(exp6).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str66);

                // 0 1 1
                List<string> vr7 = new List<string>();
                string exp7 = expression.Replace('A', '0');
                exp7 = exp7.Replace('B', '1');
                exp7 = exp7.Replace('C', '1');
                Str1 str77 = new Str1 { first = "0", second = "1", third = "1", fourth = EvaluateLogicalExpression(exp7).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str77);

                // 1 1 1
                List<string> vr8 = new List<string>();
                string exp8 = expression.Replace('A', '1');
                exp8 = exp8.Replace('B', '1');
                exp8 = exp8.Replace('C', '1');
                Str1 str88 = new Str1 { first = "1", second = "1", third = "1", fourth = EvaluateLogicalExpression(exp8).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str88);

                TruthTableDataGrid.ItemsSource = sttttt;
            }
        }

        private void logSledButton_Click(object sender, RoutedEventArgs e)
        {
            string expr1 = log1.Text;
            string expr2 = log2.Text;
            int status_log_sled = 1;

            string expression = $"({expr1})=>({expr2})";

            // Только x, y и z 
            List<string> expression_list = new List<string>();
            List<List<string>> table_list = new List<List<string>>();
            List<string> table_list1 = new List<string>();

            List<string> variables = new List<string>();
            variables = ExtractVariables(expression);

            // Если в выражении две переменные
            if (variables.Count == 2)
            {
                MessageBox.Show("Две переменные в выражении");
                List<Str1> sttttt = new List<Str1>();

                // 0 0
                string exp1 = expression.Replace('A', '0');
                exp1 = exp1.Replace('B', '0');
                Str1 str11 = new Str1 { first = "0", second = "0", fourth = EvaluateLogicalExpression(exp1).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str11);
                if (EvaluateLogicalExpression(exp1) == "False") { status_log_sled = 0; }

                // 0 1
                string exp2 = expression.Replace('A', '0');
                exp2 = exp2.Replace('B', '1');
                Str1 str22 = new Str1 { first = "0", second = "1", fourth = EvaluateLogicalExpression(exp2).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str22);
                if (EvaluateLogicalExpression(exp2) == "False") { status_log_sled = 0; }


                // 1 0 
                string exp3 = expression.Replace('A', '1');
                exp3 = exp3.Replace('B', '0');
                Str1 str33 = new Str1 { first = "1", second = "0", fourth = EvaluateLogicalExpression(exp3).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str33);
                if (EvaluateLogicalExpression(exp3) == "False") { status_log_sled = 0; }


                // 1 1
                string exp4 = expression.Replace('A', '1');
                exp4 = exp4.Replace('B', '1');
                Str1 str44 = new Str1 { first = "1", second = "1", fourth = EvaluateLogicalExpression(exp4).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str44);
                if (EvaluateLogicalExpression(exp4) == "False") { status_log_sled = 0; }


                TruthTableDataGrid.ItemsSource = sttttt;
            }
            // Если в выражении три переменные
            else if (variables.Count == 3)
            {
                List<Str1> sttttt = new List<Str1>();
                MessageBox.Show("Три переменные в выражении");
                // 0 0 0
                string exp1 = expression.Replace('A', '0');
                exp1 = exp1.Replace('B', '0');
                exp1 = exp1.Replace('C', '0');
                Str1 str11 = new Str1 { first = "0", second = "0", third = "0", fourth = EvaluateLogicalExpression(exp1).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str11);
                if (EvaluateLogicalExpression(exp1) == "False") { status_log_sled = 0; }

                // 0 0 1
                List<string> vr2 = new List<string>();
                string exp2 = expression.Replace('A', '1');
                exp2 = exp2.Replace('B', '0');
                exp2 = exp2.Replace('C', '0');
                Str1 str22 = new Str1 { first = "1", second = "0", third = "0", fourth = EvaluateLogicalExpression(exp2).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str22);
                if (EvaluateLogicalExpression(exp2) == "False") { status_log_sled = 0; }

                // 0 1 0
                List<string> vr3 = new List<string>();
                string exp3 = expression.Replace('A', '0');
                exp3 = exp3.Replace('B', '1');
                exp3 = exp3.Replace('C', '0');
                Str1 str33 = new Str1 { first = "0", second = "1", third = "0", fourth = EvaluateLogicalExpression(exp3).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str33);
                if (EvaluateLogicalExpression(exp3) == "False") { status_log_sled = 0; }

                // 1 0 0
                List<string> vr4 = new List<string>();
                string exp4 = expression.Replace('A', '0');
                exp4 = exp4.Replace('B', '0');
                exp4 = exp4.Replace('C', '1');
                Str1 str44 = new Str1 { first = "0", second = "0", third = "1", fourth = EvaluateLogicalExpression(exp4).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str44);
                if (EvaluateLogicalExpression(exp4) == "False") { status_log_sled = 0; }

                // 1 1 0
                List<string> vr5 = new List<string>();
                string exp5 = expression.Replace('A', '1');
                exp5 = exp5.Replace('B', '1');
                exp5 = exp5.Replace('C', '0');
                Str1 str55 = new Str1 { first = "1", second = "1", third = "0", fourth = EvaluateLogicalExpression(exp5).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str55);
                if (EvaluateLogicalExpression(exp5) == "False") { status_log_sled = 0; }

                // 1 0 1
                List<string> vr6 = new List<string>();
                string exp6 = expression.Replace('A', '1');
                exp6 = exp6.Replace('B', '0');
                exp6 = exp6.Replace('C', '1');
                Str1 str66 = new Str1 { first = "1", second = "0", third = "1", fourth = EvaluateLogicalExpression(exp6).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str66);
                if (EvaluateLogicalExpression(exp6) == "False") { status_log_sled = 0; }

                // 0 1 1
                List<string> vr7 = new List<string>();
                string exp7 = expression.Replace('A', '0');
                exp7 = exp7.Replace('B', '1');
                exp7 = exp7.Replace('C', '1');
                Str1 str77 = new Str1 { first = "0", second = "1", third = "1", fourth = EvaluateLogicalExpression(exp7).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str77);
                if (EvaluateLogicalExpression(exp7) == "False") { status_log_sled = 0; }

                // 1 1 1
                List<string> vr8 = new List<string>();
                string exp8 = expression.Replace('A', '1');
                exp8 = exp8.Replace('B', '1');
                exp8 = exp8.Replace('C', '1');
                Str1 str88 = new Str1 { first = "1", second = "1", third = "1", fourth = EvaluateLogicalExpression(exp8).Replace("True", "1").Replace("False", "0") };
                sttttt.Add(str88);
                if (EvaluateLogicalExpression(exp8) == "False") { status_log_sled = 0; }

                TruthTableDataGrid.ItemsSource = sttttt;
            }
            if (status_log_sled == 1) { MessageBox.Show("Является логическим следствием"); }
            else { MessageBox.Show("Не является логическим следствием"); }
        }
    }
}