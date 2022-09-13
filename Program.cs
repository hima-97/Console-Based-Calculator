// See https://aka.ms/new-console-template for more information
// For console applications, the following directives are implicitly included in the application:
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using System.Data;
using System.Globalization;
using System.Diagnostics;

// The namespace depends on the project name:
namespace consoleBasedCalculator
{
    class Program
    {
        // List of strings (i.e. expressions) for history:
        public static List<string> history = new List<string>();

        // Variable to track current operand to use in expression:
        public static string currentOperandInExpression = "";

        // Variable to track current expression to evaluate:
        public static string currentExpression = "";

        // Boolean function to check if string input is a valid double value:
        public static bool isStringValidDouble(string myString)
        {
            if (double.TryParse(myString, out double myDouble) && !Double.IsNaN(myDouble) && !Double.IsInfinity(myDouble))
            {
                // double.TryParse(secondOperand, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedSecondOperand)
                return true;
            }

            return false;
        }

        // Function to get operand from the user:
        public static string getOperandsFromUser()
        {
            // Getting user input for operand:
            Console.WriteLine("\nEnter operand:");
            string myOperand = Convert.ToString(Console.ReadLine());

            // If operand is not a valid number, then try again:
            while (isStringValidDouble(myOperand) == false)
            {
                Console.WriteLine("Not a valid operand!");
                Console.WriteLine("\nEnter operand:");
                myOperand = Convert.ToString(Console.ReadLine());
            }

            return myOperand;
        }

        // Function to find square, square root, or inverse of operand within an operation:
        public static string operationWithinOperation(string myOperand)
        {
            // Displaying menu to perform an operation on single operand:
            Console.WriteLine("\nSelect an option for current operand: " + currentOperandInExpression + " = " + myOperand + "\n" + 
                                            "1. Square operand | " + "2. Square root of operand | " + 
                                            "3. Inverse of operand | " + "4. Negate operand | " + "5. Continue");

            // Getting user selection:
            int selection = Convert.ToInt32(Console.ReadLine());

            // Switch statement to perform the operation selected on a single operand:
            switch (selection)
            {
                // Square:
                case 1:
                    {
                        currentOperandInExpression = "(" + currentOperandInExpression + ")^2";
                        myOperand = Convert.ToString(Math.Pow(Convert.ToDouble(myOperand), 2));
                        //Console.WriteLine("Current operand: " + currentOperandInExpression + " = " + myOperand);
                        myOperand = operationWithinOperation(myOperand);
                        break;
                    }
                // Square root:
                case 2:
                    {
                        currentOperandInExpression = "sqrt(" + currentOperandInExpression + ")";
                        myOperand = Convert.ToString(Math.Sqrt(Convert.ToDouble(myOperand)));
                        myOperand = operationWithinOperation(myOperand);
                        break;
                    }
                // Inverse:
                case 3:
                    {
                        currentOperandInExpression = "1/(" + currentOperandInExpression + ")";
                        myOperand = Convert.ToString(1 / (Convert.ToDouble(myOperand)));
                        myOperand = operationWithinOperation(myOperand);
                        break;
                    }
                // Negate:
                case 4:
                    {
                        currentOperandInExpression = "negate(" + currentOperandInExpression + ")";
                        double oppositeNumber = Convert.ToDouble(myOperand);
                        oppositeNumber = oppositeNumber * (-1);
                        myOperand = Convert.ToString(oppositeNumber);
                        myOperand = operationWithinOperation(myOperand);
                        break;
                    }
                // Continue:
                case 5:
                    {
                        break;
                    }
                // i.e. Else:
                default:
                    {
                        Console.WriteLine("Wrong action, try again!");
                        myOperand = operationWithinOperation(myOperand);
                        break;
                    }
            }

            return myOperand;
        }

        // Function to perform addition:
        public static void performAddition()
        {
            // Getting first operand from user:
            string firstOperand = getOperandsFromUser();
            currentOperandInExpression = firstOperand;
            firstOperand = operationWithinOperation(currentOperandInExpression);
            currentExpression = currentOperandInExpression + " + ";
            Console.Clear();

            // Getting second operand from user:
            string secondOperand = getOperandsFromUser();
            currentOperandInExpression = secondOperand;
            secondOperand = operationWithinOperation(currentOperandInExpression);
            currentExpression += currentOperandInExpression;
            Console.Clear();

            // Calculating result:
            string myResult = new DataTable().Compute(firstOperand + " + " + secondOperand, null).ToString();
            currentExpression += " = " + myResult;

            // Displaying current expression with result:
            Console.WriteLine("\n" + "Addition:\n" + currentExpression);

            // Adding current expression to history list:
            history.Add(currentExpression);

            // Variable for user selection after addition is done:
            int selection = -1;

            // While loop that executes until user selects a valid option:
            while (selection < 1 || selection > 4)
            {
                // Displaying possible options after addition is done:
                Console.WriteLine("\nSelect an option: 1. Do another addition | 2. Main menu | 3. Exit");

                // Getting user selection:
                selection = Convert.ToInt32(Console.ReadLine());

                // Switch statement to perform user selection:
                switch (selection)
                {
                    // Another addition:
                    case 1:
                        {
                            Console.Clear();
                            performAddition();
                            break;
                        }
                    // Main menu:
                    case 2:
                        {
                            Console.Clear();
                            displayOperationsMenu();
                            break;
                        }
                    // Exit:
                    case 3:
                        {
                            Environment.Exit(0);
                            break;
                        }
                    // i.e. Else:
                    default:
                        {
                            Console.WriteLine("Wrong action, try again!");
                            break;
                        }
                }
            }           
        }

        // Function to perform subtraction:
        public static void performSubtraction()
        {
            // Getting first operand from user:
            string firstOperand = getOperandsFromUser();
            currentOperandInExpression = firstOperand;
            firstOperand = operationWithinOperation(currentOperandInExpression);
            currentExpression = currentOperandInExpression + " - ";
            Console.Clear();

            // Getting second operand from user:
            string secondOperand = getOperandsFromUser();
            currentOperandInExpression = secondOperand;
            secondOperand = operationWithinOperation(currentOperandInExpression);
            currentExpression += currentOperandInExpression;
            Console.Clear();

            // Calculating result:
            string myResult = new DataTable().Compute(firstOperand + " - " + secondOperand, null).ToString();
            currentExpression += " = " + myResult;

            // Displaying current expression with result:
            Console.WriteLine("\n" + "Subtraction:\n" + currentExpression);

            // Adding current expression to history list:
            history.Add(currentExpression);

            // Variable for user selection after subtraction is done:
            int selection = -1;

            // While loop that executes until user selects a valid option:
            while (selection < 1 || selection > 4)
            {
                // Displaying possible options after subtraction is done:
                Console.WriteLine("\nSelect an option: 1. Do another subtraction | 2. Main menu | 3. Exit");

                // Getting user selection:
                selection = Convert.ToInt32(Console.ReadLine());

                // Switch statement to perform user selection:
                switch (selection)
                {
                    // Another Subtraction:
                    case 1:
                        {
                            Console.Clear();
                            performSubtraction();
                            break;
                        }
                    // Main menu:
                    case 2:
                        {
                            Console.Clear();
                            displayOperationsMenu();
                            break;
                        }
                    // Exit:
                    case 3:
                        {
                            Environment.Exit(0);
                            break;
                        }
                    // i.e. Else:
                    default:
                        {
                            Console.WriteLine("Wrong action, try again!");
                            break;
                        }
                }
            }
        }

        // Function to perform multiplication:
        public static void performMultiplication()
        {
            // Getting first operand from user:
            string firstOperand = getOperandsFromUser();
            currentOperandInExpression = firstOperand;
            firstOperand = operationWithinOperation(currentOperandInExpression);
            currentExpression = currentOperandInExpression + " * ";
            Console.Clear();

            // Getting second operand from user:
            string secondOperand = getOperandsFromUser();
            currentOperandInExpression = secondOperand;
            secondOperand = operationWithinOperation(currentOperandInExpression);
            currentExpression += currentOperandInExpression;
            Console.Clear();

            // Calculating result:
            string myResult = new DataTable().Compute(firstOperand + " * " + secondOperand, null).ToString();
            currentExpression += " = " + myResult;

            // Displaying current expression with result:
            Console.WriteLine("\n" + "Multiplication:\n" + currentExpression);

            // Adding current expression to history list:
            history.Add(currentExpression);

            // Variable for user selection after multiplication is done:
            int selection = -1;

            // While loop that executes until user selects a valid option:
            while (selection < 1 || selection > 4)
            {
                // Displaying possible options after multiplication is done:
                Console.WriteLine("\nSelect an option: 1. Do another multiplication | 2. Main menu | 3. Exit");

                // Getting user selection:
                selection = Convert.ToInt32(Console.ReadLine());

                // Switch statement to perform user selection:
                switch (selection)
                {
                    // Another division:
                    case 1:
                        {
                            Console.Clear();
                            performMultiplication();
                            break;
                        }
                    // Main menu:
                    case 2:
                        {
                            Console.Clear();
                            displayOperationsMenu();
                            break;
                        }
                    // Exit:
                    case 3:
                        {
                            Environment.Exit(0);
                            break;
                        }
                    // i.e. Else:
                    default:
                        {
                            Console.WriteLine("Wrong action, try again!");
                            break;
                        }
                }
            }
        }

        // Function to perform division:
        public static void performDivision()
        {
            // Getting first operand from user:
            string firstOperand = getOperandsFromUser();
            currentOperandInExpression = firstOperand;
            firstOperand = operationWithinOperation(currentOperandInExpression);
            currentExpression = currentOperandInExpression + " / ";
            Console.Clear();

            // Getting second operand from user:
            string secondOperand = getOperandsFromUser();
            currentOperandInExpression = secondOperand;
            secondOperand = operationWithinOperation(currentOperandInExpression);
            currentExpression += currentOperandInExpression;
            Console.Clear();

            // Calculating result:
            string myResult = new DataTable().Compute(firstOperand + " / " + secondOperand, null).ToString();
            currentExpression += " = " + myResult;

            // Displaying current expression with result:
            Console.WriteLine("\n" + "Division:\n" + currentExpression);

            // Adding current expression to history list:
            history.Add(currentExpression);

            // Variable for user selection after division is done:
            int selection = -1;

            // While loop that executes until user selects a valid option:
            while (selection < 1 || selection > 4)
            {
                // Displaying possible options after division is done:
                Console.WriteLine("\nSelect an option: 1. Do another division | 2. Main menu | 3. Exit");

                // Getting user selection:
                selection = Convert.ToInt32(Console.ReadLine());

                // Switch statement to perform user selection:
                switch (selection)
                {
                    // Another division:
                    case 1:
                        {
                            Console.Clear();
                            performDivision();
                            break;
                        }
                    // Main menu:
                    case 2:
                        {
                            Console.Clear();
                            displayOperationsMenu();
                            break;
                        }
                    // Exit:
                    case 3:
                        {
                            Environment.Exit(0);
                            break;
                        }
                    // i.e. Else:
                    default:
                        {
                            Console.WriteLine("Wrong action, try again!");
                            break;
                        }
                }
            }
        }

        // Function to perform modulo:
        public static void performModulo()
        {
            // Getting first operand from user:
            string firstOperand = getOperandsFromUser();
            currentOperandInExpression = firstOperand;
            firstOperand = operationWithinOperation(currentOperandInExpression);
            currentExpression = currentOperandInExpression + " % ";
            Console.Clear();

            // Getting second operand from user:
            string secondOperand = getOperandsFromUser();
            currentOperandInExpression = secondOperand;
            secondOperand = operationWithinOperation(currentOperandInExpression);
            currentExpression += currentOperandInExpression;
            Console.Clear();

            // Calculating result:
            string myResult = new DataTable().Compute(firstOperand + " % " + secondOperand, null).ToString();
            currentExpression += " = " + myResult;

            // Displaying current expression with result:
            Console.WriteLine("\n" + "Modulo:\n" + currentExpression);

            // Adding current expression to history list:
            history.Add(currentExpression);

            // Variable for user selection after modulo is done:
            int selection = -1;

            // While loops that executes until user selects a valid option:
            while (selection < 1 || selection > 4)
            {
                // Displaying possible options after modulo is done:
                Console.WriteLine("\nSelect an option: 1. Do another modulo | 2. Main menu | 3. Exit");

                // Getting user selection:
                selection = Convert.ToInt32(Console.ReadLine());

                // Switch statement to perform user selection:
                switch (selection)
                {
                    // Another modulo:
                    case 1:
                        {
                            Console.Clear();
                            performModulo();
                            break;
                        }
                    // Main menu:
                    case 2:
                        {
                            Console.Clear();
                            displayOperationsMenu();
                            break;
                        }
                    // Exit:
                    case 3:
                        {
                            Environment.Exit(0);
                            break;
                        }
                    // i.e. Else:
                    default:
                        {
                            Console.WriteLine("Wrong action, try again!");
                            break;
                        }
                }
            }
        }

        // Function to display history:
        public static void displayHistory()
        {
            Console.Clear();

            if (history.Count == 0)
            {
                Console.WriteLine("No history");
            }
            else
            {
                // Displaying each expression stored in the history:
                for (int i = 0; i < history.Count; i++)
                {
                    Console.WriteLine(history[i] + "\n");
                }
            }
        }

        // Function to display operations menu:
        public static void displayOperationsMenu()
        {
            Console.WriteLine("\n\nCONSOLE-BASED CALCULATOR");
            Console.WriteLine("-------------------------\n\n");

            // Displaying initial menu with different operations:
            Console.WriteLine("1) Addition");
            Console.WriteLine("2) Subtraction");
            Console.WriteLine("3) Multiplication");
            Console.WriteLine("4) Division");
            Console.WriteLine("5) Modulo");
            Console.WriteLine("6) Square of a number");
            Console.WriteLine("7) Square root of a number");
            Console.WriteLine("8) Inverse of a number");
            Console.WriteLine("9) History");
            Console.WriteLine("0) Exit\n");

            // Getting user input to select an operation:
            Console.WriteLine("Select an option:");
            int operation = Convert.ToInt32(Console.ReadLine());

            // Switch statement to perform the operation selected:
            switch (operation)
            {
                // Addition:
                case 1:
                    {
                        Console.Clear();
                        performAddition();
                        break;
                    }
                // Subtraction:
                case 2:
                    {
                        Console.Clear();
                        performSubtraction();
                        break;
                    }
                // Multiplication:
                case 3:
                    {
                        Console.Clear();
                        performMultiplication();
                        break;
                    }
                // Division:
                case 4:
                    {
                        Console.Clear();
                        performDivision();
                        break;
                    }
                // Modulo:
                case 5:
                    {
                        Console.Clear();
                        performModulo();
                        break;
                    }
                // Square of a number:
                case 6:
                    {
                        Console.Clear();
                        findSquare();
                        break;
                    }
                // Square root of a number:
                case 7:
                    {
                        Console.Clear();
                        performDivision();
                        break;
                    }
                // Inverse of a number:
                case 8:
                    {
                        Console.Clear();
                        performModulo();
                        break;
                    }
                // History:
                case 9:
                    {
                        displayHistory();
                        break;
                    }
                // Exit:
                case 0:
                    {
                        Environment.Exit(0);
                        break;
                    }
                // i.e. Else:
                default:
                    {
                        Console.WriteLine("Wrong action, try again!");
                        displayOperationsMenu();
                        break;
                    }
            }
        }

        // Main (i.e. driver) function:
        static void Main(string[] args)
        {
            displayOperationsMenu();
        }
    }
}