using System;
using System.Globalization;

namespace Lab2
{
    /// <summary>
    /// Represents a point in 3D space with double precision coordinates.
    /// </summary>
    public readonly struct Point3D
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString() => $"({X}, {Y}, {Z})";

        /// <summary>
        /// Checks if all coordinates are integers (within a small tolerance).
        /// </summary>
        public bool AreAllCoordinatesInteger(double tolerance = 1e-9)
        {
            return IsInteger(X, tolerance) && IsInteger(Y, tolerance) && IsInteger(Z, tolerance);
        }

        /// <summary>
        /// Checks if coordinate is integer (within tolerance).
        /// </summary>
        private static bool IsInteger(double value, double tolerance)
        {
            return Math.Abs(value - Math.Round(value)) <= tolerance;
        }

        /// <summary>
        /// Checks if the point belongs to the first octant (x > 0, y > 0, z > 0).
        /// </summary>
        public bool IsInFirstOctant()
        {
            return X > 0 && Y > 0 && Z > 0;
        }

        /// <summary>
        /// Reads a 3D point from console with error handling.
        /// </summary>
        public static Point3D ReadFromConsole(string name)
        {
            while (true)
            {
                Console.Write($"Введіть координати {name} точки як три числа, розділені пробілом (x y z): ");
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

                if (double.TryParse(parts[0], NumberStyles.Float | NumberStyles.AllowThousands, 
                        CultureInfo.InvariantCulture, out var x)
                    && double.TryParse(parts[1], NumberStyles.Float | NumberStyles.AllowThousands, 
                        CultureInfo.InvariantCulture, out var y)
                    && double.TryParse(parts[2], NumberStyles.Float | NumberStyles.AllowThousands, 
                        CultureInfo.InvariantCulture, out var z))
                {
                    return new Point3D(x, y, z);
                }

                Console.WriteLine("Неправильний формат числа. Використовуйте крапку як десятковий роздільник (наприклад 1.23). Спробуйте ще раз.");
            }
        }
    }

    internal static class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Аналіз точки в просторі ===");
            Console.WriteLine("Числа вводьте з крапкою як десятковим роздільником.\n");

            var point = Point3D.ReadFromConsole("першої");

            Console.WriteLine();
            AnalyzePoint(point);
        }

        /// <summary>
        /// Analyzes and displays information about a 3D point.
        /// </summary>
        private static void AnalyzePoint(Point3D point)
        {
            Console.WriteLine($"Точка: {point}");
            
            bool allInteger = point.AreAllCoordinatesInteger();
            bool inFirstOctant = point.IsInFirstOctant();

            Console.WriteLine($"Всі координати цілочислові: {(allInteger ? "ДА ✓" : "НІ ✗")}");
            Console.WriteLine($"Точка в першому октанті: {(inFirstOctant ? "ДА ✓" : "НІ ✗")}");

            if (allInteger && inFirstOctant)
            {
                Console.WriteLine("→ Точка має цілочислові координати і розташована в першому октанті.");
            }
            else if (allInteger)
            {
                Console.WriteLine("→ Точка має цілочислові координати, але не в першому октанті.");
            }
            else if (inFirstOctant)
            {
                Console.WriteLine("→ Точка в першому октанті, але координати не всі цілочислові.");
            }
            else
            {
                Console.WriteLine("→ Точка не відповідає жодній умові.");
            }
        }
    }
}