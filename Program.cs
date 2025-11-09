using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorFinal
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Calculator - Final Project";
            var ui = new UserInterface();
            ui.Run();
        }
    }

    public class UserInterface
    {
        private readonly Calculator _calc = new Calculator();
        private readonly MemoryManager _mem = new MemoryManager();
        private bool _exitRequested = false;

        public void Run()
        {
            while (!_exitRequested)
            {
                PrintMainMenu();
                int choice = InputHelper.GetValidInt("Choose an option:", 1, 6);
                try
                {
                    switch (choice)
                    {
                        case 1: ArithmeticMenu(); break;
                        case 2: MemoryMenu(); break;
                        case 3: CollectionMemoryMenu(); break;
                        case 4: ShowReadmeInfo(); break;
                        case 5: ShowAbout(); break;
                        case 6: _exitRequested = ConfirmExit(); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }

                if (!_exitRequested)
                {
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    Console.Clear();
                }
            }

            Console.WriteLine("Goodbye — thank you for testing the Calculator. Press Enter to close.");
            Console.ReadLine();
        }

        private void PrintMainMenu()
        {
            Console.Clear();
            Console.WriteLine("===== Calculator - Final Project =====");
            Console.WriteLine("1) Arithmetic operations (Add/Subtract/Multiply/Divide)");
            Console.WriteLine("2) Single Memory (store/retrieve/clear)");
            Console.WriteLine("3) Collection Memory (add/remove/display/stats)");
            Console.WriteLine("4) Show README info");
            Console.WriteLine("5) About");
            Console.WriteLine("6) Quit");
        }

        private void ArithmeticMenu()
        {
            Console.WriteLine("\n-- Arithmetic Menu --");
            Console.WriteLine("1) Add");
            Console.WriteLine("2) Subtract");
            Console.WriteLine("3) Multiply");
            Console.WriteLine("4) Divide");
            Console.WriteLine("5) Back to Main Menu");

            int op = InputHelper.GetValidInt("Choose operation:", 1, 5);
            if (op == 5) return;

            double a = InputHelper.GetValidDouble("Enter first number:");
            double b = InputHelper.GetValidDouble("Enter second number:");

            try
            {
                double result = op switch
                {
                    1 => _calc.Add(a, b),
                    2 => _calc.Subtract(a, b),
                    3 => _calc.Multiply(a, b),
                    4 => _calc.Divide(a, b),
                    _ => throw new InvalidOperationException("Unknown operation")
                };
                Console.WriteLine($"Result: {result}");
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Error: Division by zero is not allowed.");
            }
        }

        private void MemoryMenu()
        {
            Console.WriteLine("\n-- Single Memory Menu --");
            Console.WriteLine("1) Store value to memory");
            Console.WriteLine("2) Retrieve memory");
            Console.WriteLine("3) Replace memory");
            Console.WriteLine("4) Clear memory");
            Console.WriteLine("5) Back to Main Menu");

            int op = InputHelper.GetValidInt("Choose option:", 1, 5);
            switch (op)
            {
                case 1:
                    double val = InputHelper.GetValidDouble("Enter value to store in memory:");
                    _mem.StoreSingle(val);
                    Console.WriteLine("Value stored.");
                    break;
                case 2:
                    if (_mem.HasSingle)
                        Console.WriteLine($"Memory contains: {_mem.RetrieveSingle()}");
                    else
                        Console.WriteLine("Memory is empty.");
                    break;
                case 3:
                    if (_mem.HasSingle)
                    {
                        double newVal = InputHelper.GetValidDouble("Enter new value to replace memory with:");
                        _mem.StoreSingle(newVal);
                        Console.WriteLine("Memory replaced.");
                    }
                    else Console.WriteLine("Memory is empty; use 'Store value' first.");
                    break;
                case 4:
                    _mem.ClearSingle();
                    Console.WriteLine("Memory cleared.");
                    break;
            }
        }

        private void CollectionMemoryMenu()
        {
            Console.WriteLine("\n-- Collection Memory Menu --");
            Console.WriteLine("1) Add integer to collection");
            Console.WriteLine("2) Remove integer from collection");
            Console.WriteLine("3) Show collection");
            Console.WriteLine("4) Show collection statistics");
            Console.WriteLine("5) Clear collection");
            Console.WriteLine("6) Back to Main Menu");

            int op = InputHelper.GetValidInt("Choose option:", 1, 6);
            switch (op)
            {
                case 1:
                    if (_mem.IsCollectionFull)
                        Console.WriteLine($"Collection full (max {_mem.CollectionMaxSize}). Remove one first.");
                    else
                    {
                        int toAdd = InputHelper.GetValidInt("Enter integer to add:");
                        _mem.AddToCollection(toAdd);
                        Console.WriteLine("Added.");
                    }
                    break;
                case 2:
                    if (!_mem.HasCollectionItems)
                        Console.WriteLine("Collection empty.");
                    else
                    {
                        int toRemove = InputHelper.GetValidInt("Enter integer to remove:");
                        bool removed = _mem.RemoveFromCollection(toRemove);
                        Console.WriteLine(removed ? "Removed." : "Not found.");
                    }
                    break;
                case 3:
                    var items = _mem.GetCollection();
                    Console.WriteLine(items.Count == 0 ? "Collection empty." :
                                      "Collection: " + string.Join(", ", items));
                    break;
                case 4:
                    if (!_mem.HasCollectionItems)
                        Console.WriteLine("Collection empty.");
                    else
                    {
                        var stats = _mem.GetCollectionStats();
                        Console.WriteLine($"Count: {stats.Count}\nSum: {stats.Sum}\nAverage: {stats.Average}\nDifference: {stats.Difference}");
                    }
                    break;
                case 5:
                    _mem.ClearCollection();
                    Console.WriteLine("Collection cleared.");
                    break;
            }
        }

        private void ShowReadmeInfo()
        {
            Console.WriteLine("\nREADME snapshot:");
            Console.WriteLine("Full instructions and video link in README.md in the GitHub repo.");
        }

        private void ShowAbout()
        {
            Console.WriteLine("\nCalculator Final Project\nAuthor: Princess Ellis\nCourse: SDC220L - Dr. Chris Fry");
        }

        private bool ConfirmExit()
        {
            Console.WriteLine("Are you sure you want to quit? (Y/N)");
            string s = Console.ReadLine().Trim().ToUpperInvariant();
            return s == "Y" || s == "YES";
        }
    }

    public static class InputHelper
    {
        public static int GetValidInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                Console.Write($"{prompt} ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int result) && result >= min && result <= max)
                    return result;
                Console.WriteLine($"Please enter an integer between {min} and {max}.");
            }
        }

        public static double GetValidDouble(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt} ");
                if (double.TryParse(Console.ReadLine(), out double result))
                    return result;
                Console.WriteLine("Invalid number — try again.");
            }
        }
    }

    public class Calculator
    {
        public double Add(double a, double b) => a + b;
        public double Subtract(double a, double b) => a - b;
        public double Multiply(double a, double b) => a * b;
        public double Divide(double a, double b)
        {
            if (b == 0) throw new DivideByZeroException("Cannot divide by zero.");
            return a / b;
        }
    }

    public class MemoryManager
    {
        private double? _singleMemory = null;
        private readonly List<int> _collection = new();
        public int CollectionMaxSize => 10;
        public bool HasSingle => _singleMemory.HasValue;
        public bool HasCollectionItems => _collection.Count > 0;
        public bool IsCollectionFull => _collection.Count >= CollectionMaxSize;

        public void StoreSingle(double value) => _singleMemory = value;
        public double? RetrieveSingle() => _singleMemory;
        public void ClearSingle() => _singleMemory = null;

        public void AddToCollection(int value)
        {
            if (IsCollectionFull) throw new InvalidOperationException("Collection full.");
            _collection.Add(value);
        }
        public bool RemoveFromCollection(int value) => _collection.Remove(value);
        public List<int> GetCollection() => new(_collection);
        public void ClearCollection() => _collection.Clear();
        public (int Count, int Sum, double Average, int Difference) GetCollectionStats()
        {
            int c = _collection.Count;
            if (c == 0) return (0, 0, 0, 0);
            int sum = _collection.Sum();
            return (c, sum, _collection.Average(), _collection.Max() - _collection.Min());
        }
    }
}
