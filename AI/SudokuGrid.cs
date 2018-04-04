using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    internal class SudokuGrid
    {
        public static readonly IReadOnlyList<int> DefaultDomain = new List<int>() { 1, 2, 3, 4, 5, 6 };
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
                    
                    SetInitialNodeValue(i, j, value);
                }
            }

            SetAllNodeNeighbors();
            SetAllNodeDomains();
        }

        public void Randomize()
        {
            foreach (Node node in EditableGrid)
            {
                SetNodeValue(node.Row, node.Column, Randomizer.Next(1, DefaultDomain.Count + 1));
            }
        }

        // The first time we create a node
        private void SetInitialNodeValue(int rowNumber, int colNumber, int value)
        {
            Node node = Node.Create(rowNumber, colNumber, value);
            
            node.Domain = DefaultDomain.ToList();
            node.Neighbors = new List<Node>();
            Grid.Add(node);

            if (value == 0)
            {
                node.Editable = true;
                EditableGrid.Add(node);
            }           
        }

        // Set the value of an existing node, and make all the proper adjustments, such as domain
        public void SetNodeValue(int rowNumber, int colNumber, int value)
        {
            Node existingNode = Grid.Find(n => n.Row == rowNumber && n.Column == colNumber);
            SetNodeValue(existingNode, value);
        }

        public void SetNodeValue(Node node, int value)
        {
            // Override value
            node.Value = value;
            // Fix up the domain
            node.Domain = GetNodeDomain(node);

            SetAllNodeDomains();
        }

        // After the grid is constructed we can properly evaluated the domains of each node
        private void SetAllNodeDomains()
        {
            foreach (Node node in Grid)
            {
                node.Domain = GetNodeDomain(node);
            }
        }

        public static List<int> GetNodeDomain(Node node)
        {
            List<int> validValues = DefaultDomain.ToList();

            if (node.Editable)
            {
                // Explicitly ToList the original so that we can remove stuff from it
                foreach (int value in validValues.ToList())
                {
                    // If any of the node's neighbors contain the value, it's invalid
                    // i.e. A node in the same row, column, or 2x3 square
                    if (node.Neighbors.Any(n => n.Value == value))
                        validValues.Remove(value);
                }
            }
            else
            {
                // Not editable, we already know the value
                validValues.RemoveAll(value => value != node.Value);
            }

            return validValues;
        }

        public Node GetNode(int row, int col)
        {
            return Grid.First(node => node.Row == row && node.Column == col);
        }

        // After the grid is constructed we can set the neighbors of each node
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
            else if (node.Row <= 1 && node.Column > 2)
                boxNum = 2;
            // Middle-left
            else if (node.Row > 1 && node.Row < 3 && node.Column <= 2)
                boxNum = 3;
            // Middle-right
            else if (node.Row > 1 && node.Row < 3 && node.Column > 2)
                boxNum = 4;
            // Bottom-left
            else if (node.Row > 3 && node.Column <= 2)
                boxNum = 5;
            // Bottom-right
            else if (node.Row > 3 && node.Column > 2)
                boxNum = 6;

            return boxNum;
        }
    }
}
