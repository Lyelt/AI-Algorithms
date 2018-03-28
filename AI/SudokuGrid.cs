using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    internal class SudokuGrid
    {
        public static List<int> DefaultDomain = new List<int>() { 1, 2, 3, 4, 5, 6 };
        public static Random Randomizer = new Random();

        public List<Node> Grid { get; private set; }

        public List<Node> EditableGrid { get; private set; }
       
        public int Rows { get; private set; }

        public int Columns { get; private set; }

        public SudokuGrid(int rows, int cols, string initialValueString)
        {
            Rows = rows;
            Columns = cols;
            Grid = new List<Node>();
            EditableGrid = new List<Node>();
            char[] initialGridValues = initialValueString.ToCharArray();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // Initial grid is represented by an array of characters
                    // 0-9, where 0 is a value that has not been filled in
                    int currentIndex = (i * rows) + j;
                    char currentChar = initialGridValues[currentIndex];
                    int value = (int)char.GetNumericValue(currentChar);

                    SetGridValue(i, j, value);
                }
            }

            SetAllNodeNeighbors();
        }

        public void Randomize()
        {
            foreach (Node node in Grid)
            {
                if (node.Value == 0)
                {
                    node.Value = Randomizer.Next(1, DefaultDomain.Count + 1);
                }
            }
        }

        public void SetGridValue(int rowNumber, int colNumber, int value)
        {
            Node node = new Node()
            {
                Row = rowNumber,
                Column = colNumber,
                Value = value
            };
            
            node.Domain = DefaultDomain;
            node.Neighbors = new List<Node>();
            Grid.Add(node);

            if (value == 0)
                EditableGrid.Add(node);
        }

        private void SetAllNodeNeighbors()
        {
            foreach (Node node1 in Grid)
            {
                foreach (Node node2 in Grid)
                {
                    // Gives us a pair of nodes that are related
                    // Should we check if the pair already exists backward?
                    if (node1 != node2 && NodesAreRelated(node1, node2))
                    {
                        node1.Neighbors.Add(node2);
                    }
                }
            }
        }

        public static bool NodesAreRelated(Node node1, Node node2)
        {
            // If nodes are in the same row, column, or 3x3 square they are related
            return (node1.Row == node2.Row || node1.Column == node2.Column)
                    || ((GetBoxNumber(node1) == GetBoxNumber(node2)));
        }

        private static int GetBoxNumber(Node node)
        {
            int boxNum = 0;

            // Top-left 2x3 box
            if (node.Row <= 1 && node.Column <= 2)
                boxNum = 1;
            // Top-right
            else if (node.Row <= 2 && node.Column > 2)
                boxNum = 2;
            // Middle-left
            else if (node.Row > 1 && node.Row < 3 && node.Column <= 2)
                boxNum = 3;
            // Middle-right
            else if (node.Row > 1 && node.Row < 3 && node.Column > 2)
                boxNum = 4;
            // Bottom-left
            else if (node.Row > 2 && node.Column <= 2)
                boxNum = 5;
            // Bottom-right
            else if (node.Row > 2 && node.Column > 2)
                boxNum = 6;

            return boxNum;
        }
    }
}
