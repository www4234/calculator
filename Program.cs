using System;
using System.Collections.Generic;

namespace calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            RunCalculatorLoop();
        }

        /// <summary>
        /// Главный жизненный цикл программы.
        /// </summary>
        private static void RunCalculatorLoop()
        {
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (IsExitCommand(input)) 
                    break;

                if (string.IsNullOrWhiteSpace(input)) 
                    continue;

                ProcessExpression(input);
            }
        }

        private static bool IsExitCommand(string input)
        {
            return input != null && input.Trim().ToLower() == "exit";
        }

        /// <summary>
        /// Управляет процессом разбора, вычисления и вывода результата.
        /// </summary>
        private static void ProcessExpression(string input)
        {
            string[] tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (!IsValidExpressionStructure(tokens))
            {
                Console.WriteLine("Ошибка: Неверная структура выражения. Используйте формат: '2 + 2 * 2'");
                return;
            }

            if (TryCalculate(tokens, out double result, out string errorMessage))
            {
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine($"Ошибка: {errorMessage}");
            }
        }

        /// <summary>
        /// Проверяет базовую структуру: токенов должно быть нечетное количество (минимум 1).
        /// </summary>
        private static bool IsValidExpressionStructure(string[] tokens)
        {
            return tokens.Length > 0 && tokens.Length % 2 != 0;
        }

        /// <summary>
        /// Выполняет вычисление строки с учетом математического приоритета знаков.
        /// </summary>
        private static bool TryCalculate(string[] tokens, out double result, out string errorMessage)
        {
            result = 0;
            errorMessage = null;

            // Списки для раздельного хранения чисел и операторов
            List<double> numbers = new List<double>();
            List<string> operators = new List<string>();

            // Шаг 1: Первичный парсинг и валидация токенов
            for (int i = 0; i < tokens.Length; i++)
            {
                if (i % 2 == 0) // Четные индексы — всегда числа
                {
                    if (!double.TryParse(tokens[i], out double number))
                    {
                        errorMessage = $"'{tokens[i]}' не является валидным числом.";
                        return false;
                    }
                    numbers.Add(number);
                }
                else // Нечетные индексы — всегда операторы
                {
                    string op = tokens[i];
                    if (op != "+" && op != "-" && op != "*" && op != "/")
                    {
                        errorMessage = $"Неизвестный оператор '{op}'.";
                        return false;
                    }
                    operators.Add(op);
                }
            }

            // Шаг 2: Первый проход — выполняем высокоприоритетные операции (* и /)
            for (int i = 0; i < operators.Count; i++)
            {
                if (operators[i] == "*" || operators[i] == "/")
                {
                    double left = numbers[i];
                    double right = numbers[i + 1];
                    double stepResult = 0;

                    if (operators[i] == "*")
                    {
                        stepResult = left * right;
                    }
                    else // Деление
                    {
                        if (right == 0)
                        {
                            errorMessage = "Деление на ноль.";
                            return false;
                        }
                        stepResult = left / right;
                    }

                    // Сворачиваем списки: заменяем левое число результатом, удаляем правое число и знак
                    numbers[i] = stepResult;
                    numbers.RemoveAt(i + 1);
                    operators.RemoveAt(i);
                    i--; // Сдвигаем индекс назад, так как элементы сместились
                }
            }

            // Шаг 3: Второй проход — выполняем низкоприоритетные операции (+ и -)
            result = numbers[0];
            for (int i = 0; i < operators.Count; i++)
            {
                double nextNumber = numbers[i + 1];
                if (operators[i] == "+")
                {
                    result += nextNumber;
                }
                else if (operators[i] == "-")
                {
                    result -= nextNumber;
                }
            }

            return true;
        }
    }
}
