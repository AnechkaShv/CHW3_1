using System.Text;

namespace FileWorkingLibrary
{
    public class DataProcessing
    {
        enum Column
        {
            id = 1,
            name,
            email,
            age,
            city,
            isPremium,
            orders

        }
        private Customer[] _customers;
        private string[] _ordersStr;
        public DataProcessing() { }
        /// <summary>
        /// This constructor creates Customer array and initialize its object's fields when we read data from a file.
        /// </summary>
        /// <param name="fPath"></param>
        public DataProcessing(string fPath)
        {
            // Initializing dictionary with data from a file.
            Dictionary<string, object?>[] data = JsonParser.ReadJson(fPath);

            // Array of Customers to initialize.
            _customers = new Customer[data.Length];

            // Orders to convert to double array.
            _ordersStr = new string[data.Length];

            // Initializing orders and converting its values to double arrays. Initializing Customer array.
            for (int i = 0; i < data.Length; i++)
            {
                // We are sure that orders value is not null.
                _ordersStr[i] = data[i]["orders"].ToString();

                string[] orders = _ordersStr[i].Split(",", StringSplitOptions.RemoveEmptyEntries);
                double[] newOrders = new double[orders.Length];

                for (int j = 0; j < orders.Length; j++)
                {
                    newOrders[j] = double.Parse(orders[j]);
                }
                _customers[i] = new Customer(int.Parse(data[i]["customer_id"].ToString()), int.Parse(data[i]["age"].ToString()), data[i]["name"].ToString(), data[i]["email"].ToString(), data[i]["city"].ToString(), newOrders, bool.Parse(data[i]["is_premium"].ToString()));
            }
        }

        /// <summary>
        /// This constructor creates Customer array and initialize its object's fields when we read data from the console.
        /// </summary>
        /// <param name="sb"></param>
        public DataProcessing(StringBuilder sb)
        {
            // Doing the same things like when we read from a file.
            Dictionary<string, object?>[] data = JsonParser.ReadJson(null, sb);
            _customers = new Customer[data.Length];
            _ordersStr = new string[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                _ordersStr[i] = data[i]["orders"].ToString();
                string[] orders = _ordersStr[i].Split(",", StringSplitOptions.RemoveEmptyEntries);
                double[] newOrders = new double[orders.Length];
                for (int j = 0; j < orders.Length; j++)
                {
                    newOrders[j] = double.Parse(orders[j]);
                }
                _customers[i] = new Customer(int.Parse(data[i]["customer_id"].ToString()), int.Parse(data[i]["age"].ToString()), data[i]["name"].ToString(), data[i]["email"].ToString(), data[i]["city"].ToString(), newOrders, bool.Parse(data[i]["is_premium"].ToString()));
            }
        }
        /// <summary>
        /// This method selects elements with suitable values.
        /// </summary>
        /// <param name="indexColumn"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Customer[]? Select(int indexColumn, string value)
        {
            // Quantity of suitable elements.
            int counter = 0;

            Customer[]? selectedData;

            // Comparing every element in the selected column with user's value.
            for (int i = 0; i < _customers.Length; i++)
            {
                if ((indexColumn == 1 && int.TryParse(value, out _) && _customers[i].id == int.Parse(value)) ||
                    (indexColumn == 2 && _customers[i].name.Contains(value)) ||
                    (indexColumn == 3 && _customers[i].email.Contains(value)) ||
                    (indexColumn == 4 && int.TryParse(value, out _) && _customers[i].age == int.Parse(value)) ||
                    (indexColumn == 5 && _customers[i].city.Contains(value)) ||
                    (indexColumn == 6 && bool.TryParse(value, out _) && _customers[i].isPremium == bool.Parse(value)) ||
                    (indexColumn == 7 && _ordersStr[i].Contains(value)))
                {
                    counter += 1;
                }
            }
            // If there are no suitable elements returns null.
            if (counter == 0)
            {
                selectedData = null;
                return selectedData;
            }
            // Sizes of the result array and suitable elements are equal = counter.
            selectedData = new Customer[counter];

            // Index of result data's row to write result there.
            int idxElem = 0;

            // Checking if there are no suitable elements.
            bool isValue = false;

            for (int i = 0; i < _customers.Length; i++)
            {
                // Comparing every element in the selected column with user's value and filling the result table.
                if ((indexColumn == 1 && int.TryParse(value, out _) && _customers[i].id == int.Parse(value)) || 
                    (indexColumn == 2 && _customers[i].name.Contains(value)) ||
                    (indexColumn == 3 && _customers[i].email.Contains(value)) ||
                    (indexColumn == 4 && int.TryParse(value, out _) && _customers[i].age == int.Parse(value)) ||
                    (indexColumn == 5 && _customers[i].city.Contains(value)) ||
                    (indexColumn == 6 && bool.TryParse(value, out _) && _customers[i].isPremium == bool.Parse(value)) ||
                    (indexColumn == 7 && _ordersStr[i].Contains(value)))
                {
                    selectedData[idxElem] = _customers[i];
                    idxElem++;
                    isValue = true;
                }
            }
            if (!isValue)
                return null;
            return selectedData;
        }

        /// <summary>
        /// This method sorts an array.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public Customer[] Sort(int idx, int indexColumn)
        {
            Customer[] sortedData = _customers;

            // Bubble sorting rows according to the column.
            for (int i = 0; i < sortedData.Length - 1; i++)
            {
                for (int j = 0; j < sortedData.Length - i - 1; j++)
                {
                    // Index of ascending sorting(user's menu number 1).
                    if (idx == 1)
                    {
                        // Sorting not "orders" fields with hand-made method CompareTo.
                        if (indexColumn != 7)
                        {
                            
                            if (_customers[j].CompareTo(_customers[j + 1], indexColumn) >= 0 || sortedData[j] is null)
                            {
                                Customer temp = sortedData[j + 1];
                                sortedData[j + 1] = sortedData[j];
                                sortedData[j] = temp;
                            }
                        }
                        // Sorting "orders" field.
                        else
                        {
                            if (_customers[j].CompareTo(_customers[j + 1]) >= 0 || sortedData[j] is null)
                            {
                                Customer temp = sortedData[j + 1];
                                sortedData[j + 1] = sortedData[j];
                                sortedData[j] = temp;
                            }
                        }
                    }
                    // Index of descending sorting(user's menu number 2).
                    else
                    {
                        // Sorting not "orders" fields with hand-made method CompareTo.
                        if (indexColumn != 7)
                        {
                            if (_customers[j].CompareTo(_customers[j + 1], indexColumn) <= 0 || sortedData[j + 1] is null)
                            {
                                Customer temp = sortedData[j + 1];
                                sortedData[j + 1] = sortedData[j];
                                sortedData[j] = temp;

                            }
                        }
                        // Sorting "orders" field.
                        else
                        {
                            if (_customers[j].CompareTo(_customers[j + 1]) <= 0 || sortedData[j + 1] is null)
                            {
                                Customer temp = sortedData[j + 1];
                                sortedData[j + 1] = sortedData[j];
                                sortedData[j] = temp;
                            }
                        }
                    }
                }
            }
            return sortedData;
        }
    }
}