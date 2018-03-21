using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    class Program
    {
        static void Main(string[] args)
        {
            SudokuGrid initialProblem = LoadGridFromFile(args[0]);

            List<CSPSolver> problemSolvers = new List<CSPSolver>()
            {
                new ArcConsistencySolver(initialProblem),
                new BacktrackingSolver(initialProblem),
                new GradientDescentSolver(initialProblem)
            };
            
            foreach (CSPSolver solver in problemSolvers)
            {
                CSPSolution solution = solver.Solve();
                PrintSolution(solution);
            }
        }

        private static SudokuGrid LoadGridFromFile(string filePath)
        {
            SudokuGrid grid = new SudokuGrid(6, 6, File.ReadAllText(filePath));
            return grid;
        }

        private static void PrintSolution(CSPSolution solution)
        {

        }
    }
}
