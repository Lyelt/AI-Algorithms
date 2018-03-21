using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    internal class ArcConsistencySolver : CSPSolver
    {
        public ArcConsistencySolver(SudokuGrid initialGrid) : base (initialGrid)
        {
            // Use default constructor
        }

        public override CSPSolution Solve()
        {
            Console.WriteLine("Beginning arc consistency check");
            _solverStopwatch = Stopwatch.StartNew();

            // Do arc consistency checking to narrow the domains
            Queue<Arc> arcs = GetAllArcs();
            DoArcConsistency(arcs);

            // Complete the solution with backtracking search
            BacktrackingSolver backtracker = new BacktrackingSolver(_grid);
            CSPSolution solution2 = backtracker.Solve();

            _solverStopwatch.Stop();

            return CompleteSolve("Arc Consistency with Backtracking Search", solution2.SolutionGrid, _solverStopwatch.Elapsed, _procesesed);
        }

        private void DoArcConsistency(Queue<Arc> arcs)
        {
            while (arcs.Count > 0)
            {
                Arc currentArc = arcs.Dequeue();
                Node node1 = currentArc.Node1;
                Node node2 = currentArc.Node2;

                // Reduce the domain of node 1 based on the violations from node 2
                if (ArcReduce(node1, node2))
                {
                    // If the domain was reduced to zero, the problem is not arc consistent
                    if (node1.Domain.Count == 0)
                        throw new InvalidOperationException("The initial problem is not arc consistent");

                    // Add all the arcs of node1's neighbors
                    foreach (Node neighbor in node1.Neighbors.Where(n => n != node2))
                    {
                        arcs.Enqueue(new Arc() { Node1 = neighbor, Node2 = node1 });
                    }
                }
            }
        }

        private bool ArcReduce(Node node1, Node node2)
        {
            bool revised = false;

            // Loop through the domain of the first node
            foreach (int x1 in node1.Domain)
            {
                // If there are any violations of our constraint (i.e. they are equal)
                // Remove the value from node1's domain - it's invalid
                if (node2.Domain.Any(x2 => x2 == x1))
                {
                    node1.Domain.Remove(x1);
                    revised = true;
                    _procesesed++;
                }
            }

            return revised;
        }

        private Queue<Arc> GetAllArcs()
        {
            Queue<Arc> arcs = new Queue<Arc>();
            foreach (Node node1 in _grid.Grid)
            {
                foreach (Node node2 in _grid.Grid)
                {
                    // Gives us a pair of nodes that are related
                    // Should we check if the pair already exists backward?
                    if (node1 != node2 && SudokuGrid.NodesAreRelated(node1, node2))
                    {
                        arcs.Enqueue(new Arc()
                        {
                            Node1 = node1,
                            Node2 = node2
                        });
                    }
                }
            }
            return arcs;
        }
    }
}
