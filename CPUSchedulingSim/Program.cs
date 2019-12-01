using System;
using System.Collections.Generic;

namespace CPUSchedulingSim
{
    public class Program
    {
        public static void Main()
        {
            bool runAgain = true;

            while(runAgain)
            {
                //Get the requested user option
                int selected = GetUserOption();

                //execut the request
                ExecuteRequest(selected);

                //determine if the program should continue executing
                runAgain = GetRunAgain();
            }
            
        }

        /// <summary>
        /// Gets the user input and validates the input
        /// </summary>
        /// <returns></returns>
        private static int GetUserOption()
        {           
            while(true)
            {

                //write to the console and list the options
                Console.WriteLine("Please select one of the following options:");
                Console.WriteLine("1: First Come First Serve");
                Console.WriteLine("2: Round Robin");
                Console.WriteLine("3: Algorithm Compare");

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

                //The input was numeric, make sure it us a valid selection
                switch(option)
                {
                    //The user selected First Come First Serve
                    case (int)SchedulingAlgorithms.FCFS:
                        return 1;

                    case (int)SchedulingAlgorithms.RR:
                        return 2;
                    //The user selected Compare the algorithms
                    case (int)SchedulingAlgorithms.Compare:
                        return 3;

                    //The user did not select a valid option
                    default:
                        Console.WriteLine("Please enter a valid option");
                        break;
                }
            }
        }

        private static void ExecuteRequest(int SelectedOption)
        {
            List<ProcessDTO> processList = GenerateList();

            switch (SelectedOption)
            {
                case (int)SchedulingAlgorithms.FCFS:
                    FCFS FirstCome = new FCFS(processList);
                    FirstCome.Run();
                    FirstCome.PrintResults();
                    break;
                case (int)SchedulingAlgorithms.RR:
                    RoundRobin robin = new RoundRobin(processList);
                    robin.Run();
                    robin.PrintResults();
                    break;
                case (int)SchedulingAlgorithms.Compare:
                    Console.WriteLine("Compare Algorithms Here");
                    break;
                default:
                    break;
            }
        }


        public static List<ProcessDTO> GenerateList()
        {
            //Create a list to store processes in 
            List<ProcessDTO> processList = new List<ProcessDTO>();

            //seed with total seconds
            Random rand = new Random(DateTime.Now.Second);

            //Set the total number of processes
            int totalProcesses = rand.Next(10, 20);

            //create the processes
            for(int i = 0; i<totalProcesses; i++)
            {
                ProcessDTO process = new ProcessDTO();
                process.ProcessId = i;
                process.Priority = rand.Next(0, 5);
                process.ArrivalTime = 0;
                process.BurstTime = rand.Next(5, 50);
                process.RemainingTime = process.BurstTime;
                processList.Add(process);
            }

            return processList;
        }
        private static bool GetRunAgain()
        {
            while(true)
            {
                Console.WriteLine("Run program again? [Y|N]");

                string input = Console.ReadLine();

                if(input.ToUpper() == "Y" || input.ToUpper() == "N")
                    return input.ToUpper() == "Y" ? true:false;

                Console.WriteLine("Please input a valid option");
            }
        }
    }

    public enum SchedulingAlgorithms
    {
        FCFS = 1,
        RR = 2,
        Compare = 3
    }
}
