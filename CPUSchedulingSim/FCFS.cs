using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CPUSchedulingSim
{
    public class FCFS
    {
        List<ProcessDTO> processList;
        private double AvgWait = 0;
        private int TotalExec = 0;

        public FCFS(List<ProcessDTO> list)
        {
            //Initialize the list of processes
            processList = new List<ProcessDTO>();

            //Populate the queue out of the List passed to the class
            foreach(ProcessDTO process in list)
            {
                processList.Add(process);
            }

        }

        /// <summary>
        /// Runs the scheduling algorithm
        /// </summary>
        public void Run()
        {
            foreach (ProcessDTO process in processList)
                process.WaitTime = 0;

            TotalExec = 0;
            foreach(ProcessDTO process in processList)
            {
                //set the wait time of the process before being executed
                process.WaitTime = (TotalExec - process.ArrivalTime);

                //Add to the total execution time if this is not the last process
                TotalExec += process.BurstTime;
            }

            //calculate the averate wait time
            foreach (ProcessDTO process in processList)
                AvgWait += process.WaitTime;
            AvgWait /= processList.Count;
        }

        /// <summary>
        /// Prints the informaiton of each process and stats
        /// </summary>
        public void PrintResults()
        {
            Console.WriteLine("First Come First Serve Results");
            Console.WriteLine("Id\tPriority\tArrivalTime\tBurstTime\tWaitTime");
            for (int i = 0; i < 64; i++)
                Console.Write('-');
            Console.WriteLine();

            foreach(ProcessDTO process in processList)
            {
                Console.WriteLine($"{process.ProcessId}\t{process.Priority}\t\t{process.ArrivalTime}\t\t{process.BurstTime}\t\t{process.WaitTime}");
            }

            //Print the average wait time
            Console.WriteLine($"Average Wait Time {AvgWait}");

            //Print the total wait time
            //Use the total execution time, subtract the burst time of the last process
            Console.WriteLine($"Total Wait Time {TotalExec - processList[processList.Count - 1].BurstTime}");

            //print the total execution time
            Console.WriteLine($"Total Execution Time: {TotalExec }");
        }

        public double GetAvgWait()
        {
            return AvgWait;
        }
    }
}
