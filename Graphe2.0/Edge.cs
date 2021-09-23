using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphe2._0
{
    class Edges
    {
        public readonly Node From;
        public readonly Node To;
        public Edges(Node first, Node second)
        {
            this.From = first;
            this.To = second;
        }

        public bool IsIncident(Node node)
        {
            return From == node || To == node;
        }

        public Node OtherNode(Node node)
        {
            if (!IsIncident(node)) throw new ArgumentException();
            if (From == node) return To;
            return From;
        }
    }
}

