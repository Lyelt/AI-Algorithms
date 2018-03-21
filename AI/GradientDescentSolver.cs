using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    internal class GradientDescentSolver : CSPSolver
    {
        public GradientDescentSolver(SudokuGrid initialGrid)
            : base(initialGrid)
        {
            // Use default constructor
        }

        public override CSPSolution Solve()
        {
            Console.WriteLine("Solving using gradient descent");
            _solverStopwatch = Stopwatch.StartNew();

            GradientDescent();
            
            _solverStopwatch.Stop();
            Console.WriteLine("Gradient descent complete");

            return CompleteSolve("Gradient Descent");
        }

        private void GradientDescent()
        {

        }
    }
}
