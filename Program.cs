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

        // Variable to track first operand:
        public static string firstOperand = "";

        // Variable to track second operand:
        public static string secondOperand = "";

        // Variable to track current operand to use in expression:
        public static string currentOperandInExpression = "";

        // Variable to track current expression to evaluate:
        public static string currentExpression = "";

        // Variable to track result of an expression:
        public static string myResult = "";

        // Boolean function to check if input string contains only digits:
        public static bool isStringDigitsOnly(string myString)
        {
            foreach (char c in myString)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            if (myString == "" || myString == null)
                return false;

            return true;
        }

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
            string myOperand = "";

            // Until operand is not a valid number, keep trying again:
            while (isStringValidDouble(myOperand) == false)
            {
                Console.Clear();

                // Getting user input for operand:
                if (firstOperand == "")
                {
                    Console.WriteLine("ENTER FIRST OPERAND:\n");
                }
                else
                {
                    Console.WriteLine("CURRENT EXPRESSION:       " + currentExpression + "[second operand]\n\n");
                    Console.WriteLine("ENTER SECOND OPERAND:\n");
                }

                myOperand = Convert.ToString(Console.ReadLine());
            }

            return myOperand;
        }

        // Function to find square, square root, or inverse of operand within an operation:
        public static string operationWithinOperation(string myOperand)
        {
            Console.Clear();

            // Displaying menu to perform an operation on single operand:
            Console.WriteLine("SELECT OPTION FOR CURRENT OPERAND: " + currentOperandInExpression + " = " + myOperand + "\n\n" + 
                                            "1. Square operand | " + "2. Square root of operand | " + 
                                            "3. Inverse of operand | " + "4. Negate operand | " + "5. Continue\n");

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
                        Console.WriteLine("INVALID INPUT!");
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
            if (firstOperand == "")
            {
                firstOperand = getOperandsFromUser();
            }
            currentOperandInExpression = firstOperand;
            Console.Clear();
            firstOperand = operationWithinOperation(currentOperandInExpression);
            currentExpression = currentOperandInExpression + " " + myOperator + " ";
            Console.Clear();

            // Getting second operand from user:
            if (secondOperand == "")
            {
                secondOperand = getOperandsFromUser();
            }
            currentOperandInExpression = secondOperand;
            Console.Clear();
            secondOperand = operationWithinOperation(currentOperandInExpression);
            currentExpression += currentOperandInExpression;
            Console.Clear();

            // If any of the operands is not a valid double, then throw an error message:
            if (isStringValidDouble(firstOperand) == false || isStringValidDouble(secondOperand) == false)
            {
                // Displaying error message:
                Console.WriteLine("ERROR! INVALID " + operationType + ":\n\n" + currentExpression);

                // Resetting operands for next expression:
                firstOperand = "";
                secondOperand = "";
                currentOperandInExpression = "";
                currentExpression = "";

                // Display basic menu with option to display main menu or exit program:
                displayBasicMenu();
            }
            // If you are doing modulo operation and any of the operands is not an integer, then throw an error message:
            else if (myOperator == "%" && (firstOperand.Contains(".") || secondOperand.Contains(".")))
            {
                // Displaying error message:
                Console.WriteLine("ERROR! INVALID " + operationType + ":\n\n" + currentExpression);

                // Resetting operands for next expression:
                firstOperand = "";
                secondOperand = "";
                currentOperandInExpression = "";
                currentExpression = "";

                // Display basic menu with option to display main menu or exit program:
                displayBasicMenu();
            }
            else
            {
                // Calculating result based on operation type selected:
                myResult = new DataTable().Compute(firstOperand + " " + myOperator + " " + secondOperand, null).ToString();
                currentExpression += " = " + myResult;

                // Displaying current expression with result:
                Console.WriteLine(operationType + ":\n\n" + currentExpression);

                // Adding current expression to history list:
                history.Add(currentExpression);

                // Resetting operands for next expression:
                firstOperand = "";
                secondOperand = "";
                currentOperandInExpression = "";
                currentExpression = "";

                // Shwoing menu with possible operations to perform with result of current expression:
                operationWithResult();
            }
        }

        // Function to perform operation with one operand:
        // (i.e. square, square root, or inverse)
        public static void operationWithOneOperand(string operationType)
        {
            // Getting operand from user:
            if (firstOperand == "")
            {

                firstOperand = getOperandsFromUser();
            }
            currentOperandInExpression = firstOperand;
            firstOperand = operationWithinOperation(currentOperandInExpression);

            // Calculating and displaying expression with its result:
            Console.Clear();
            Console.WriteLine("THE EXPRESSION " + operationType + "(" + currentOperandInExpression + ") EVALUATES TO:");

            // Performing calculations based on operation type selected:
            if (operationType == "Square")
            {
                myResult = Convert.ToString(Math.Pow(Convert.ToDouble(firstOperand), 2));
                currentOperandInExpression = "(" + currentOperandInExpression + ")^2";
            }
            else if (operationType == "Sqrt")
            {
                myResult = Convert.ToString(Math.Sqrt(Convert.ToDouble(firstOperand)));
                currentOperandInExpression = "Sqrt(" + currentOperandInExpression + ")";
            }
            else if (operationType == "Inverse")
            {
                myResult = Convert.ToString(1 / (Convert.ToDouble(firstOperand)));
                currentOperandInExpression = "1/(" + currentOperandInExpression + ")";
            }
            else if (operationType == "Negate")
            {
                double oppositeNumber = Convert.ToDouble(firstOperand);
                oppositeNumber = oppositeNumber * (-1);
                firstOperand = Convert.ToString(oppositeNumber);
                myResult = firstOperand;
                currentOperandInExpression = "Negate(" + currentOperandInExpression + ")";
            }

            currentExpression = currentOperandInExpression + " = " + myResult;
            Console.WriteLine("\n" + currentExpression);

            // Adding current expression to history list:
            history.Add(currentExpression);

            // Resetting operands for next expression:
            firstOperand = "";
            secondOperand = "";
            currentOperandInExpression = "";
            currentExpression = "";

            // Shwoing menu with possible operations to perform with result of current expression:
            operationWithResult();
        }

        // Function to perform an operation with the result of previous expression:
        public static void operationWithResult()
        {
            // Variable to track user selection:
            int selection = -1;

            // While loop that executes until user selects a valid option:
            while (selection < 0 || selection > 11)
            {
                // Displaying possible options after operation is done:
                Console.WriteLine("\n\nSELECT OPTION FOR CURRENT RESULT:");
                Console.WriteLine("----------------------------------------\n");

                // Displaying different operations that can be performed with result:
                Console.WriteLine("1)   Addition using current number");
                Console.WriteLine("2)   Subtraction using current number");
                Console.WriteLine("3)   Multiplication using current number");
                Console.WriteLine("4)   Division using current number");
                Console.WriteLine("5)   Modulo using current number");
                Console.WriteLine("6)   Square current number");
                Console.WriteLine("7)   Square root current number");
                Console.WriteLine("8)   Inverse current number");
                Console.WriteLine("9)   Negate current number");
                Console.WriteLine("10)  History");
                Console.WriteLine("11)  Main Menu");
                Console.WriteLine("0)   Exit\n");

                // Getting user selection:
                selection = Convert.ToInt32(Console.ReadLine());

                // Switch statement to perform the operation selected:
                switch (selection)
                {
                    // Addition:
                    case 1:
                        {
                            Console.Clear();
                            firstOperand = myResult;
                            operationWithTwoOperands("ADDITION", "+");
                            break;
                        }
                    // Subtraction:
                    case 2:
                        {
                            Console.Clear();
                            firstOperand = myResult;
                            operationWithTwoOperands("SUBTRACTION", "-");
                            break;
                        }
                    // Multiplication:
                    case 3:
                        {
                            Console.Clear();
                            firstOperand = myResult;
                            operationWithTwoOperands("MULTIPLICATION", "*");
                            break;
                        }
                    // Division:
                    case 4:
                        {
                            Console.Clear();
                            firstOperand = myResult;
                            operationWithTwoOperands("DIVISION", "/");
                            break;
                        }
                    // Modulo:
                    case 5:
                        {
                            Console.Clear();
                            firstOperand = myResult;
                            operationWithTwoOperands("MODULO", "%");
                            break;
                        }
                    // Square of a number:
                    case 6:
                        {
                            Console.Clear();
                            firstOperand = myResult;
                            operationWithOneOperand("Square");
                            break;
                        }
                    // Square root of a number:
                    case 7:
                        {
                            Console.Clear();
                            firstOperand = myResult;
                            operationWithOneOperand("Sqrt");
                            break;
                        }
                    // Inverse of a number:
                    case 8:
                        {
                            Console.Clear();
                            firstOperand = myResult;
                            operationWithOneOperand("Inverse");
                            break;
                        }
                    // Negate a number:
                    case 9:
                        {
                            Console.Clear();
                            firstOperand = myResult;
                            operationWithOneOperand("Negate");
                            break;
                        }
                    // History:
                    case 10:
                        {
                            Console.Clear();
                            displayHistory();
                            break;
                        }
                    // Main menu:
                    case 11:
                        {
                            Console.Clear();
                            displayMainMenu();
                            break;
                        }
                    // Exit:
                    case 0:
                        {
                            Console.Clear();
                            Environment.Exit(0);
                            break;
                        }
                    // i.e. Else:
                    default:
                        {
                            Console.WriteLine("INVALID INPUT!");
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
                Console.WriteLine("NO HISTORY");
            }
            else
            {
                // Displaying each expression stored in the history:
                for (int i = 0; i < history.Count; i++)
                {
                    Console.WriteLine(history[i] + "\n");
                }
            }

            // Display basic menu with option to display main menu or exit program:
            displayBasicMenu();
        }

        // Function to display basic menu:
        public static void displayBasicMenu()
        {
            // Variable for user selection after addition is done:
            int selection = -1;

            // While loop that executes until user selects a valid option:
            while (selection < 1 || selection > 2)
            {
                // Displaying possible options:
                Console.WriteLine("\n\nSELECT OPTION: 1) Main Menu | 2) Exit");

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
                            Console.WriteLine("INVALID INPUT!");
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
            Console.WriteLine("1)   Addition");
            Console.WriteLine("2)   Subtraction");
            Console.WriteLine("3)   Multiplication");
            Console.WriteLine("4)   Division");
            Console.WriteLine("5)   Modulo");
            Console.WriteLine("6)   Square");
            Console.WriteLine("7)   Square root");
            Console.WriteLine("8)   Inverse");
            Console.WriteLine("9)   Negate");
            Console.WriteLine("10)  History");
            Console.WriteLine("0)   Exit\n\n");

            // Getting user input to select an operation:
            Console.WriteLine("SELECT OPTION:");
            string selectionString = Console.ReadLine();
            if (isStringDigitsOnly(selectionString) == false)
            {
                Console.WriteLine("\nINVALID INPUT!");
                Thread.Sleep(1000); // Delay of 1000 ms
                Console.Clear();
                displayMainMenu();
                
            }
            else
            {
                int selection = Convert.ToInt32(selectionString);
                // Switch statement to perform the operation selected:
                switch (selection)
                {
                    // Addition:
                    case 1:
                        {
                            Console.Clear();
                            operationWithTwoOperands("ADDITION", "+");
                            break;
                        }
                    // Subtraction:
                    case 2:
                        {
                            Console.Clear();
                            operationWithTwoOperands("SUBTRACTION", "-");
                            break;
                        }
                    // Multiplication:
                    case 3:
                        {
                            Console.Clear();
                            operationWithTwoOperands("MULTIPLICATION", "*");
                            break;
                        }
                    // Division:
                    case 4:
                        {
                            Console.Clear();
                            operationWithTwoOperands("DIVISION", "/");
                            break;
                        }
                    // Modulo:
                    case 5:
                        {
                            Console.Clear();
                            operationWithTwoOperands("MODULO", "%");
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
                            Console.Clear();
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
                            Console.WriteLine("\nINVALID INPUT!");
                            Thread.Sleep(1000); // Delay of 1000 ms
                            Console.Clear();
                            displayMainMenu();
                            break;
                        }
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