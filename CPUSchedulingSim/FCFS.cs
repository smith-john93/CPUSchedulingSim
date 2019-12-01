using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CPUSchedulingSim
{
    public class FCFS
    {
        IList<ProcessDTO> Results;
        private double AvgWait = 0;
        private int TotalExec = 0;

        public FCFS(List<ProcessDTO> ProcessList)
        {
            //Initialize the list of processes
            Results = new List<ProcessDTO>();

            //Populate the queue out of the List passed to the class
            foreach(ProcessDTO process in ProcessList)
            {
                Results.Add(process);
            }

        }

        public void Run()
        {
            TotalExec = 0;
            foreach(ProcessDTO process in Results)
            {
                //set the wait time of the process before being executed
                process.WaitTime = TotalExec;

                //Add to the total execution time if this is not the last process
                TotalExec += process.BurstTime;
            }

            //calculate the averate wait time
            foreach (ProcessDTO process in Results)
                AvgWait += process.WaitTime;
            AvgWait = AvgWait / Results.Count;
        }

        public void PrintResults()
        {
            Console.WriteLine("Id\tPriority\tArrivalTime\tBurstTime\tWaitTime");
            for (int i = 0; i < 64; i++)
                Console.Write('-');
            Console.WriteLine();

            foreach(ProcessDTO process in Results)
            {
                Console.WriteLine($"{process.ProcessId}\t{process.Priority}\t\t{process.ArrivalTime}\t\t{process.BurstTime}\t\t{process.WaitTime}");
            }

            //Print the average wait time
            Console.WriteLine($"Average Wait Time {AvgWait}");

            //Print the total wait time
            //Use the total execution time, subtract the burst time of the last process
            Console.WriteLine($"Total Wait Time {TotalExec - Results[Results.Count - 1].BurstTime}");

            //print the total execution time
            Console.WriteLine($"Total Execution Time: {TotalExec }");
        }

        public double GetAvgWait()
        {
            return AvgWait;
        }
    }
}
