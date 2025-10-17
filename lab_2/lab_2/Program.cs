using System;
using System.Globalization;

namespace Lab2
{
    /// <summary>
    /// Represents a 2D point with double precision.
    /// </summary>
    public readonly struct Point2D
    {
        public double X { get; }
        public double Y { get; }

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X}, {Y})";

        public static Point2D ReadFromConsole(string name)
        {
            while (true)
            {
                Console.Write($"Введіть координати {name} точки як два числа, розділені пробілом (x y): ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine("Введення не може бути порожнім. Спробуйте ще раз.");
                    continue;
                }

                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                {
                    Console.WriteLine("Введіть рівно два числа.");
                    continue;
                }

                if (double.TryParse(parts[0], NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var x)
                    && double.TryParse(parts[1], NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var y))
                {
                    return new Point2D(x, y);
                }

                Console.WriteLine("Неправильний формат числа. Використовуйте крапку як десятковий роздільник (наприклад 1.23). Спробуйте ще раз.");
            }
        }
    }

    /// <summary>
    /// Represents the line ax + by + c = 0.
    /// Encapsulates coefficients and provides method to check point membership.
    /// </summary>
    public sealed class Line
    {
        private readonly double _a;
        private readonly double _b;
        private readonly double _c;

        public double A => _a;
        public double B => _b;
        public double C => _c;

        public Line(double a, double b, double c)
        {
            if (Math.Abs(a) < double.Epsilon && Math.Abs(b) < double.Epsilon)
            {
                throw new ArgumentException("Щонайменше один із коефіцієнтів a або b має бути відмінним від нуля, щоб задати пряму.");
            }

            _a = a;
            _b = b;
            _c = c;
        }

        /// <summary>
        /// Returns true if the point lies on the line within a small tolerance.
        /// Uses distance-to-line normalization by sqrt(a^2+b^2).
        /// </summary>
        public bool Contains(Point2D point, double tolerance = 1e-9)
        {
            var numerator = Math.Abs(_a * point.X + _b * point.Y + _c);
            var denom = Math.Sqrt(_a * _a + _b * _b);
            if (denom == 0) // defensive, should not happen because constructor validates
            {
                return false;
            }

            var distance = numerator / denom;
            return distance <= tolerance;
        }

        public static Line ReadFromConsole()
        {
            while (true)
            {
                Console.Write("Введіть коефіцієнти a, b, c для прямої (ax + by + c = 0) як три числа, розділені пробілом: ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine("Введення не може бути порожнім. Спробуйте ще раз.");
                    continue;
                }

                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 3)
                {
                    Console.WriteLine("Введіть рівно три числа.");
                    continue;
                }

                if (double.TryParse(parts[0], NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var a)
                    && double.TryParse(parts[1], NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var b)
                    && double.TryParse(parts[2], NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var c))
                {
                    try
                    {
                        return new Line(a, b, c);
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                }

                Console.WriteLine("Неправильний формат числа. Використовуйте крапку як десятковий роздільник (наприклад 1.23). Спробуйте ще раз.");
            }
        }

        public override string ToString() => $"{_a}x + {_b}y + {_c} = 0";
    }

    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("Перевірка приналежності точки прямій (ax + by + c = 0)");
            Console.WriteLine("Числа вводьте з крапкою як десятковим роздільником (Invariant culture).");

            var line = Line.ReadFromConsole();
            var firstPoint = Point2D.ReadFromConsole("першої");
            var secondPoint = Point2D.ReadFromConsole("другої");

            Console.WriteLine();
            Console.WriteLine($"Пряма: {line}");
            Console.WriteLine($"Перша точка: {firstPoint} -> {(line.Contains(firstPoint) ? "належить прямій" : "не належить прямій")}");
            Console.WriteLine($"Друга точка: {secondPoint} -> {(line.Contains(secondPoint) ? "належить прямій" : "не належить прямій")}");
        }
    }
}