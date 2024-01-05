using System;
using System.Collections.Generic;
using System.Windows;
using System.Text;
using System.Threading.Tasks;

namespace WeightedTree
{
    //Організація роботи дерева
    public class Tree
    {
        //Клас для завдання властивостей кожному вузлу в дереві
        public class TreeNode
        {
            public int Key;
            public string Value;
            public string id;
            public TreeNode Left;
            public TreeNode Right;
            public bool first;
        }

        public TreeNode Node;
        public int levelCount;
        public int count;
        public List<TreeNode> Que = new List<TreeNode>();

        //Метод для створення черги з вузлів
        public void CreateQue(string str, int key)
        {
            table = new Dictionary<string, string>();
            Que.Add(new TreeNode());
            count++;
            Que[Que.Count - 1].Value = str;
            Que[Que.Count - 1].Key = key;
        }

        //Метод, який створює виважене бінарне дерево на основі алгоритму Хаффмана
        public void CreateTree()
        {
            Sort();
            while (Que.Count > 1)
            {
                Que[0] = MergerNode(Que[0], Que[1]);
                Que.RemoveAt(1);
                Sort();
            }
            Node = Que[0];
            Node.first = false;
            FindId(ref Node);
        }

        //Метод, для розташування частот у порядку зростання
        public void Sort()
        {
            TreeNode tmp;

            for (int i = 0; i < Que.Count; i++)
                for (int j = 0; j < Que.Count; j++)
                {
                    if (Que[i].Key < Que[j].Key)
                    {
                        tmp = Que[i];
                        Que[i] = Que[j];
                        Que[j] = tmp;
                    }
                }
        }

        //Метод, який для двох вузлів з найменшими частотами, формує бінарне дерево
        //з їхніми вузлами і частотою сумою частот цих вузлів
        public TreeNode MergerNode(TreeNode node1, TreeNode node2)
        {
            TreeNode t = new TreeNode();
            t.Right = node2;
            t.Left = node1;
            t.Key = node1.Key + node2.Key;

            return t;
        }

        string str = "";
        public Dictionary<string, string> table;

        //Метод, який формує код Хаффмана для кожного елемента зваженого дерева
        public void FindId(ref TreeNode node)
        {
            if (node.Value != null)
            {
                node.id = str;
                table.Add(node.Value, node.id);
                node.first = false;
                str = str.Remove(str.Length - 1);
            }
            else
            {
                if (node.Left != null)
                {
                    node.first = false;
                    str += "0";
                    FindId(ref node.Left);
                }
                if (node.Right != null)
                {
                    node.first = false;
                    str += "1";
                    FindId(ref node.Right);
                }

                if (!node.first)
                {
                    if (str.Length != 0)
                        str = str.Remove(str.Length - 1);
                    else return;
                }
            }
        }

        //Метод, що дозволяє виконати декодування рядка за кодом Хаффмана
        public string FindNode(string key)
        {
            string ss = "";
            TreeNode node = Node;
            char[] ch = key.ToCharArray();
            string str = ch[0].ToString();
            int i = 0;

            while (i != ch.Length)
            {
                if (ch[i] == '0')
                {
                    if (node.Left != null)
                    {
                        node = node.Left;
                        if (node.id == str)
                            ss += node.Value;
                    }
                    else ss += "0";
                }
                else if (ch[i] == '1')
                {
                    if (node.Right != null)
                    {
                        node = node.Right;
                        if (node.id == str)
                            ss += node.Value;
                    }
                    else ss += "1";            
                }

                if (node.Left == null && node.Right == null)
                {
                    node = Node;
                    str = "";
                }

                if (ch[i] == ' ')
                {
                    ss += ' ';
                    str = "";
                }

                i++;
                if (i != ch.Length)
                    str += ch[i];
            }
            return ss;
        }

        public string[][] k;

        //Метод, що обчислює координати кожного елемента дерева, для того щоб його намалювати на формі
        public TreePoints[][] Print(TreePoints[][] l, double x1, double x2, double y1, double y2, double w, double h)
        {
            l = new TreePoints[levelCount][];
            k = new string[levelCount][];
            for (int i = 0; i < levelCount; i++)
            {
                int f = (int)Math.Pow(2, i);
                l[i] = new TreePoints[(int)Math.Pow(2, i)];
                k[i] = new string[(int)Math.Pow(2, i)];

                for (int j = 0; j < f; j++)
                    l[i][j]  = new TreePoints();
            }

            PrintRecursion2(levelCount);

            double wS, hS = h / levelCount;
            double x0, y0 = y1 + hS / 2;

            for (int i = 0; i < levelCount; i++)
            {
                int m = k[i].Length;
                wS = (double)w / (2 * m);
                x0 = wS + x1;
                for (int j = 0; j < m; j++)
                {

                    if (k[i][j] != "empty")
                    {
                        l[i][j].X = x0;
                        l[i][j].Y = y0;
                    }
                    x0 += 2.0f * wS;
                }
                y0 += hS;
            }

            return l;
        }

        private void PrintRecursion2(int countLevel)
        {
            List<TreeNode> Q = new List<TreeNode>();
            Q.Add(Node);
            for (int i = 0; i < countLevel; i++)
            {
                int m = Q.Count;
                for (int j = 0; j < m; j++)
                {
                    if (Q[j] != null)
                    {
                        if (Q[j].Value == null)
                            k[i][j] = " ";
                        else
                            k[i][j] = Q[j].Value;

                        Q.Add(Q[j].Left);
                        Q.Add(Q[j].Right);
                    }
                    else
                    {
                        k[i][j] = "empty";
                        Q.Add(null);
                        Q.Add(null);
                    }
                
                }
                for (int j = 0; j < m; j++)
                    Q.RemoveAt(0);
            }
            Q.Clear();
        }        
    }
}

