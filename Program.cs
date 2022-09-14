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

        // Variable to track result of an expression:
        public static string myResult = "";

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
                Console.WriteLine("Invalid operand!");
                Console.WriteLine("\nEnter operand:");
                myOperand = Convert.ToString(Console.ReadLine());
            }

            return myOperand;
        }

        // Function to find square, square root, or inverse of operand within an operation:
        public static string operationWithinOperation(string myOperand)
        {
            // Displaying menu to perform an operation on single operand:
            Console.WriteLine("\nSELECT OPTION FOR CURRENT OPERAND: " + currentOperandInExpression + " = " + myOperand + "\n" + 
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
                        myOperand = operationWithinOperation(myOperand);
                        break;
                    }
                // Square root:
                case 2:
                    {
                        currentOperandInExpression = "Sqrt(" + currentOperandInExpression + ")";
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
                        currentOperandInExpression = "Negate(" + currentOperandInExpression + ")";
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

        // Function to perform operation with two operands:
        // (i.e. addition, subtraction, multiplication, division, or modulo)
        public static void operationWithTwoOperands(string operationType, string myOperator)
        {
            // Getting first operand from user:
            string firstOperand = getOperandsFromUser();
            currentOperandInExpression = firstOperand;
            firstOperand = operationWithinOperation(currentOperandInExpression);
            currentExpression = currentOperandInExpression + " " + myOperator + " ";
            Console.Clear();

            // Getting second operand from user:
            string secondOperand = getOperandsFromUser();
            currentOperandInExpression = secondOperand;
            secondOperand = operationWithinOperation(currentOperandInExpression);
            currentExpression += currentOperandInExpression;
            Console.Clear();

            // If any of the operands is NaN, then throw an error message:
            if (firstOperand == "NaN" || secondOperand == "NaN")
            {
                // Displaying error message:
                Console.WriteLine("\n" + "ERROR! Invalid operand, " + operationType + " cannot be performed:\n" + currentExpression);
            }
            else
            {
                // Calculating result based on operation type selected:
                myResult = new DataTable().Compute(firstOperand + " " + myOperator + " " + secondOperand, null).ToString();
                currentExpression += " = " + myResult;

                // Displaying current expression with result:
                Console.WriteLine("\n" + operationType + "\n" + currentExpression);

                // Adding current expression to history list:
                history.Add(currentExpression);
            }

            // Variable for user selection after operation is done:
            int selection = -1;

            // While loop that executes until user selects a valid option:
            while (selection < 1 || selection > 4)
            {
                // Displaying possible options after operation is done:
                Console.WriteLine("\nSELECT OPTION: 1. Do another " + operationType + " | 2. Main menu | 3. Exit");

                // Getting user selection:
                selection = Convert.ToInt32(Console.ReadLine());

                // Switch statement to perform user selection:
                switch (selection)
                {
                    // Repeat same operation type:
                    case 1:
                        {
                            Console.Clear();
                            operationWithTwoOperands(operationType, myOperator);
                            break;
                        }
                    // Main menu:
                    case 2:
                        {
                            Console.Clear();
                            displayMainMenu();
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

        // Function to perform operation with one operand:
        // (i.e. square, square root, or inverse)
        public static void operationWithOneOperand(string operationType)
        {
            // Getting operand from user:
            string operand = getOperandsFromUser();
            currentOperandInExpression = operand;
            operand = operationWithinOperation(currentOperandInExpression);

            // Calculating and displaying expression with its result:
            Console.Clear();
            Console.WriteLine("The expression " + operationType + "(" + currentOperandInExpression + ") evaluates to:");

            // Performing calculations based on operation type selected:
            if (operationType == "Square")
            {
                myResult = Convert.ToString(Math.Pow(Convert.ToDouble(operand), 2));
                currentOperandInExpression = "(" + currentOperandInExpression + ")^2";
            }
            else if (operationType == "Sqrt")
            {
                myResult = Convert.ToString(Math.Sqrt(Convert.ToDouble(operand)));
                currentOperandInExpression = "Sqrt(" + currentOperandInExpression + ")";
            }
            else if (operationType == "Inverse")
            {
                myResult = Convert.ToString(1 / (Convert.ToDouble(operand)));
                currentOperandInExpression = "1/(" + currentOperandInExpression + ")";
            }
            else if (operationType == "Negate")
            {
                double oppositeNumber = Convert.ToDouble(operand);
                oppositeNumber = oppositeNumber * (-1);
                operand = Convert.ToString(oppositeNumber);
                myResult = operand;
                currentOperandInExpression = "Negate(" + currentOperandInExpression + ")";
            }

            currentExpression = currentOperandInExpression + " = " + myResult;
            Console.WriteLine("\n" + currentExpression);

            // Adding current expression to history list:
            history.Add(currentExpression);

            // Variable for user selection after operation is done:
            int selection = -1;

            // While loop that executes until user selects a valid option:
            while (selection < 1 || selection > 3)
            {
                // Displaying possible options after operation is done:
                Console.WriteLine("\nSELECT OPTION: 1. " + operationType + " another number | 2. Main menu | 3. Exit");

                // Getting user selection:
                selection = Convert.ToInt32(Console.ReadLine());

                // Switch statement to perform user selection:
                switch (selection)
                {
                    // Repeat same operation type:
                    case 1:
                        {
                            Console.Clear();
                            operationWithOneOperand(operationType);
                            break;
                        }
                    // Main menu:
                    case 2:
                        {
                            Console.Clear();
                            displayMainMenu();
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

            // Variable for user selection after addition is done:
            int selection = -1;

            // While loop that executes until user selects a valid option:
            while (selection < 1 || selection > 2)
            {
                // Displaying possible options after square is found:
                Console.WriteLine("\nSelect an option: 1. Main menu | 2. Exit");

                // Getting user selection:
                selection = Convert.ToInt32(Console.ReadLine());

                // Switch statement to perform user selection:
                switch (selection)
                {
                    // Main menu:
                    case 1:
                        {
                            Console.Clear();
                            displayMainMenu();
                            break;
                        }
                    // Exit:
                    case 2:
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

        // Function to display operations menu:
        public static void displayMainMenu()
        {
            Console.WriteLine("CONSOLE-BASED CALCULATOR");
            Console.WriteLine("-------------------------\n\n");

            // Displaying initial menu with different operations:
            Console.WriteLine("1) Addition");
            Console.WriteLine("2) Subtraction");
            Console.WriteLine("3) Multiplication");
            Console.WriteLine("4) Division");
            Console.WriteLine("5) Modulo");
            Console.WriteLine("6) Square a number");
            Console.WriteLine("7) Square root a number");
            Console.WriteLine("8) Inverse a number");
            Console.WriteLine("9) Negate a number");
            Console.WriteLine("10) History");
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
                        operationWithTwoOperands("Addition", "+");
                        break;
                    }
                // Subtraction:
                case 2:
                    {
                        Console.Clear();
                        operationWithTwoOperands("Subtraction", "-");
                        break;
                    }
                // Multiplication:
                case 3:
                    {
                        Console.Clear();
                        operationWithTwoOperands("Multiplication", "*");
                        break;
                    }
                // Division:
                case 4:
                    {
                        Console.Clear();
                        operationWithTwoOperands("Division", "/");
                        break;
                    }
                // Modulo:
                case 5:
                    {
                        Console.Clear();
                        operationWithTwoOperands("Modulo", "%");
                        break;
                    }
                // Square of a number:
                case 6:
                    {
                        Console.Clear();
                        operationWithOneOperand("Square");
                        break;
                    }
                // Square root of a number:
                case 7:
                    {
                        Console.Clear();
                        operationWithOneOperand("Sqrt");
                        break;
                    }
                // Inverse of a number:
                case 8:
                    {
                        Console.Clear();
                        operationWithOneOperand("Inverse");
                        break;
                    }
                // Negate a number:
                case 9:
                    {
                        Console.Clear();
                        operationWithOneOperand("Negate");
                        break;
                    }
                // History:
                case 10:
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
                        displayMainMenu();
                        break;
                    }
            }
        }

        // Main (i.e. driver) function:
        static void Main(string[] args)
        {
            displayMainMenu();
        }
    }
}