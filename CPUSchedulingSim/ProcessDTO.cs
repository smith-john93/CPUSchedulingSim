using System;
using System.Collections.Generic;
using System.Text;

namespace CPUSchedulingSim
{
    public class ProcessDTO
    {
        public ProcessDTO()
        {

        }
        
        public int ProcessId { get; set; }

        public int Priority { get; set; }

        public int ArrivalTime { get; set;}

        public int BurstTime { get; set; }

        public int WaitTime { get; set; }

        public int RemainingTime { get; set; }

        public int lastProcess { get; set; }
    }
}
