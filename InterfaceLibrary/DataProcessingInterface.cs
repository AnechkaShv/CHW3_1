using FileWorkingLibrary;


namespace InterfaceLibrary
{
    public class DataProcessingInterface
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
        private int _idx;
        private Customer?[] _newCustomers;
        public DataProcessingInterface() { }
        /// <summary>
        /// This constructor initialize index of processing.
        /// </summary>
        /// <param name="idx"></param>
        public DataProcessingInterface(int idx)
        {
            this._idx = idx;
        }
        /// <summary>
        /// This method implements sorted interface and rturns sorted data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public Customer[] SortInterface(DataProcessing data, ref bool flag)
        {
            // Printing menu of choosing sorting value.
            MainInterface.PrintColor("Please choose the number of field for sorting", ConsoleColor.Magenta, ConsoleColor.Cyan);
            Menu menu = new Menu("Use up/down keys to choose menu item.", new string[] { "customer_id", "name", "email", "age", "city", "is_premium", "orders" });

            // Getting user's choice of value.
            int num = menu.ActMenu();

            MainInterface.PrintColor($"You have chosen column {(Column)num} for sorting.", ConsoleColor.Magenta, ConsoleColor.Cyan);

            // Sorting "orders".
            if (num == 7)
            {
                // Printing warning message.
                MainInterface.PrintColor("This field includes array and is going to be sorted by its length at first. Then if lengths are equal values of an array will be compared. Do you really want to sort an array?", ConsoleColor.Yellow, ConsoleColor.DarkYellow);
                menu = new Menu("Use up/down keys to choose menu item.", new string[] { "Yes", "No. return to the menu" });

                int sure = menu.ActMenu();
                // Implementing sorting.
                if (sure == 1)
                {
                    _newCustomers = data.Sort(_idx, num);
                }
                // Returning to the menu.
                else
                {
                    flag = false;
                    return _newCustomers;
                }
            }
            // Sorting elements.
            else
            {
               _newCustomers = data.Sort(_idx, num);
            }
            return _newCustomers;
        }
        /// <summary>
        /// This method implements selection interface.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public Customer[] SelectInterface(DataProcessing data, ref bool flag)
        {
            Console.WriteLine("Please choose the number of field for sorting");
            // Printing menu of choosing sorting value.
            Menu menu = new Menu("Use up/down keys to choose menu item.", new string[] { "customer_id", "name", "email", "age", "city", "is_premium", "orders" });

            int num = menu.ActMenu();
            MainInterface.PrintColor($"You have chosen column {(Column)num} for selection.", ConsoleColor.Magenta, ConsoleColor.Cyan);

            // This cycle doesn't allow to do next step until user enters right data.
            while (true)
            {
                Console.WriteLine($"Enter value to make a selection in column {(Column)num}");
                Console.Write("Value: ");

                // Getting selection value from user.
                string? value = Console.ReadLine();

                // Checking value.
                if (value == null || value.Length <= 0)
                {
                    MainInterface.PrintColor("Wrong value. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                // Selecting data.
                _newCustomers = data.Select(num, value);

                // If there are no user's value in the field than notTable = null  and printing menu of actions.
                if (_newCustomers == null)
                {
                    Menu emptyOutput = new Menu("This value doesn't exist in the table. Do you want to try again?", new string[] { "1. Yes", "2. No. Return to the menu" });

                    int numEmpty = emptyOutput.ActMenu();
                    // If user wants to enter value(s) again.
                    if (numEmpty == 1)
                    {
                        continue;
                    }
                    // If user wants to return to the main menu.
                    else
                    {
                        flag = false;
                        break;
                    }
                }
                break;
            }
            return _newCustomers;
        }
    }
}
