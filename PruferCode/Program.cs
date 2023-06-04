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
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    result.Add(int.Parse(line.Trim()));
                }
            }

            return result;
        }

        public List<string> ReadDecodeFromFile()
        {
            List<string> result = new List<string>();

            using (StreamReader sr = new StreamReader(outputFile))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    result.Add(line.Trim());
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
            decode.Add($"{n} -> {lastNumber}");

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

        public List<int> EncodePrufer(List<string> decode)
        {
            List<int> code = new List<int>();
            int n = decode.Count + 2;
            int[] degrees = new int[n + 1];

            // Подсчет степеней вершин
            foreach (string pair in decode)
            {
                string[] vertices = pair.Split(new[] { " -> " }, StringSplitOptions.None);
                int vertex1, vertex2;

                if (int.TryParse(vertices[0], out vertex1) && int.TryParse(vertices[1], out vertex2))
                {
                    degrees[vertex1]++;
                    degrees[vertex2]++;
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный формат вершин в файле decode.txt.");
                    return code;
                }
            }

            // Кодирование Прюфера
            for (int i = 0; i < n - 2; i++)
            {
                int smallest = Array.FindIndex(degrees, d => d == 1);

                if (smallest == -1)
                {
                    smallest = Array.FindIndex(degrees, d => d != 0); // Выбрать первый доступный индекс, не равный 0
                }

                string[] vertices = decode[i].Split(new[] { " -> " }, StringSplitOptions.None);
                int vertex1, vertex2;

                if (int.TryParse(vertices[0], out vertex1) && int.TryParse(vertices[1], out vertex2))
                {
                    code.Add(vertex2);
                    degrees[smallest]--;
                    degrees[vertex1]--;
                    degrees[vertex2]--;
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный формат вершин в файле decode.txt.");
                    return code;
                }
            }

            return code;
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

        public void WriteCodeToFile(List<int> code)
        {
            using (StreamWriter sw = new StreamWriter(inputFile))
            {
                foreach (int num in code)
                {
                    sw.WriteLine(num);
                }
            }

            Console.WriteLine("Закодированный код Прюфера успешно записан в файл.");
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
                Console.WriteLine("1 - закодировать (нужно заполнить файл decode.txt)");
                Console.WriteLine("2 - декодировать (нужно заполнить файл code.txt)");
                Console.WriteLine("3 - выход из программы");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Кодирование:");
                        List<string> decodeToEncode = prufer.ReadDecodeFromFile();
                        List<int> encoded = prufer.EncodePrufer(decodeToEncode);
                        prufer.WriteCodeToFile(encoded);
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