using FileWorkingLibrary;

namespace InterfaceLibrary
{
    public class PrintInterface
    {
        private Customer[] _data;
        private int _num;
        private string[] _columnNames = { "customer_id", "name", "email", "age", "city", "is_premium", "orders" };
        public PrintInterface() { }
        /// <summary>
        /// This constructor initializes data and number of choice how to print data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="num"></param>
        public PrintInterface(Customer[] data, int num)
        {
            _data = data;
            _num = num;
        }
        /// <summary>
        /// This method prints data in the table format(like csv data).
        /// </summary>
        public void PrintAsTable()
        {
            int N = _data.Length;

            // Jagged array for Customer data.
            string[][] printTable;
            int[] maxElems;

            // If we need to print first N elements or the whole data.
            if (_num == 1 || _num == 2)
            {
                Console.WriteLine($"Enter 1 <= N <= {_data.Length}");

                // Processing wrong numbers.
                while (!int.TryParse(Console.ReadLine(), out N) || N < 1 || N > _data.Length)
                    MainInterface.PrintColor("Wrong number.Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
            }

            printTable = new string[N][];

            // Printing first N rows or the whole table.
            if (_num == 1 || _num == 3)
            {

                // Initializing array with Customer data.
                for (int i = 0; i < N; i++)
                {
                    printTable[i] = new string[] { $"{_data[i].id}", $"{_data[i].name}", $"{_data[i].email}", $"{_data[i].age}", $"{_data[i].city}", $"{_data[i].isPremium}", $"{String.Join(',', _data[i].orders)}" };
                }
            }
            // Printing last N elements.
            else
            {
                // Initializing array with Customer data.
                for (int i = _data.Length - N; i < _data.Length; i++)
                {
                    printTable[i - _data.Length + N] = new string[] { $"{_data[i].id}", $"{_data[i].name}", $"{_data[i].email}", $"{_data[i].age}", $"{_data[i].city}", $"{_data[i].isPremium}", $"{String.Join(',', _data[i].orders)}" };
                }
            }
            // Array of the maximal length of element in every row.
            maxElems = new int[_columnNames.Length];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < printTable[i].Length; j++)
                {
                    //Maximal length of element between two column names rows in each column.
                    maxElems[j] = Math.Max(Math.Max(maxElems[j], printTable[i][j].Length), _columnNames[j].Length);
                }
            }
            // Printing columns' names.
            for (int i = 0; i < _columnNames.Length; i++)
            {
                // Printing formatted values except the last one.
                if (i != _columnNames.Length - 1)
                    Console.Write(_columnNames[i] + new string(' ', maxElems[i] - _columnNames[i].Length) + '|' + ' ');
                else
                    Console.Write(_columnNames[i]);
            }
            Console.WriteLine();

            // Checking empty values.
            for (int i = 0; i < printTable.Length; i++, Console.WriteLine())
            {
                // Number of empty elements in each row.
                int counter = 0;
                for (int j = 0; j < printTable[i].Length; j++)
                {
                    if (printTable[i][j] == " " || printTable[i][j] is null)
                    {
                        counter += 1;
                    }
                }
                // Printing elements of row only if the row isn't empty.
                if (counter != printTable[i].Length)
                {
                    for (int j = 0; j < printTable[i].Length; j++)
                    {
                        // Checking elements.
                        if (printTable[i][j] != null && printTable[i][j] != " " && printTable[i][j].Length > 0)
                        {
                            // Printing elements of the table with right number of spaces.
                            if (j != printTable[i].Length - 1)
                            {
                                Console.Write(printTable[i][j] + new string(' ', maxElems[j] - printTable[i][j].Length) + '|' + ' ');
                            }
                            // Printing last element without spaces.
                            else
                            {
                                Console.Write(printTable[i][j]);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// This method prints data in the json format.
        /// </summary>
        public void PrintAsJson()
        {
            int N = _data.Length;

            // If we need to print first N elements or the whole data.
            if (_num == 1 || _num == 2)
            {
                Console.WriteLine($"Enter 1 <= N <= {_data.Length}");

                // Processing wrong numbers.
                while (!int.TryParse(Console.ReadLine(), out N) || N < 1 || N > _data.Length)
                    MainInterface.PrintColor("Wrong number.Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
            }
            // Number of empty elements in each row.
            int counter = 0;
            for (int i = 0; i < N; i++)
            {
                if (_data[i] is null)
                {
                    counter += 1;
                }
            }
            // Printing values and keys only if the element isn't empty.
            if (counter != _data.Length)
            { 
                // Printing first N elements or the whole data.
                if (_num == 1 || _num == 3)
                {
                    Console.WriteLine("[");
                    for (int i = 0; i < N; i++)
                    {
                        if (i != _data.Length - 1)
                            Console.Write(_data[i].ToString());
                        else
                            // Printing without , of last element. 
                            Console.Write($"{_data[i].ToString()[..^2]}\n");
                    }
                    Console.Write("]");
                }

                // Printing last N elements.
                else
                {
                    Console.WriteLine("[");
                    for (int i = _data.Length - N; i < _data.Length; i++)
                    {
                        if (i != _data.Length - 1)
                            Console.Write(_data[i].ToString());
                        else
                            Console.Write($"{_data[i].ToString()[..^2]}\n");
                    }
                    Console.Write("]");
                }
            }
        }
    }
}
