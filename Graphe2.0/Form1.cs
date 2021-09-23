using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphe2._0 { 
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private List<PictureBox> pb;
        private int[,] Ints;
        private DataGridView dgv;
        int a = 0;
        private Button btn2;
        private Button b1;
        private Button b3;
        public Form1()
        {
            InitializeComponent();
            this.Height = SystemInformation.PrimaryMonitorSize.Height - 100 ;
            this.Width = SystemInformation.PrimaryMonitorSize.Width;
            pb = new List<PictureBox>();
            WindowState = FormWindowState.Maximized;

            b1 = new Button();
            b1.Location = new Point(this.Width / 2, Height - 50);
            b1.Size = new Size(50, 20);
            Controls.Add(b1);
            b1.Text = "Draw!";
            b1.Click += b1_Click;


            dgv = new DataGridView();
            dgv.Location = new Point(Width - 300, 0);
            dgv.Size = new Size(300, 150);

            btn2 = new Button();
            btn2.Location = new Point(Width - 300, dgv.Height + 20);
            btn2.Text = "OK";
            btn2.Size = new Size(30, 30);
            btn2.Click += Btn2_Click;
            Controls.Add(btn2);

            Controls.Add(dgv);

            b3 = new Button();
            b3.Location = new Point(Width / 2 , Height - 20);
            b3.Text = "Now find a Road";
            b3.Size = new Size(100, 30);
            b3.Click += B3_Click;

        }

        static List<Node> DijkstraAlgoritm(Graph graph, Dictionary<Edges, double> weights, Node start, Node end)
        {
            List<Node> notVisited = graph.Nodes.ToList();
            var track = new Dictionary<Node, DijkstraData>();
            track[start] = new DijkstraData { Previous = null, Price = 0 };

            while (true)
            {
                Node toOpen = null;
                double bestPrice = double.PositiveInfinity;
                foreach (var v in notVisited)
                {
                    if (track.ContainsKey(v) && track[v].Price < bestPrice)
                    {
                        toOpen = v;
                        bestPrice = track[v].Price;
                    }
                }
                if (toOpen == null) return null;
                if (toOpen == end) break;
                foreach (var e in toOpen.IncidentEdges.Where(z => z.From == toOpen))
                {
                    var currentPrice = track[toOpen].Price + weights[e];
                    var nextnode = e.OtherNode(toOpen);
                    if (!track.ContainsKey(nextnode) || track[nextnode].Price > currentPrice)
                        track[nextnode] = new DijkstraData { Price = currentPrice, Previous = toOpen };
                }
                notVisited.Remove(toOpen);
            }
            var result = new List<Node>();
            while (end != null)
            {
                result.Add(end);
                end = track[end].Previous;
            }
            result.Reverse();
            return result;
        }



        private void B3_Click(object sender, EventArgs e)
        {
               
        }

        private void Btn2_Click(object sender, EventArgs e)
        {
            dgv.AllowUserToAddRows = false;
            dgv.RowCount = pb.Count;
            dgv.ColumnCount = pb.Count;

            for (int i = 0; i < pb.Count; i++)
            {
                string I = $"{i}";
                dgv.Columns[i].HeaderText = I;
                dgv.Rows[i].HeaderCell.Value = I;
            }
        }

        private void b1_Click(object sender, EventArgs e)
        {

            Controls.Remove(btn2);
            Controls.Remove(b1);
            Controls.Remove(dgv);

            graphics = CreateGraphics();
            Ints = new int[a, a];

            Dictionary<Edges, double> weights = new Dictionary<Edges, double>();

            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < a; j++)
                {
                    if (Convert.ToInt32(dgv.Rows[i].Cells[j].Value) == 1)
                        Ints[i, j] = 1;
                }
            }

            Pen p1 = new Pen(Color.Black);
            Graph graph = new Graph(a);
            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < a; j++)
                {
                    if (Ints[i,j] == 1)
                    {
                        double z = Math.Sqrt(((pb[i].Location.X - pb[j].Location.X) * (pb[i].Location.X - pb[j].Location.X)) + ((pb[i].Location.Y - pb[j].Location.Y) * (pb[i].Location.Y - pb[j].Location.Y)));
                        graph.Connect(i, j);
                        weights[(Edges)graph[i, j]] = z;
                        graphics.DrawLine(p1, pb[i].Location.X, pb[i].Location.Y, pb[j].Location.X, pb[j].Location.Y);
                    }
                }
            }
           


            Thread.Sleep(3000);

            Pen p2 = new Pen(Color.Red);
            var x = DijkstraAlgoritm(graph, weights, (Node)graph[0], (Node)graph[graph.Length - 1]);
            int[] b = new int[x.Count];

            for (int i = 0; i < x.Count; i++)
            {
                b[i] = x[i].NodeNumber;
            }

            Graphics g2 = CreateGraphics();
            for (int i = 0; i < b.Length; i++)
            {
                if (i == b.Length - 1)
                    break;

                g2.DrawLine(p2, pb[b[i]].Location, pb[b[i + 1]].Location);

            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

          
            SolidBrush sb = new SolidBrush(Color.Red);
            SolidBrush sb1 = new SolidBrush(Color.Black);
            if(e.Button == MouseButtons.Left)
            {
                PictureBox pic = new PictureBox();
                pic.Location = new Point(e.Location.X, e.Location.Y);
                
                pic.Size = new Size(30, 30);

                pb.Add(pic);
                Controls.Add(pic);


                pic.Image = new Bitmap(pic.Width, pic.Height);
                graphics = Graphics.FromImage(pic.Image);
                graphics.FillEllipse(sb, -2, -2, 33, 33);
                graphics.DrawString($"{a}", this.Font, sb1, 9, 10);
                //pic.Refresh();
                a++;
            }
        }

        
    }
}
