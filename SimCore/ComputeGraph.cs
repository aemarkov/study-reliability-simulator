using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimCore
{
    /// <summary>
    /// Класс графа, осуществляющий вычисления
    /// </summary>
    public class ComputeGraph
    {
        class Node
        {
            public double Lambda;
            public bool IsBroke;
            public Wire[] Wires;
        }

        class Wire
        {
            public double Lambda;
            public bool IsBroke;
            private bool Bidirectional;
            public Node EndNode;
        }



        private Node[] _nodes;
    }

}
