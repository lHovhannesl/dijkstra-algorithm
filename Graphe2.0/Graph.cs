using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphe2._0
{
    class Graph
    {
        private Node[] nodes;

        public Graph(int nodesCount)
        {
            nodes = Enumerable.Range(0, nodesCount).Select(z => new Node(z)).ToArray();
        }

        public int Length { get { return nodes.Length; } }
        public object this[int index, int in2]
        {
            get
            {
                foreach (var n in Edges)
                    if (n.From.NodeNumber == index && n.To.NodeNumber == in2)
                        return n;
                    else if (n.From.NodeNumber == in2 && n.To.NodeNumber == index)
                        return n;
                return null;
            }
        }
        public object this[int index]
        {
            get
            {
                return nodes[index];
            }
        }

        public IEnumerable<Node> Nodes { get { foreach (var node in nodes) yield return node; } }

        public void Connect(int index1, int index2)
        {
            Node.Connect(nodes[index1], nodes[index2], this);
        }

        public IEnumerable<Edges> Edges { get { return nodes.SelectMany(z => z.IncidentEdges.Distinct()); } }

        public static Graph MakeGraph(params int[] incidentNodes)
        {
            var graph = new Graph(incidentNodes.Max() + 1);
            for (int i = 0; i < incidentNodes.Length - 1; i += 2)
                graph.Connect(incidentNodes[i], incidentNodes[i + 1]);
            return graph;
        }
    }
}
