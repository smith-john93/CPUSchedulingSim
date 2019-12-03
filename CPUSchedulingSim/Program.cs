using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace CPUSchedulingSim
{
    public class Program
    {
        private const string COMP_FILE= "ComparisonResults.txt";
        public static void Main()
        {
            
            bool runAgain = true;

            while(runAgain)
            {

                //Get the requested user option
                int option = GetUserOption();

                if(option == (int)ExecutionOption.End)
                {
                    runAgain = false;
                }

                //execut the request
                ExecuteRequest(option);

                //determine if the program should continue executing
                runAgain = GetRunAgain(false);
            }           
            Console.WriteLine("Ending Program Execution");
        }

        /// <summary>
        /// Gets the user input and validates the input
        /// </summary>
        /// <returns></returns>
        private static int GetUserOption()
        {
            while(true)
            {

                Console.WriteLine("Please select one of the following options:");
                Console.WriteLine("1: Single Algorithm Mode");
                Console.WriteLine("2: Algorithm Compare Mode");
                Console.WriteLine("3: Exit");

                //read the input as a string
                string input = Console.ReadLine();

                //Attempt to parse the string
                bool result = int.TryParse(input, out int option);

                //if the string was non-numeric, make the user inut again
                if (!result)
                {
                    Console.WriteLine("Please input a number");
                    continue;
                }

                switch(option)
                {
                    case (int)ExecutionOption.Single:
                        return (int)ExecutionOption.Single;
                    case (int)ExecutionOption.Compare:
                        return (int)ExecutionOption.Compare;
                    case (int)ExecutionOption.End:
                        return (int)ExecutionOption.End;
                    default:
                        Console.WriteLine("Please enter a valid option");
                        break;
                }
            }
        }

        /// <summary>
        /// Handles processing of the user request
        /// </summary>
        /// <param name="SelectedOption"></param>
        private static void ExecuteRequest(int SelectedOption)
        {
            switch (SelectedOption)
            {
                case (int)ExecutionOption.Single:
                    RunSingle();
                    break;
                case (int)ExecutionOption.Compare:
                    Compare();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Generates a single list and runs the list against selected algorithms
        /// </summary>
        private static void RunSingle()
        {
            List<ProcessDTO> processList = GenerateList();

            bool continueRun = true;

            while(continueRun)
            {
                Console.WriteLine("Select an algorithm to run:");
                Console.WriteLine("1: First Come First Serve");
                Console.WriteLine("2: Round Robin");
                string input = Console.ReadLine();

                bool validInput = int.TryParse(input, out int result);

                switch(result)
                {
                    case (int)SchedulingAlgorithms.FCFS:
                        FCFS firstCome = new FCFS(processList);
                        firstCome.Run();
                        firstCome.PrintResults();
                        break;
                    case (int)SchedulingAlgorithms.RR:
                        RoundRobin robin = new RoundRobin(processList);
                        robin.Run();
                        robin.PrintResults();
                        break;
                    default:
                        Console.WriteLine("Please Enter a valid option.");
                        continue;
                }

                continueRun = GetRunAgain(true);

            }

        }

        /// <summary>
        /// Generates a list of processes
        /// </summary>
        /// <returns></returns>
        public static List<ProcessDTO> GenerateList()
        {
            //Create a list to store processes in 
            List<ProcessDTO> processList = new List<ProcessDTO>();

            //seed with total seconds
            Random rand = new Random(DateTime.Now.Millisecond);

            //Set the total number of processes
            int totalProcesses = rand.Next(10, 20);

            //create the processes
            for(int i = 0; i<totalProcesses; i++)
            {
                ProcessDTO process = new ProcessDTO();
                process.ProcessId = i;
                process.Priority = rand.Next(0, 5);
                process.ArrivalTime = 0;
                process.BurstTime = rand.Next(5, 25);
                process.RemainingTime = process.BurstTime;
                processList.Add(process);
            }

            return processList;
        }
        
        /// <summary>
        /// Code to get the user input if program sections should run again
        /// </summary>
        /// <param name="inner"></param>
        /// <returns></returns>
        private static bool GetRunAgain(bool inner)
        {
            while(true)
            {
                if (inner)
                    Console.WriteLine("Select Another Algorithm? [Y|N]");
                else
                    Console.WriteLine("Run again? [Y|N]");
                string input = Console.ReadLine();

                if(input.ToUpper() == "Y" || input.ToUpper() == "N")
                    return input.ToUpper() == "Y" ? true:false;

                Console.WriteLine("Please input a valid option");
            }
        }

        /// <summary>
        /// Runs all algortihms x times and writes results to a file
        /// </summary>
        private static void Compare()
        {
            bool validInput = false;
            int runCount = 0;
            while (!validInput)
            {
                Console.WriteLine("Please enter amount of times to run:");
                string input = Console.ReadLine();
                validInput = int.TryParse(input, out runCount);
            }

            //build a data structure to hold the objects and results
            Dictionary<SchedulingAlgorithms, List<double>> processDict = new Dictionary<SchedulingAlgorithms, List<double>>();
            processDict.Add(SchedulingAlgorithms.FCFS, new List<double>());
            processDict.Add(SchedulingAlgorithms.RR, new List<double>());

            Console.WriteLine("Starting Algorithm Execution");
            for (int i = 0; i < runCount; i++)
            {
                List<ProcessDTO> processList = GenerateList();
                
                foreach (KeyValuePair<SchedulingAlgorithms, List<double>> kvPair in processDict)
                {
                    switch (kvPair.Key)
                    {
                        case SchedulingAlgorithms.FCFS:
                            FCFS firstCome = new FCFS(processList);
                            firstCome.Run();
                            kvPair.Value.Add(firstCome.GetAvgWait());              
                            
                            // this sleep needs to be here to ensure the add completes properly
                            Thread.Sleep(1); 
                            break;

                        case SchedulingAlgorithms.RR:
                            RoundRobin robin = new RoundRobin(processList);
                            robin.Run();
                            kvPair.Value.Add(robin.GetAvgWait());
                            
                            // this sleep needs to be here to ensure the add completes properly
                            Thread.Sleep(1); 
                            break;

                        default:
                            break;
                    }
                }
                processList.Clear();
            }
            Console.WriteLine("Completed Running Algorithms");

            if (File.Exists(COMP_FILE))
                File.Delete(COMP_FILE);

            using (StreamWriter stream = new StreamWriter(COMP_FILE))
            {

                stream.WriteLine("Results of Scheduling Algorithm Comparisons");
                stream.WriteLine($"Total interations: {runCount}\n");

                Console.WriteLine("Results of Scheduling Algorithm Comparisons");
                Console.WriteLine($"Total interations: {runCount}\n");

                foreach (KeyValuePair<SchedulingAlgorithms, List<double>> kvPair in processDict)
                {
                    switch (kvPair.Key)
                    {
                        case SchedulingAlgorithms.FCFS:
                            stream.WriteLine("Results for First Come First Serve");
                            stream.WriteLine($"Lowest Average Wait: {kvPair.Value.Min()}");
                            stream.WriteLine($"Total Average Wait: {(kvPair.Value.Sum() / kvPair.Value.Count)}");
                            stream.WriteLine($"Highest Average Wait: {kvPair.Value.Max()}");
                            stream.WriteLine();
                            
                            Console.WriteLine("Results for First Come First Serve");
                            Console.WriteLine($"Lowest Average Wait: {kvPair.Value.Min()}");
                            Console.WriteLine($"Total Average Wait: {(kvPair.Value.Sum() / kvPair.Value.Count)}");
                            Console.WriteLine($"Highest Average Wait: {kvPair.Value.Max()}");
                            Console.WriteLine();
                            break;
                        case SchedulingAlgorithms.RR:
                            stream.WriteLine("Results for Round Robin");
                            stream.WriteLine($"Lowest Average Wait: {kvPair.Value.Min()}");
                            stream.WriteLine($"Total Average Wait: {(kvPair.Value.Sum() / kvPair.Value.Count)}");
                            stream.WriteLine($"Highest Average Wait: {kvPair.Value.Max()}");
                            stream.WriteLine();

                            Console.WriteLine("Results for Round Robin");
                            Console.WriteLine($"Lowest Average Wait: {kvPair.Value.Min()}");
                            Console.WriteLine($"Total Average Wait: {(kvPair.Value.Sum() / kvPair.Value.Count)}");
                            Console.WriteLine($"Highest Average Wait: {kvPair.Value.Max()}");
                            Console.WriteLine();
                            break;
                    }
                }
            }
            Console.WriteLine("Completed Writing to file");
        }
    }

    /// <summary>
    /// Enumeration for the execution options 
    /// </summary>
    public enum ExecutionOption
    {
        Single = 1,
        Compare = 2,
        End = 3,
    }

    /// <summary>
    /// Enumeration for the designed scheduling algorithms
    /// </summary>
    public enum SchedulingAlgorithms
    {
        FCFS = 1,
        RR = 2
    }
}
