using System;

while (true)
{
    Console.Write("> ");
    string input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input) || input.ToLower() == "exit") break;

    // Разбиваем строку по пробелам
    string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    if (parts.Length % 2 == 0)
    {
        Console.WriteLine("Ошибка: Неполное выражение.");
        continue;
    }

    // Берем первое число за стартовую точку
    if (!double.TryParse(parts[0], out double result))
    {
        Console.WriteLine("Ошибка: Первым элементом должно быть число.");
        continue;
    }

    bool hasError = false;

    // Идем по массиву с шагом 2, забирая знак операции и следующее число
    for (int i = 1; i < parts.Length; i += 2)
    {
        string oper = parts[i];
        
        if (!double.TryParse(parts[i + 1], out double nextNumber))
        {
            Console.WriteLine($"Ошибка: Ожидалось число вместо '{parts[i + 1]}'.");
            hasError = true;
            break;
        }

        switch (oper)
        {
            case "+": result += nextNumber; break;
            case "-": result -= nextNumber; break;
            case "*": result *= nextNumber; break;
            case "/": 
                if (nextNumber != 0) result /= nextNumber;
                else { Console.WriteLine("Ошибка: Деление на ноль!"); hasError = true; }
                break;
            default:
                Console.WriteLine($"Ошибка: Неизвестный оператор '{oper}'.");
                hasError = true;
                break;
        }

        if (hasError) break;
    }

    if (!hasError)
    {
        Console.WriteLine(result);
    }
}
