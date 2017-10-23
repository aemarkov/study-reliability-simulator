using System.Collections.Generic;
using System.Linq;

namespace SimCore
{
    public class Graph
    {
        public Dictionary<string, Vertex> Vertex { get; set; }

        public List<Element> Elements { get; set; }

        public Graph()
        {
            Vertex = new Dictionary<string, Vertex>();
            Elements = new List<Element>();
        }
    }

    public class Vertex
    {
        public  string Name { get; set; }

        /// <summary>
        /// Интенсивность отказов узла
        /// </summary>
        public double Intensity => Elements.Sum(x => x.Amount * x.Element.Intensity);

        /// <summary>
        /// Элементы, из которых состоит узел
        /// </summary
        public List<(Element Element, int Amount)> Elements { get; set; }

        public List<Edge> Edges { get; set; }


        public Vertex()
        {
            Elements = new List<(Element Element, int Amount)>();
            Edges = new List<Edge>();
        }

        public override string ToString() => Name;
    }

    /// <summary>
    /// Вся связь
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// Длина связи
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Удельная интенсивость отказов
        /// </summary>
        public double SpecificIntensity { get; set; }

        /// <summary>
        /// Интенсивность отказов всей связи
        /// </summary>
        public double Intensity => Length * SpecificIntensity;

        
        public Vertex Vertex { get; set; }
        public string NodeName { get; set; }

        public override string ToString() => $"{NodeName} ({Length})";
    }

    /// <summary>
    /// Элемент, из которых состоят ноды
    /// </summary>
    public class Element
    {
        /// <summary>
        /// Название элемента
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Интенсивнгость отказов, 10^-6 1/час
        /// </summary>
        public double Intensity { get; set; }

        public override string ToString() => $"{Name} ({Intensity})";
    }
}
