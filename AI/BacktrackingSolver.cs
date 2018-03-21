using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    internal class BacktrackingSolver : CSPSolver
    {
        public BacktrackingSolver(SudokuGrid initialGrid)
            : base(initialGrid)
        {
            // Use default constructor
        }

        public override CSPSolution Solve()
        {
            Console.WriteLine("Beginning backtracking search");
            _solverStopwatch = Stopwatch.StartNew();

            BacktrackingSearch();

            _solverStopwatch.Stop();
            Console.WriteLine("Backtracking search complete");

            return CompleteSolve("Backtracking Search");
        }

        private bool BacktrackingSearch()
        {
            // Done if all squares contain a nonzero value
            if (_grid.Grid.All(node => node.Value != 0))
                return true;

            // Get the next zero-value node to process
            Node nextNode = GetNextEmptyNode();
            foreach (int value in GetValidValuesForNode(nextNode))
            {
                nextNode.Value = value; // put the value in the next node

                if (BacktrackingSearch()) // try to continue
                    return true;

                nextNode.Value = 0; // backtracking search failed, so remove the value we added
            }

            return false;
        }

        
    }
}
