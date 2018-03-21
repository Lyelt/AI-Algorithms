using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    internal abstract class CSPSolver
    {
        protected SudokuGrid _grid;
        protected Stopwatch _solverStopwatch;
        protected int _procesesed = 0;

        protected CSPSolver(SudokuGrid initialGrid)
        {
            _grid = initialGrid;
        }

        public abstract CSPSolution Solve();

        protected List<int> GetValidValuesForNode(Node node)
        {
            List<int> validValues = SudokuGrid.DEFAULT_DOMAIN;

            foreach(int value in validValues)
            {
                if (node.Neighbors.Any(n => n.Value == value))
                    validValues.Remove(value);
            }

            return validValues;
        }

        protected Node GetNextEmptyNode()
        {
            // Get the square with the smallest domain (fewest possible values currently)
            int smallestDomain = _grid.Grid.Where(node => node.Value == 0).Min(node => node.Domain.Count);
            // Return the first one
            return _grid.Grid.First(node => node.Domain.Count == smallestDomain);
        }

        protected CSPSolution CompleteSolve(string name)
        {
            return CompleteSolve(name, _grid, _solverStopwatch.Elapsed, _procesesed);
        }

        protected CSPSolution CompleteSolve(string name, SudokuGrid grid, TimeSpan elapsed, int processed)
        {
            CSPSolution solution = new CSPSolution();

            solution.AlgorithmName = name;
            solution.SolutionGrid = grid;
            solution.TimeElapsed = elapsed;
            solution.Processed = processed;

            return solution;
        }
    }
}
