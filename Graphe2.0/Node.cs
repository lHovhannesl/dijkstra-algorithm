using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphe2._0
{
    class Node
    {
        readonly List<Edges> edges = new List<Edges>();
        public readonly int NodeNumber;

        public Node(int num)
        {
            NodeNumber = num;
        }

        public IEnumerable<Node> IncidentNodes { get { return edges.Select(z => z.OtherNode(this)); } }

        public IEnumerable<Edges> IncidentEdges { get { foreach (var e in edges) yield return e; } }

        public static void Connect(Node n1, Node n2, Graph graph)
        {
            if (!graph.Nodes.Contains(n1) || !graph.Nodes.Contains(n2)) throw new ArgumentException();
            var edge = new Edges(n1, n2);
            n1.edges.Add(edge);
            n2.edges.Add(edge);
        }
    }
}
