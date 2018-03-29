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
            Console.WriteLine($"{args[0]} loaded");

            bool quit = false;

            while (!quit)
            {
                SudokuGrid initialProblem = LoadGridFromFile(args[0]);

                Console.WriteLine();
                Console.WriteLine("Choose an algorithm: ArcConsistency (ac), BacktrackingSearch (bts), or GradientDescent (gd)");
                Console.WriteLine("Press Q to quit, or P to print the initial grid");
                string choice = Console.ReadLine();

                List<CSPSolver> problemSolvers = new List<CSPSolver>();

                switch (choice.ToLower().Trim())
                {
                    case "print":
                    case "p":
                        PrintGrid(initialProblem);
                        break;
                    case "q":
                    case "quit":
                        quit = true;
                        break;
                    case "ac":
                    case "arcconsistency":
                    case "arc":
                        problemSolvers.Add(new ArcConsistencySolver(initialProblem));
                        break;
                    case "backtracking":
                    case "bts":
                    case "bt":
                    case "backtrackingsearch":
                        problemSolvers.Add(new BacktrackingSolver(initialProblem));
                        break;
                    case "gradientdescent":
                    case "gd":
                    case "gradient":
                        problemSolvers.Add(new GradientDescentSolver(initialProblem));
                        break;
                    default:    // just do the solution for all of them
                        problemSolvers.Add(new ArcConsistencySolver(initialProblem));
                        problemSolvers.Add(new BacktrackingSolver(initialProblem));
                        problemSolvers.Add(new GradientDescentSolver(initialProblem));
                        break;
                }


                foreach (CSPSolver solver in problemSolvers)
                {
                    CSPSolution solution = solver.Solve();
                    PrintSolution(solution);
                }
            }
        }

        private static SudokuGrid LoadGridFromFile(string filePath)
        {
            SudokuGrid grid = new SudokuGrid(6, 6, File.ReadAllText(filePath));
            return grid;
        }

        private static void PrintSolution(CSPSolution solution)
        {
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine($"Solution for {solution.AlgorithmName}");
            Console.WriteLine($"Solved in {solution.TimeElapsed.TotalMilliseconds} ms");
            Console.WriteLine($"Processed {solution.Processed} nodes");
            Console.WriteLine();

            PrintGrid(solution.SolutionGrid);
            
            Console.WriteLine("-------------------------------------------------------");
        }

        private static void PrintGrid(SudokuGrid grid)
        {
            for (int i = 0; i < grid.Rows; i++)
            {
                for (int j = 0; j < grid.Columns; j++)
                {
                    Node thisNode = grid.GetNode(i, j);
                    if (j == 5) // last column, go onto a newline
                    {
                        Console.WriteLine(thisNode.Value);

                        if (i == 1 || i == 3) // 2nd or 4th row, write an extra newline so we can visualize the 2x3 boxes
                            Console.WriteLine();
                    }
                    else
                        Console.Write(thisNode.Value);

                    if (j == 2) // space after 3rd column to help us visualize the 2x3 boxes
                        Console.Write(" ");
                }
            }
        }
    }
}
