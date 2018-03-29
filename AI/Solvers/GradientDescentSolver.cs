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
        private SudokuGrid _solvedGrid;

        public GradientDescentSolver(SudokuGrid initialGrid)
            : base(initialGrid)
        {
            _solvedGrid = initialGrid; // start with the initial grid
            _solvedGrid.Randomize();
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
            foreach (Node node in _solvedGrid.EditableGrid)
            {
                DoDescent(node);
            }

            if (PerformEvaluationFunction() > 0)
                GradientDescent();
        }

        private void DoDescent(Node currentNode)
        {
            int originalEvaluation = PerformEvaluationFunction();

            // If we still have constraint violations...
            if (originalEvaluation > 0)
            {
                int bestEvaluation = 0;
                int bestValue = 0;

                // Try every value in the current node's domain
                foreach (int value in currentNode.Domain)
                {
                    int currentEvaluation = PerformEvaluationFunction();
                    
                    // If we are violating fewer constraints with the new guess, keep track
                    if (currentEvaluation < originalEvaluation)
                    {
                        bestEvaluation = currentEvaluation;
                        bestValue = value;
                        _solvedGrid.SetNodeValue(currentNode.Row, currentNode.Column, bestValue);
                        _procesesed++;
                    }
                }
            }
        }

        // How many constraints are being violated by the current guess?
        private int PerformEvaluationFunction()
        {
            int violations = 0;

            foreach (Node node in _solvedGrid.Grid)
            {
                // The valid values for this node
                List<int> validValues = SudokuGrid.GetNodeDomain(node);

                // If the current value is not one of the valid ones, we have a violation
                if (!validValues.Contains(node.Value))
                {
                    violations++;
                }
            }

            return violations;
        }
    }
}
