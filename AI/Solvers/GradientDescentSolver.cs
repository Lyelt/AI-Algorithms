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

            try
            {
                GradientDescent();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Here is the best solution that was found: ");
                return CompleteSolve("Gradient Descent - Aborted");
            }

            _solverStopwatch.Stop();
            Console.WriteLine("Gradient descent complete");

            return CompleteSolve("Gradient Descent");
        }

        private void GradientDescent()
        {
            if (_solverStopwatch.Elapsed.TotalSeconds > 60)
                throw new InvalidOperationException("Could not solve in 60 seconds");

            foreach (Node node in _solvedGrid.EditableGrid)
            {
                DoDescent(node);
            }

            // Didn't solve, try again
            if (PerformEvaluationFunction() > 0)
            {
                _iterations++;
                _solvedGrid.Randomize();
                GradientDescent();
            }
        }

        private void DoDescent(Node currentNode)
        {
            int bestEvaluation = PerformEvaluationFunction();
            // If we still have constraint violations...
            if (bestEvaluation > 0)
            {
                int bestValue = 0;

                // Try every value in the current node's domain
                foreach (int value in currentNode.Domain)
                {
                    // Try to set the node to a different value
                    _solvedGrid.SetNodeValue(currentNode.Row, currentNode.Column, value);
                    
                    // See how that affects the evaluation function
                    int currentEvaluation = PerformEvaluationFunction();
                    
                    // If we are violating fewer constraints with the new guess, keep track
                    if (currentEvaluation <= bestEvaluation)
                    {
                        bestEvaluation = currentEvaluation;
                        bestValue = value;
                    }

                    _procesesed++;
                }
                
                if (bestValue != 0)
                    _solvedGrid.SetNodeValue(currentNode.Row, currentNode.Column, bestValue);
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
