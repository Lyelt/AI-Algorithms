using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI
{
    internal class Node
    {
        public int Row { get; set; }

        public int Column { get; set; }

        public int Value { get; set; }

        public List<Node> Neighbors { get; set; }

        public List<int> Domain { get; set; }

        // A node with the same row and column number is considered equal
        public override bool Equals(object obj)
        {
            Node other = obj as Node;

            if (other == null)
                return false;

            return other.Row == this.Row && other.Column == this.Column;
        }

        // Required when overriding equals
        public override int GetHashCode()
        {
            return this.Row.GetHashCode() ^ this.Column.GetHashCode();
        }
    }
}
