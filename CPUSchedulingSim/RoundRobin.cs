using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPUSchedulingSim
{
    public class RoundRobin
    {
        private List<ProcessDTO> processList;
        private const int QUANTUM = 10;
        private int tQuantum;
        private int contextSwitches;
        private double AvgWait;
        public RoundRobin(List<ProcessDTO> list)
        {
            processList = new List<ProcessDTO>();

            foreach (ProcessDTO process in list)
                processList.Add(process);
        }

        /// <summary>
        /// Runs the scheduling algorithm
        /// </summary>
        public void Run()
        {
            //Reset the variables needed for this execution
            foreach(ProcessDTO process in processList)
            {
                process.WaitTime = 0;
                process.RemainingTime = process.BurstTime;
                process.lastProcess = 0;

            }

            //set the local variables needed for execution
            tQuantum = 0;
            int index = 0;
            int waits = 0;
            contextSwitches = 0;                        

            //we will run through the list until all processes have no remaining time
            while(processList.Any(a =>a.RemainingTime > 0))
            {
                //process this instance until there is no quantum left
                int remainingQuantum = QUANTUM;
                
                while(remainingQuantum > 0 && processList.Any(a => a.RemainingTime > 0))
                {
                    //if there is no remaining time in this process, go to the next one
                    if (processList[index].RemainingTime == 0)
                    {
                        index = IncrimentIndex(index);
                        continue;
                    }

                    //if the process has more remaining time than the remaining quantum
                    //subtract the remainder and set the remaining quantum to 0
                    if(processList[index].RemainingTime >= remainingQuantum)
                    {
                        CalculateWait(processList[index], remainingQuantum, waits);
                        processList[index].RemainingTime -= remainingQuantum;
                        remainingQuantum = 0;
                        processList[index].lastProcess = waits;

                        index = IncrimentIndex(index);
                    }
                    //The process has less remaning time than remaining quantum
                    //set the remaining time to 0 and subtract the process time from the quantum
                    else
                    {
                        CalculateWait(processList[index], remainingQuantum, waits);

                        int rem = processList[index].RemainingTime;
                        processList[index].RemainingTime = 0;
                        remainingQuantum -= rem;
                        index = IncrimentIndex(index);
                        contextSwitches++;
                    }
                }
                waits++;
                tQuantum++;
            }

            int waitSum = processList.Sum(a => a.WaitTime);
            AvgWait = waitSum / processList.Count;
        }

        /// <summary>
        /// Prints the informaiton of each process and stats
        /// </summary>
        public void PrintResults()
        {
            Console.WriteLine("Round Robin Results");
            Console.WriteLine("Id\tPriority\tArrivalTime\tBurstTime\tWaitTime");
            for (int i = 0; i < 64; i++)
                Console.Write('-');
            Console.WriteLine();

            foreach (ProcessDTO process in processList)
            {
                Console.WriteLine($"{process.ProcessId}\t{process.Priority}\t\t{process.ArrivalTime}\t\t{process.BurstTime}\t\t{process.WaitTime}");
            }

            Console.WriteLine($"Total Quantums: {tQuantum}");
            
            //Print the total context switches
            Console.WriteLine($"Total Context Switches: {contextSwitches}");

            //Print the maximum and average wait time
            Console.WriteLine($"Maximum Initial Wait Time {(processList.Count-1) * QUANTUM}");

            Console.WriteLine($"Average Wait Time {AvgWait}");
        }

        /// <summary>
        /// Incriments the index for the algorithm loop
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int IncrimentIndex(int index)
        {
            //if the index is at the max, set back to 0
            if (index == processList.Count - 1)
                return 0;
            //set the index to the next number
            else
                return index + 1;
        }

        /// <summary>
        /// Calculates the aditional wait to add to the process
        /// </summary>
        /// <param name="process"></param>
        /// <param name="remainingTime"></param>
        /// <param name="totalWaits"></param>
        private void CalculateWait(ProcessDTO process, int remainingTime, int totalWaits)
        {
            //if this is the first execution, there is no wait to calculate
            if (totalWaits == 0 && remainingTime == 10)
            {
                process.WaitTime = 0;
                return;
            }

            //calculate the number of full cycles before this process has been reached
            int waitDiff = totalWaits - process.lastProcess;

            //calculate the exact time since the process has been touched
            process.WaitTime += (waitDiff * QUANTUM) + (QUANTUM - remainingTime);
        }

        /// <summary>
        /// calculate the average wait time for the process
        /// </summary>
        /// <returns></returns>
        public double GetAvgWait()
        {
            return AvgWait;
        }
    }
}
