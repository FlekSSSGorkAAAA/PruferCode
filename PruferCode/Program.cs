using System;
using System.Collections.Generic;
using System.IO;

namespace PruferCode
{
    class Prufer
    {
        string inputFile;
        string outputFile;

        public Prufer(string inputFile, string outputFile)
        {
            this.inputFile = inputFile;
            this.outputFile = outputFile;
        }

        public List<int> ReadCodeFromFile()
        {
            List<int> result = new List<int>();

            using (StreamReader sr = new StreamReader(inputFile))
            {
                string line = sr.ReadLine();
                string[] strings = line.Split(',');
                foreach (string str in strings)
                {
                    result.Add(int.Parse(str.Trim()));
                }
            }

            return result;
        }

        public List<string> DecodePrufer(List<int> code)
        {
            List<string> decode = new List<string>();

            int n = code.Count + 2;
            int[] degrees = new int[n + 1];

            // Подсчет степеней вершин
            foreach (int num in code)
            {
                degrees[num]++;
            }

            // Поиск наименьшей вершины
            int smallest = 1;
            while (degrees[smallest] != 0)
            {
                smallest++;
            }

            // Декодирование кода Прюфера
            foreach (int num in code)
            {
                decode.Add($"{num} -> {smallest}");
                degrees[num]--;
                degrees[smallest]--;

                if (degrees[num] == 0 && num < smallest)
                {
                    smallest = num;
                }
                else
                {
                    while (degrees[smallest] != 0)
                    {
                        smallest++;
                    }
                }
            }

            // Добавление последней вершины
            int lastNumber = 1;
            while (degrees[lastNumber] != 0)
            {
                lastNumber++;
            }
            decode.Add($"{lastNumber} -> {n}");

            return decode;
        }


        private int FindSmallest(List<int> numbers)
        {
            int smallest = int.MaxValue;
            foreach (int num in numbers)
            {
                if (num != 0 && num < smallest)
                {
                    smallest = num;
                }
            }
            return smallest;
        }


        public void WriteDecodeToFile(List<string> decode)
        {
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                foreach (string str in decode)
                {
                    sw.WriteLine(str);
                }
            }

            Console.WriteLine("Код Прюфера успешно декодирован и записан в файл.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Prufer prufer = new Prufer("code.txt", "decode.txt");

            while (true)
            {
                Console.WriteLine("Что вы желаете?");
                Console.WriteLine("1 - закодировать (нужно заполнить файл code.txt)");
                Console.WriteLine("2 - декодировать (нужно заполнить файл decode.txt)");
                Console.WriteLine("3 - выход из программы");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Кодирование:");
                        break;
                    case 2:
                        Console.WriteLine("Декодирование:");
                        List<int> code = prufer.ReadCodeFromFile();
                        List<string> decoded = prufer.DecodePrufer(code);
                        prufer.WriteDecodeToFile(decoded);
                        break;
                    case 3:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}