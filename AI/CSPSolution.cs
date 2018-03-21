using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    internal class CSPSolution
    {
        public string AlgorithmName { get; set; }

        public TimeSpan TimeElapsed { get; set; }
        
        public int Processed { get; set; }

        public SudokuGrid SolutionGrid { get; set; }
    }
}
