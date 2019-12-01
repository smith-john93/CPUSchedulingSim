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
        public RoundRobin(List<ProcessDTO> list)
        {
            processList = new List<ProcessDTO>();

            foreach (ProcessDTO process in list)
                processList.Add(process);
        }

        public void Run()
        {
            tQuantum = 0;
            int index = 0;
            int waitTime = 0;
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
                    //subtract the remainder and set the remaining quantu to 0
                    if(processList[index].RemainingTime >= remainingQuantum)
                    {
                        CalculateWait(processList[index], remainingQuantum, waits);

                        processList[index].RemainingTime -= remainingQuantum;
                        remainingQuantum = 0;
                        processList[index].lastProcess = waits;

                        IncrimentIndex(index);
                    }
                    else
                    {
                        CalculateWait(processList[index], remainingQuantum, waits);

                        int rem = processList[index].RemainingTime;
                        processList[index].RemainingTime = 0;
                        remainingQuantum -= rem;
                        IncrimentIndex(index);
                        contextSwitches++;
                    }
                }
                waits++;
                tQuantum++;
            }
        }

        public void PrintResults()
        {
            Console.WriteLine("Id\tPriority\tArrivalTime\tBurstTime\tWaitTime");
            for (int i = 0; i < 64; i++)
                Console.Write('-');
            Console.WriteLine();

            foreach (ProcessDTO process in processList)
            {
                Console.WriteLine($"{process.ProcessId}\t{process.Priority}\t\t{process.ArrivalTime}\t\t{process.BurstTime}\t\t{process.WaitTime}");
            }

            Console.WriteLine($"Total Quantums: {tQuantum}");
            
            //Print the maximum wait time
            Console.WriteLine($"Maximum Initial Wait Time {(processList.Count-1) * QUANTUM}");

            //Print the total context switches
            Console.WriteLine($"Total Context Switches: {contextSwitches}");

            ////Print the total wait time
            ////Use the total execution time, subtract the burst time of the last process
            //Console.WriteLine($"Total Wait Time {TotalExec - Results[Results.Count - 1].BurstTime}");

            ////print the total execution time
            //Console.WriteLine($"Total Execution Time: {TotalExec }");
        }

        private int IncrimentIndex(int index)
        {
            if (index == processList.Count - 1)
                return 0;
            else
                return index + 1;
        }

        private void CalculateWait(ProcessDTO process, int remainingTime, int totalWaits)
        {
            if (totalWaits == 0 && remainingTime == 10)
            {
                process.WaitTime = 0;
                return;
            }

            int waitDiff = totalWaits - process.lastProcess;

            process.WaitTime += (waitDiff * QUANTUM) + (QUANTUM - remainingTime);
        }
    }
}
