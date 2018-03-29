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

            try
            {
                DoArcConsistency(arcs);
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.ToString());
            }


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

            // Don't bother trying to reduce the domain of something we were already given
            if (!node1.Editable)
                return revised;

            // Loop through the domain of the first node
            // Note that we explicity ToList() the domain so that we can actually modify the original domain as we loop through it
            foreach (int x1 in node1.Domain.ToList())  
            {
                // If this value results in any inconsistencies with node2, remove it from node1's domain
                if (node2.Value == x1)
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
                    if (node1 != node2 // it's the same node
                        && SudokuGrid.NodesAreRelated(node1, node2) // nodes have to be related to be considered an arc
                        && (node1.Editable || node2.Editable))      // at least one of them should be editable, otherwise why bother?
                    {
                        Arc arc = new Arc()
                        {
                            Node1 = node1,
                            Node2 = node2
                        };
                        
                        if (!arcs.Contains(arc) )
                        {
                            arcs.Enqueue(arc);
                        }
                    }
                }
            }
            return arcs;
        }
    }
}
