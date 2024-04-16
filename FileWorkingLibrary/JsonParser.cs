using System.Text;

namespace FileWorkingLibrary
{
    public static class JsonParser
    {
        // States for finite automation.
        enum State
        {
            DataBeginning,
            KeyBeginning,
            KeyEnding,
            KeyReading,
            ValueBeginning,
            ValueEnding,
            ValueReading,
            DataEnding,
            OrdersBeginning,
            OrdersEnding,
            OrdersReading
        }
        /// <summary>
        /// This method reads json file and parse it or parse data from console.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Dictionary<string, object?>[] ReadJson(string? fileName = null, StringBuilder? sb = null)
        {
            // Setting english culture to read json double array correctly.
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            // Reading data from a file.
            if (sb == null && fileName != null)
            {
                sb = new StringBuilder();

                // Stream redirection.
                using (StreamReader sr = new StreamReader(fileName))
                {
                    Console.SetIn(sr);
                    string? line;
                    while ((line = Console.ReadLine()) != null)
                    {
                        sb.Append(line);
                    }
                }
                // Returning the the ordinary input stream.
                Console.SetIn(new StreamReader(Console.OpenStandardInput()));
            }
            // Checking data.
            if (String.IsNullOrEmpty(sb.ToString()) || sb[0] != '[' || sb[^1] != ']')
                throw new ArgumentException();
            State state = State.DataBeginning;

            // Number of elements.
            int count = sb.ToString().Split("},", StringSplitOptions.RemoveEmptyEntries).Length;

            Dictionary<string, object?>[] data = new Dictionary<string, object?>[count];

            // Initializing dictionary.
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new Dictionary<string, object?>();
            }
            count = 0;
            state = State.DataBeginning;

            // Variables for reading keys and values in it.
            StringBuilder key = new StringBuilder();
            StringBuilder value = new StringBuilder();

            // Finite automation cycle.
            for (int i = 0; i < sb.Length; i++)
            {
                switch (state)
                {
                    case State.DataBeginning or State.ValueEnding when sb[i] == '"':
                        state = State.KeyReading;
                        break;
                    case State.KeyReading when sb[i] != '"':
                        key.Append(sb[i]);
                        break;
                    case State.KeyReading when sb[i] == '"':
                        data[count].Add(key.ToString(), null);
                        state = State.KeyEnding;
                        break;
                    case State.KeyEnding when sb[i] != ' ' && sb[i] != ':' && sb[i] != '[' && sb[i] != '"':
                        state = State.ValueReading;
                        value.Append(sb[i]);
                        break;
                    case State.ValueReading when sb[i] != ',' && sb[i] != '"':
                        value.Append(sb[i]);
                        break;
                    case State.ValueReading when sb[i] == ',' || sb[i] == '"':
                        data[count][key.ToString()] = value;
                        state = State.ValueEnding;
                        key = new StringBuilder();
                        value = new StringBuilder();
                        break;
                    case State.KeyEnding when sb[i] == '[':
                        state = State.OrdersBeginning;
                        break;
                    case State.OrdersBeginning when Char.IsDigit(sb[i]):
                        value.Append(sb[i]);
                        state = State.OrdersReading;
                        break;
                    case State.OrdersReading when sb[i] != ']' && sb[i] != '\n' && sb[i] != ' ':
                        value.Append(sb[i]);
                        break;
                    case State.OrdersReading when sb[i] == ']':
                        data[count][key.ToString()] = value;
                        state = State.OrdersEnding;
                        key = new StringBuilder();
                        value = new StringBuilder();
                        break;
                    case State.OrdersEnding when sb[i] == '}':
                        state = State.DataEnding;
                        break;
                    case State.DataEnding when sb[i] == '{':
                        count += 1;
                        state = State.DataBeginning;
                        break;
                }
            }
            // Checking the result of processing data with automation.
            if (data == null || state != State.DataEnding || !IsCorrectJson(data))
                throw new ArgumentException();
            return data;
        }
        /// <summary>
        /// This method checks data in json file or from the console.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsCorrectJson(Dictionary<string, object>[] data)
        {
            if (data is null)
                return false;

            for (int i = 0; i < data.Length; i++)
            {
                // Checking keys' names 
                if (data[i] is null || !data[i].ContainsKey("customer_id") || !data[i].ContainsKey("name") || !data[i].ContainsKey("email") || !data[i].ContainsKey("age") || !data[i].ContainsKey("city") || !data[i].ContainsKey("is_premium") || !data[i].ContainsKey("orders") || data[i].Count() != 7)
                    return false;
                // Checking values.
                foreach (var item in data[i])
                {
                    if (item.Value == null || item.Value == (object)0)
                        return false;
                }
                // Checking values' type.
                if (!int.TryParse(data[i]["customer_id"].ToString(), out _) || !int.TryParse(data[i]["age"].ToString(), out _) || !bool.TryParse(data[i]["is_premium"].ToString(), out _))
                    return false;

                // Initializing dictionary's values.
                data[i]["customer_id"] = int.Parse(data[i]["customer_id"].ToString());
                data[i]["age"] = int.Parse(data[i]["age"].ToString());
                data[i]["is_premium"] = bool.Parse(data[i]["is_premium"].ToString());

                // Initializing "orders".
                string[] orders = data[i]["orders"].ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < orders.Length; j++)
                {
                    if (!double.TryParse(orders[j], out _))
                        return false;
                }
            }
            return true;
        }
        /// <summary>
        /// This method writes processed data.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="print"></param>
        public static void WriteJson(string fileName, Action print)
        {
            // Redirecting output stream.
            using (StreamWriter sw = new StreamWriter(fileName, false))
            {
                Console.SetOut(sw);
                print();
            }
            // Returning to the ordinary stream.
            StreamWriter stream = new StreamWriter(Console.OpenStandardOutput());
            stream.AutoFlush = true;
            Console.SetOut(stream);
        }
    }
}
