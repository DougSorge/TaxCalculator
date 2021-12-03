using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TaxCalculatorProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Thank you for choosing this Tax Calculator. \r\n");


            bool programRunning = true;
            bool verbose = false;
            string userChoice = "";
            string userState = "";
            double userIncome = 0;
            while (programRunning)
            {
                Console.WriteLine("Please Enter a choice exactly as it appears from the Menu Below:");
                try
                {
                    Console.WriteLine("Enter A to display all State Tax data. \nEnter C to enter Tax Calculator. \nEnter E to see Employee Information");
                    userChoice = Console.ReadLine().ToLower();
                    if (userChoice == "a")
                    {
                        Console.WriteLine("\nThe Following Errors Were Found in the State Tax Data...\nCorrect Data Appears After Error Log:\r\n");
                        TaxCalculator.PrintDictionary();
                    }
                    else if (userChoice == "c")
                    {
                        //prompt for verbose mode
                        Console.WriteLine("Would You like to activate Painstaking Detail Mode? Type Yes or No");
                        if (Console.ReadLine().ToLower() == "yes")
                        {
                            verbose = true;
                        }

                        //prompt user for state code and income level.
                        Console.Write("Enter State Abbreviation Here: ");
                        userState = Console.ReadLine().ToUpper();
                        Console.Write("\rEnter Income Here: ");

                        //make sure user entry was a number
                        while (!double.TryParse(Console.ReadLine(), out userIncome))
                        {
                            Console.Write("\rEnter Income Here (Must be numbers only): ");
                        };


                        Console.WriteLine("\r\nCalculating Taxes For You...");

                        Console.WriteLine("\nThe Following Errors Were Found in the State Tax Data...\nCorrect Data Appears After Error Log:\r\n");
                        Console.WriteLine($"{TaxCalculator.ComputeTaxFor(userState, userIncome, verbose)}");
                        
                    }
                    else if(userChoice == "e")
                    {
                        string userChoiceEmpMenu;
                        Console.WriteLine("\nWelcome to the Employee Tax Info Database. \nEnter A to see all available data. \nEnter S to search for particular data.");
                        userChoiceEmpMenu = Console.ReadLine();
                        if (userChoiceEmpMenu.ToLower() == "a")
                        {
                            Console.WriteLine("\nThe Following Errors Were Found in the State Tax Data and Employee Data repectively...\nCorrect Employee Data Appears After Error Log:\r\n");
                            EmployeeList.PrintList();
                        
                        } else if(userChoiceEmpMenu.ToLower() =="s"){
                            bool search = true;

                            //Declaring IEnumerable needed for requesting and storing queries. 
                            IEnumerable<EmployeeRecord> QueryRequest = null;
                       

                            //Prompting for user choice again to determine what to sort by.
                            Console.WriteLine("\nYou have chosen to perform a search.");


                            while (search)
                            {
                                //make user select an option.
                                Console.WriteLine("Enter ID to filter by ID\nEnter N to filter by Name\nEnter S to filter by State Code\nEnter HW to filter by Hours Worked\nEnter HP to filter by Hourly Pay\nQ to Cancel Search");
                                userChoiceEmpMenu = Console.ReadLine().ToLower();

                                //ensure choice is valid.
                                while (userChoiceEmpMenu != "id" && userChoiceEmpMenu != "n" && userChoiceEmpMenu != "s" && userChoiceEmpMenu != "hw" && userChoiceEmpMenu != "hp" && userChoiceEmpMenu != "q")
                                {
                                    Console.WriteLine("Not Recognized....\nEnter ID to filter by ID\nEnter N to filter by Name\nEnter S to filter by State Code\nEnter HW to filter by Hours Worked\nEnter HP to filter by Hourly Pay\nQ to Cancel Search");
                                    userChoiceEmpMenu = Console.ReadLine().ToLower();
                                }

                                if (userChoiceEmpMenu == "q")
                                {
                                    search = false;
                                    continue;
                                }

                                //make user choose which direction to sort by. Ascending is default.
                                Console.WriteLine("\nNow Enter a direction to sory by:\nEnter A for ascending\nEnter D for descending");
                                string sortOrder = Console.ReadLine().ToLower();

                                //clarify printout
                                Console.WriteLine("\nThe Following Errors were found in the Tax Table and Employee Table respectively. Search results are printed following the error log.\n");

                                //switch statement allows us to sort by column name by using LINQ functionality of c#. The IEnumerable of type EmployeeRecord is used to house the data returned by the LINQ query. 'from' signifies the start of the query. 'employee' is called a range variable and acts as the i in a foreach loop. 'in' declares what iterable we are searching in, this time it is the list named Employees inside of EmployeeList class. 'orderby' is where we determine hwat field or property we are sorting by and is automatically returned in ascending order. 'select' returns the entire query (I think). 
                                switch (userChoiceEmpMenu)
                                {
                                    case "id": QueryRequest = from employee in EmployeeList.Employees orderby employee.ID select employee; break;
                                    case "n":
                                        QueryRequest = from employee in EmployeeList.Employees orderby employee.Name select employee;
                                        break;
                                    case "s":
                                        QueryRequest = from employee in EmployeeList.Employees orderby employee.StateCode select employee;
                                        break;
                                    case "hw":
                                        QueryRequest = from employee in EmployeeList.Employees orderby employee.HoursWorked select employee;
                                        break;
                                    case "hp":
                                        QueryRequest = from employee in EmployeeList.Employees orderby employee.HourlyRate select employee;
                                        break;
                                     
                                    default:
                                        Console.WriteLine("Choice not recognized, sorting by ID as default.\n");
                                        QueryRequest = from employee in EmployeeList.Employees orderby employee.ID select employee;
                                        break;
                                }
                              
                                //simple if else that will reverse the QeuryRequest enumerable if descending order is desired.
                                if(sortOrder == "d") { 
                                    QueryRequest = QueryRequest.Reverse();
                                    foreach (var employee in QueryRequest)
                                    {
                                        Console.WriteLine(employee);
                                    }
                                } else
                                {
                                    foreach (var employee in QueryRequest)
                                    {
                                        Console.WriteLine(employee);
                                    }
                                }
                              
                                //asks if we'd like to perform another search or quit to main menu.
                                Console.WriteLine("\nPress Q to quit Search Program. Enter anything to perform another search.");
                                if(Console.ReadLine().ToLower() == "q")
                                {
                                    search = false;
                                }
                            }
                        }

                    }
                    else
                    {
                        throw new Exception("Try Again");
                    }
                    Console.WriteLine("\r\nEnter Q to quit main program. Enter anything else to run program once more.");
                    if (Console.ReadLine().ToUpper() == "Q")
                    {
                        Console.WriteLine("Have a nice life!");
                        programRunning = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


 

        }
    }

    //Tax Calculator houses all of the logic for calculating the taxes and populating the dictionary with our tax data.
    static class TaxCalculator
    {
        private const string Path = @"C:\Users\13475\Desktop\dotNet\Projects\TaxCalculator\taxtable.csv";
        static public Dictionary<string, List<TaxRecord>> TaxTable = new Dictionary<string, List<TaxRecord>>();


        static TaxCalculator()
        {
            try
            {
                StreamReader file = File.OpenText(Path);
                int lineNumber = 1;


                //declare an unintialized List variable of TaxRecord type that will be used to accept our TaxRecord class instances.
                List<TaxRecord> taxTableEntryValues;

                while (!file.EndOfStream)
                {
                    try
                    {
                        //create a string from file line.
                        string taxLine = file.ReadLine();

                        /*I moved this out of the while Loop and the program still has the same functionality. Redeclaring this variable dozens of times seemed like a mega waste of resources.
                        List<TaxRecord> taxTableEntryValues;*/

                        //feed taxLine and line number into TaxRecord Constructor to form a new tax record object.
                        TaxRecord taxTableEntry = new TaxRecord(taxLine, lineNumber);

                        //checking to avoid duplicate pairs. Upon first Loop we wont find the key so we will create a new List object.
                        //On subsequent loops, Whenever we encounter a new State Code we will create an additional new list.
                        if (!TaxTable.TryGetValue(taxTableEntry.StateCode, out taxTableEntryValues))
                        {
                            //create a new list instance of type taxRecord to be populated with the required TaxRecord data to be passed into the Dictionary along with the new key.
                            taxTableEntryValues = new List<TaxRecord>();

                            //Append the current taxRecord object to the List.
                            taxTableEntryValues.Add(taxTableEntry);

                            //Adding the state code as a key and the tax values as the value.
                            TaxTable.Add(taxTableEntry.StateCode, taxTableEntryValues);
                        }
                        //Our else block is invoked when we see that the current state code is already in the Dictionary. We are just adding another tax record object to the List of values associated with current key.
                        else
                        {
                            taxTableEntryValues.Add(taxTableEntry);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    //increasing the line number by one which aids with exception tracking.
                    lineNumber++;
                }
                
                file.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }


        }

        //Print Function to check if data has been parsed correctly and to ensure errors were caught.
        public static void PrintDictionary()
        {
            //loop over each jey in the dictionary
            foreach (var kvp in TaxTable)
            {
                //log the key
                Console.WriteLine($"{kvp.Key}");

                //Our kvp.value in this case is a List of objects. Loop over each entry in the List associated with the current key. Utilize the overridden ToString function in TaxRecord class to output the Data. "data" in this case represents a TaxRecord instantiation.
                foreach (var data in kvp.Value)
                {
                    Console.WriteLine($"{data}");
                }
                Console.WriteLine();
            }
        }

        //This is for calculating employee taxes due. The ComputeTaxFor function below is not applicable to our objective. Obviously that's not a good example of functional programming and resuability but we are in a bit too deep and I'm very happy with the ComputeTaxFor functions capabilities. We needed to copy the logic over and remove many of the Console.Write lines because the additional information totally blows up the data presentation of the Employee list making things impossible to decipher.
        public static double ComputeTaxForEmployee(string stateCode, double income)
        {
            //decalre variable to tally up taxes for each bracket.
            double computedTax = 0;

            //Declare list of type tax record to store the value pulled from the trygetvalue function below
            List<TaxRecord> requestedTaxData;

            //if statement to check if state code provided by user is within the dictionary.
            try
            {
                if (TaxTable.TryGetValue(stateCode, out requestedTaxData))
                {
                    for (int i = 0; i < requestedTaxData.Count; i++)
                    {
                        if (income >= requestedTaxData[i].LowerBound && income <= requestedTaxData[i].UpperBound)
                        {
                            computedTax += (income - requestedTaxData[i].LowerBound) * requestedTaxData[i].Rate;
                        }

                        //if the income is greater than the current bracket we calculate the maximum tax for exceeded bracket.
                        else if (income > requestedTaxData[i].UpperBound)
                        {
                            computedTax += (requestedTaxData[i].UpperBound - requestedTaxData[i].LowerBound) * requestedTaxData[i].Rate;
                        }
                    }
                }
                //if we are unable to find the state code in the dictionary we print a simple message informing the user that they have entered an incorrect choice. 
                else
                {
                   throw new Exception($"\r\nEmployee List Error: StateCode '{stateCode}' could not be found in our database...");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //return the computed tax rounded to two decimals.
            return Math.Round(computedTax, 2);
        }

        //This function was the function created for part 1 and includes many Console statements and data displays that work well for letting the user request specific state data.
        public static double ComputeTaxFor(string stateCode, double income, bool verbose)
        {
            //decalre variable to tally up taxes for each bracket.
            double computedTax = 0;

            //Declare list of type tax record to store the value pulled from the trygetvalue function below
            List<TaxRecord> requestedTaxData;


             //if we are unable to find the state code in the dictionary we print a simple message informing the user that they have entered an incorrect choice 
            if(!TaxTable.TryGetValue(stateCode, out requestedTaxData))
            {
                Console.WriteLine($"\r\nStateCode {stateCode} could not be found in our database...");
                return 0.00;

            }
            //else if statement to check if state code provided by user is within the dictionary.
           else if (TaxTable.TryGetValue(stateCode, out requestedTaxData))
            {
                //We are being nice and outputting the data for the requested state.
                Console.WriteLine($"\r\nTax Brackets and Rates for the State of {requestedTaxData[0].StateName}:");

                foreach (TaxRecord taxRecord in requestedTaxData)
                {
                    Console.WriteLine(taxRecord);
                }

                if (verbose == false)
                {
                    //still within the affirmative if check we are looping over the values associated with our key and checking the provided income  against the tax brackets. If the income is within a tax bracket we calculate the data for that bracket.
                    for (int i = 0; i < requestedTaxData.Count; i++)
                    {
                        if (income >= requestedTaxData[i].LowerBound && income <= requestedTaxData[i].UpperBound)
                        {
                            computedTax += (income - requestedTaxData[i].LowerBound) * requestedTaxData[i].Rate;
                        }

                        //if the income is greater than the current bracket we calculate the maximum tax for exceeded bracket.
                        else if (income > requestedTaxData[i].UpperBound)
                        {
                            computedTax += (requestedTaxData[i].UpperBound - requestedTaxData[i].LowerBound) * requestedTaxData[i].Rate;
                        }
                    }
                }
                //adding print statements to describe what is happening each step of the way.
                else if (verbose == true)
                {
                    Console.WriteLine("We will be comparing the income amount you supplied to each Tax Bracket for your chosen state. ");
                    for (int i = 0; i < requestedTaxData.Count; i++)
                    {
                        if (income >= requestedTaxData[i].LowerBound && income <= requestedTaxData[i].UpperBound)
                        {
                            computedTax += (income - requestedTaxData[i].LowerBound) * requestedTaxData[i].Rate;
                            Console.WriteLine($"Income falls within bracket number {i + 1}. Subtracting {income} from {requestedTaxData[i].LowerBound} and multiplying that figure by {requestedTaxData[i].Rate}.");
                            Console.WriteLine($"Current total Taxes = {computedTax}");
                        }

                        //if the income is greater than the current bracket we calculate the maximum tax for exceeded bracket.
                        else if (income > requestedTaxData[i].UpperBound)
                        {
                            computedTax += (requestedTaxData[i].UpperBound - requestedTaxData[i].LowerBound) * requestedTaxData[i].Rate;
                            Console.WriteLine($"Income exceeds cap for this tax bracket. Calculating maximum tax amount by subtracting { requestedTaxData[i].LowerBound} from {requestedTaxData[i].UpperBound} and multiplying it by {requestedTaxData[i].Rate}");
                            Console.WriteLine($"Current total Taxes = {computedTax}");
                        }
                    }
                }

            }
           
            Console.Write("Total Taxes for your state and income level are: ");



            //return the computed tax rounded to two decimals.
            return Math.Round(computedTax, 2);
        }
    }

    //TaxRecord class is used to create entries that will ultimately be inserted to the dictionary found within the TaxCalculator class. 
    public class TaxRecord
    {
        //set all fields as private to store the data.
        private string _StateCode = "";
        private string _StateName = "";
        private double _LowerBound = 0;
        private double _UpperBound = 0;
        private double _Rate = 0;

        //set up properties that will get and set the private fields. This is useful for flexibility purposes. The manipulation of the data can be adjusted easier without giving access to the actual data store.
        public string StateCode { get { return _StateCode; } set { _StateCode = value; } }
        public string StateName { get { return _StateName; } set { _StateName = value; } }
        public double LowerBound { get { return _LowerBound; } set { _LowerBound = value; } }
        public double UpperBound { get { return _UpperBound; } set { _UpperBound = value; } }
        public double Rate { get { return _Rate; } set { _Rate = value; } }

        //Constructor for the Tax Record object that accepts the csv file line and the current line number of the file.
        public TaxRecord(string FileLine, int lineNumber)
        {
            //take each line from the file and split it into an array of strings
            string[] splitData = FileLine.Split(",");


            //if length is correct
            if (splitData.Length == 5)
            {
                //loop over splitData first checking for empty strings indicating a value is missing
                for (int i = 0; i < splitData.Length; i++)
                {
                    if (splitData[i] == "")
                    {
                        throw new Exception($"Data on line {lineNumber} in column {i} is missing.");
                    }
                }
                //if length is not correct we know that a column is missing. 
            }
            else if (splitData.Length != 5)
            {
                throw new Exception($"Tax Table Error: Internal Data Structure is not the correct length.\nThis is due to an incorrect format on line {lineNumber}. Please adjust file to reflect example format:\n 'State Code,State Name,Bracket Floor,Bracket Cap, Tax Rate'");
            }


            //once correct length is determined or a correction error is thrown we move on to parsing data for input into public class variables. Not sure how to do this programatically, but since we know there should only be 5 pieces of data we can declaratively work through the set.

            //Check to see if the State Code column adheres to 2 character standard. If I was really making sure I would write a longer check to see if the State Code matched a DataBase of acceptable inputs. 
            if (splitData[0].Length != 2)
            {
                throw new Exception($"Tax Table Error: State Code must be exactly 2 characters. Line {lineNumber} : Column 1");
            }
            else
            {
                _StateCode = splitData[0];
            }

            _StateName = splitData[1];

            //Utilizing TryParse to determine if the number columns are actually numbers. This doesn't ensure that numbers are correct, that would require a crosscheck with an official database.
            if (!double.TryParse(splitData[2], out _LowerBound))
            {
                throw new Exception($"Tax Table Error: Data in Column 3 : Line {lineNumber} is not a number.");
            };
            if (!double.TryParse(splitData[3], out _UpperBound))
            {
                throw new Exception($"Tax Table Error: Data in Column 4 : Line {lineNumber} is not a number");
            };
            if (!double.TryParse(splitData[4], out _Rate))
            {
                throw new Exception($"Tax Table Error: Data in Column 5 : Line {lineNumber} is not a number");
            };
        }

        public override string ToString()
        {
            return $" | LowerBound: {LowerBound,-7} | UpperBound: {UpperBound,-12} | Rate: {Rate,-5}";
        }
    }

    //EmployeeRecord class is used to create entries that will ultimately be inserted to the List found within the EmployeeList class.
    public class EmployeeRecord
    {
        //private fields for data backing
        private int _ID;
        private string _Name = "";
        private string _StateCode = "";
        private double _HoursWorked = 0;
        private double _HourlyRate = 0;
        private double _TaxesDue = 0;

        //public getters and setters
        public int ID { get { return _ID; } set { _ID = value; } }

        public string Name { get { return _Name; } set { _Name = value; } }

        public string StateCode { get { return _StateCode; } set { _StateCode = value; } }

        public double HoursWorked { get { return _HoursWorked; } set { _HourlyRate = value; } }

        public double HourlyRate { get { return _HourlyRate; } set { _HourlyRate = value; } }

        public double TaxesDue { get { return _TaxesDue; } set { _TaxesDue = value; } }

        //constructor for parsing and setting incoming data.
        public EmployeeRecord(string fileline, int lineNumber)
        {
            //split the line of data into an array of string values
            string[] splitData = fileline.Split(",");

            //if length is correct
            if (splitData.Length == 5)
            {
                //loop over splitData first checking for empty strings indicating a value is missing
                for (int i = 0; i < splitData.Length; i++)
                {
                    if (splitData[i] == "")
                    {
                        throw new Exception($"Employee List Error: Data on line {lineNumber} in column {i} is missing.");
                    }
                }
                //if length is not correct we know that a column is missing. 
            }
            else if (splitData.Length > 5)
            {
                throw new Exception($"Employee List Error: Line {lineNumber} incorrect format. Too many values provided. Disregarding this entry. Please adjust file to reflect example format: \n'ID,Name,State Code,Hours Worked,Pay Rate'\r\n");
            }
            else if (splitData.Length < 5)
            {
                throw new Exception($"Employee List Error: Line {lineNumber} incorrect format. Not enough values provided. Disregarding this entry. Please adjust file to reflect example format: \n'ID,Name,State Code,Hours Worked,Pay Rate'\r\n");
            }




            //throwing exceptions for the data coming in. Make sure data types can be parsed and are of correct format

            //People can be named whatever they want so there is no check on this field.
            Name = splitData[1];

            //If the ID is not a whole number we throw an exception. 
            if (int.Parse(splitData[0]).GetType() != typeof(int))
            {
                throw new Exception($"Employee List Error: Column 1 on Line {lineNumber}. Value MUST be a whole number. Disregarding this entry. Please Adjust Data accordingly.\r\n");
            }
            else { int.TryParse(splitData[0], out _ID); }

            //state code must be two characters in length
            if (splitData[2].Length != 2)
            {
                throw new Exception($"Employee List Error: Line {lineNumber} Column 3 must be a 2 character state name abbreviation. Disregarding this entry.\r\n");
            }
            else { StateCode = splitData[2]; }

            //Hours Worked
            if (!double.TryParse(splitData[3], out _HoursWorked))
            {
                throw new Exception($"Employee List Error:: Line {lineNumber} Column 4 must be a number. Disregarding this entry.\r\n");
            }

            //Hourly rate
            if (!double.TryParse(splitData[4], out _HourlyRate))
            {
                throw new Exception($"Employee List Error: Line {lineNumber} Column 5 must be a number. Disregarding this entry.\r\n");
            }

            _TaxesDue = TaxCalculator.ComputeTaxForEmployee(splitData[2], _HourlyRate * _HoursWorked);
        }
        public override string ToString()
        {

            return $"ID: {_ID,-8} | Name: {_Name,-12} | Sate Code: {_StateCode,-4} | Hours Worked: {_HoursWorked,-5} | Pay Rate: {_HourlyRate,-8} | Taxes Due: {_TaxesDue, -8}";
        }
    }

        static class EmployeeList
        {
            const string path = @"C:\Users\13475\Desktop\dotNet\Projects\TaxCalculator\employees.csv";
            static public List<EmployeeRecord> Employees = new List<EmployeeRecord>();

            public static void PrintList()
            {
                foreach (var employee in Employees)
                {
                    Console.WriteLine(employee);
                }
            }

            static EmployeeList()
            {


                try
                {
                    StreamReader file = File.OpenText(path);
                    int lineNumber = 1;

                
               
                while (!file.EndOfStream)
                    {
                   
                    try
                        {
                            //pull one line from the file
                            string fileLine = file.ReadLine();

                            //creating new employee object from file line read above. This constructor parses and manipulates the data.
                            EmployeeRecord EmployeeData = new EmployeeRecord(fileLine, lineNumber);

                            //add newly created employee data to the list
                            Employees.Add(EmployeeData);
                        }
                       
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        //increase line number to track progress and assist in error handling
                        lineNumber++;
                    }
                
                file.Close();
                }
                catch (IOException e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
            }


        }
}

