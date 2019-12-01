using System;
using System.Collections.Generic;
using System.Text;

namespace CPUSchedulingSim
{
    public class FCFS
    {
        Queue<ProcessDTO> ProcessQueue;

        public FCFS(List<ProcessDTO> ProcessList)
        {
            //Initialize the Queue for First Come First Serve
            ProcessQueue = new Queue<ProcessDTO>();

            //Populate the queue out of the List passed to the class
            foreach(ProcessDTO process in ProcessList)
            {
                ProcessQueue.Enqueue(process);
            }
        }



    }
}
