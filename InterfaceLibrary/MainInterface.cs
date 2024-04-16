using System.Text;
using FileWorkingLibrary;

namespace InterfaceLibrary
{
    public class MainInterface
    {
        private PathProcessing _fPath;
        private PathProcessing? _nPath;
        private DataProcessing _data;
        private Customer[] _processedData;
        private int _num;

        /// <summary>
        /// This constructor checks path and data in it.
        /// </summary>
        public MainInterface()
        {
            while (true)
            {
                SayHello();
                Console.WriteLine("Please choose one option to read data.");
                Console.WriteLine();

                // Printing menu to choose a way of saving file.
                Menu menu = new Menu("Use up/down keys to choose menu item.", new string[] { "1. Read data from a *.json file.", "2. Read data from the console." });

                // Variable of user's choice.
                _num = menu.ActMenu();
                try
                {
                    // If user chooses reading data from a file..
                    if (_num == 1)
                    {
                        PrintColor("Enter your file's absolute name.", ConsoleColor.Magenta, ConsoleColor.DarkCyan);

                        // Checking path.
                        _fPath = new PathProcessing(Console.ReadLine());

                        _data = new DataProcessing(_fPath.FPath);
                        PrintColor("You enter correct file name and data in it is also right.Congratulations!", ConsoleColor.Green, ConsoleColor.DarkGreen);
                    }
                    // If user chooses reading data from console.
                    else
                    {
                        PrintColor("Enter your data. To finish enter write \"exit\" at new line", ConsoleColor.Magenta, ConsoleColor.DarkCyan);
                        StringBuilder sb = new StringBuilder();
                        string? line;

                        // Reading data while !exit
                        while ((line = Console.ReadLine()) != null && line != "exit")
                        {
                            sb.Append(line);
                        }
                        _data = new DataProcessing(sb);
                        Console.WriteLine("You enter correct data.Congratulations!");
                    }
                }
                catch (PathTooLongException)
                {
                    PrintColor("Your file name is too long. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    PrintColor("Wrong file path. Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (UnauthorizedAccessException)
                {
                    PrintColor("This file can be only read. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (IOException)
                {
                    PrintColor("Error while writing data in the file. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (ArgumentException)
                {
                    PrintColor("Wrong file. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch(NullReferenceException)
                {
                    PrintColor("Sorry, there is a problem with data. Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                }
                break;
            }
        }
        /// <summary>
        /// This methods implements menu interface.
        /// </summary>
        /// <param name="num"></param>
        public void ShowMenu(out int num)
        {
            // Show interactive menu.
            Menu menu = new Menu("Please choose a way of processing file", new string[] { "1. Sort data field ascending", "2. Sort data field descending", "3. Make a selection of field", "4. Exit the program" });
           
            // Repeating cycle to show menu again if the corresponding option has been chosen.
            while (true)
            {
                // Index of user's choice.
                num = menu.ActMenu();

                // Variable to indicate when we should exit data processing.
                bool flag = true;
                DataProcessingInterface process;

                // If user chooses sorting ascending.
                if (num == 1)
                {
                    process = new DataProcessingInterface(1);
                    _processedData = process.SortInterface(_data, ref flag);
                    if (!flag)
                        continue;
                    break;
                }
                // If user chooses sorting descending.
                if (num == 2)
                {
                    process = new DataProcessingInterface(2);
                    _processedData = process.SortInterface(_data, ref flag);
                    if (!flag)
                        continue;
                    break;
                }
                // If user chooses selecting.
                if (num == 3)
                {
                    process = new DataProcessingInterface(2);
                    _processedData = process.SelectInterface(_data, ref flag);
                    if (!flag)
                        continue;
                    break;
                }
                // If user chooses exit.
                if (num == 4)
                    return;
            }
        }
        /// <summary>
        /// This method implements saving data to the file interface.
        /// </summary>
        public void SaveData()
        {
            // Print or not print.
            Menu printMenu = new Menu("Do you want to print your data?", new string[] { "Yes", "No, I just want to save it." });

            // User's choice.
            int choice = printMenu.ActMenu();

            // If user chooses 
            if(choice == 1)
            {
                PrintData();
            }
            Menu saveMenu;

            // Initializing num with 2 when user entered data in console and there are no oportunity to save it in the initial file.
            int num = 2;

            //  If user printed data in console.
            if (_num == 2)
            {
                PrintColor("Your data is going to be saved in a new file because wou entered your data without an initial file.", ConsoleColor.Yellow, ConsoleColor.DarkYellow);
            }
            // If data had been read from the file.
            else
            {
                saveMenu = new Menu("Your data is going to be saved. Please choose a way of saving", new string[] { "Save to the initial file instead of the original data", "Save in a new file" });
                num = saveMenu.ActMenu();
            }
            // The cycle doesn't allow to continue until data will be saved correctly.
            while (true)
            {
                try
                {
                    PrintInterface print = new PrintInterface(_processedData, 3);

                    // If user chooses saving in initial file.
                    if (num == 1)
                    {
                        PrintColor($"Your data will be saved in the initial file {_fPath.FPath}", ConsoleColor.Magenta, ConsoleColor.DarkCyan);
                        JsonParser.WriteJson(_fPath.FPath, print.PrintAsJson);
                    }
                    // If user chooses saving in a new file.
                    if (num == 2)
                    {
                        PrintColor("Enter file name to create the file and save data there.", ConsoleColor.Magenta, ConsoleColor.DarkCyan);
                        _nPath = new PathProcessing(Console.ReadLine());
                        JsonParser.WriteJson(_nPath.NPath, print.PrintAsJson);

                    }
                    Console.WriteLine();
                    PrintColor("Data has been successfully saved.", ConsoleColor.Green, ConsoleColor.DarkGreen);
                    break;
                }
                catch (PathTooLongException)
                {
                    PrintColor("Your file name is too long. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    PrintColor("Wrong file path. Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (UnauthorizedAccessException)
                {
                    PrintColor("This file can be only read. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (IOException)
                {
                    PrintColor("Error while writing data in the file. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (ArgumentException)
                {
                    PrintColor("Wrong file name.Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch(NullReferenceException)
                {
                    PrintColor("Wrong file name. Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                }
            }
        }
        /// <summary>
        /// This method implements printing data in console.
        /// </summary>
        public void PrintData()
        {
            // Printing menu of choosing the way of printing data.
            Menu print = new Menu("How do you want to print table?", new string[] { "As a table", "In JSON format" });

            int numMenu = print.ActMenu();

            // Printing menu of choosing.
            Menu topBottom = new Menu("Do you want to see only first/last elemnts", new string[] { "First N elements", "Last N elements", "All elements" });
            int choice = topBottom.ActMenu();

            PrintInterface printData = new PrintInterface(_processedData, choice);

            // If it is needed to print data as a table.
            if (numMenu == 1)
            {
                printData.PrintAsTable();
            }
            else
            {
                printData.PrintAsJson();
            }
        }
        /// <summary>
        /// This method greets user according to date time.
        /// </summary>
        private void SayHello()
        {
            if (DateTime.Now.Hour <= 12 && DateTime.Now.Hour >= 5)
                PrintColor("Good morning!", ConsoleColor.Yellow, ConsoleColor.Yellow);

            else if (DateTime.Now.Hour <= 17 && DateTime.Now.Hour > 12)
                PrintColor( "Good afternoon!", ConsoleColor.Magenta, ConsoleColor.Magenta);

            else if (DateTime.Now.Hour <= 22 && DateTime.Now.Hour > 17)
                PrintColor( "Good evening!", ConsoleColor.Blue, ConsoleColor.Blue);

            else if (DateTime.Now.Hour < 5 && DateTime.Now.Hour > 1)
                PrintColor( "Good Night! I am sorry you haven't slept yet.", ConsoleColor.Cyan, ConsoleColor.Cyan);

            else
                PrintColor( "Good night!", ConsoleColor.DarkBlue, ConsoleColor.DarkBlue);
        }
        /// <summary>
        /// This method prints colourful text.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="color"></param>
        public static void PrintColor(string str, ConsoleColor colorDay, ConsoleColor colorNight)
        {
            if (DateTime.Now.Hour <= 19 && DateTime.Now.Hour >= 7)
            {
                Console.ForegroundColor = colorDay;
                Console.WriteLine(str);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = colorNight;
                Console.WriteLine(str);
                Console.ResetColor();
            }
        }
    }
}