using System.IO;

namespace SimCore
{
    public class SimpleGraphParser : IGraphParser
    {
        private double _specificIntencity;

        public SimpleGraphParser(double specificIntensity)
        {
            _specificIntencity = specificIntensity;
        }

        /// <summary>
        /// Парсит граф
        /// </summary>
        /// <param name="graphPath"></param>
        /// <returns></returns>
        public Graph OpenGraph(string graphPath)
        {
            if(!File.Exists(graphPath))
                throw new IOException($"Graph file \"{graphPath}\" not found");

            string elementsPath = GetElementsPath(graphPath);

            if (!File.Exists(elementsPath))
                throw new IOException($"Elements file \"{elementsPath}\" for given graph file not found");

            Graph graph = new Graph();

            // Парсим структуру
            ParseGraphStructure(graph, graphPath);

            // Парсим элементы, из которых состоят вершины
            ParseGraphElements(graph, elementsPath);
            

            return graph;
        }

       
        private void ParseGraphStructure(Graph graph, string graphPath)
        {
            // Читаем структуру графа, представленную в виде списка связанности в CSV
            /*
             * Имя узла; Имя соседнего узла; Длина связи; Имя соседнего узла; Длина связи; ...
             * Имя узла; Имя соседнего узла; Длина связи; Имя соседнего узла; Длина связи; ...
             * ...
             */
            using (var reader = new StreamReader(graphPath))
            {
                string line;
                int lineIndex = 0;


                while ((line = reader.ReadLine()) != null)
                {
                    // Читаем и проверяем линию на корректность
                    string[] lineItems = line.Split(';');

                    if (lineItems.Length % 2 != 1)
                        throw new GraphParseException($"Invalid number of elements at line {lineIndex}");

                    if (string.IsNullOrWhiteSpace(lineItems[0]))
                        throw new GraphParseException($"Invalid name of vertex at line {lineIndex}");

                    var vertex = new Vertex();
                    vertex.Name = lineItems[0];

                    // Парсим связи
                    for (int i = 1; i < lineItems.Length - 1; i += 2)
                    {
                        // Читем имя соединенной вершины
                        // Если пусто - то прекращаем читать связи этой вершины, даже если потом что-то будет
                        string connectedName = lineItems[i];
                        if (string.IsNullOrWhiteSpace(connectedName))
                            break;

                        if (!double.TryParse(lineItems[i + 1], out double length))
                            throw new GraphParseException($"Edge length is not a number at line {lineIndex}, column {i+1}");

                        // Добавляем связь
                        vertex.Edges.Add(new Edge()
                        {
                            Length = length,
                            NodeName = connectedName,
                            SpecificIntensity = _specificIntencity
                        });
                    }

                    graph.Vertex.Add(vertex.Name, vertex);
                    lineIndex++;
                }


                // Добавлены связи через имена, теперь надо восстановить ссылки
                FixGraphReferences(graph);
            }
        }


        // Восстанавливает ссылки на вершины в связях на основе имен вершин
        private static void FixGraphReferences(Graph graph)
        {
            foreach (var vertex in graph.Vertex)
            {
                foreach (var edge in vertex.Value.Edges)
                {
                    if (!graph.Vertex.ContainsKey(edge.NodeName))
                        throw new GraphParseException($"Vertex with name {edge.NodeName} not found (at vertex {vertex.Value})");

                    edge.Vertex = graph.Vertex[edge.NodeName];
                }
            }
        }

        private void ParseGraphElements(Graph graph, string elementsPath)
        {
            using (var reader = new StreamReader(elementsPath))
            {
                string line;
                int lineIndex = 1;

                // Парсим заголовок
                line = reader.ReadLine();
                if(line == null)
                    throw new GraphParseException("Elements file is empty");

                string[] vertexNames = line.Split(';');
                if (vertexNames.Length != graph.Vertex.Count + 2)
                    throw new GraphParseException($"Invalid number of columns in elements table at line {lineIndex}");


                lineIndex++;

                // Парсим значения
                while ((line = reader.ReadLine()) != null)
                {
                    // Читаем и проверяем линию на корректность
                    string[] lineItems = line.Split(';');

                    if(lineItems.Length != graph.Vertex.Count + 2)
                        throw new GraphParseException($"Invalid number of columns in elements table at line {lineIndex}");

                    // Название элемента
                    string elementName = lineItems[0];
                    if (string.IsNullOrWhiteSpace(elementName))
                        throw new GraphParseException($"Element name is empty at line {lineIndex}");

                    // Интенсивность отказов элемента
                    if(!double.TryParse(lineItems[1], out double intensity))
                        throw new GraphParseException($"Intensity is not a number at line {lineIndex}, column 2");

                    var element = new Element() {Intensity = intensity, Name = elementName};
                    graph.Elements.Add(element);


                    // Читаем кол-во элементов в узлах
                    for (int i = 2; i < lineItems.Length; i++)
                    {
                        string vertexName = vertexNames[i];
                        if(!graph.Vertex.ContainsKey(vertexName))
                            throw new GraphParseException($"Vertex with name {vertexName} not found");

                        if(lineItems[i].Trim()=="-" || string.IsNullOrWhiteSpace(lineItems[i]))
                            continue;

                        if(!int.TryParse(lineItems[i], out int amount))
                            throw new GraphParseException($"Elements amount is not number at line {lineIndex}, column {i+1}");

                        if(amount == 0)
                            continue;

                        graph.Vertex[vertexName].Elements.Add((element, amount));
                    }

                    lineIndex++;
                }
            }
        }



        public void SaveGraph(Graph graph, string filename)
        {
            throw new System.NotImplementedException();
        }


        private string GetElementsPath(string graphPath)
        {
            string filename = Path.GetFileNameWithoutExtension(graphPath);
            string elementsFilename = $"{filename}_elements.csv";
            return Path.Combine(Path.GetDirectoryName(graphPath), elementsFilename);
        }
    }
}
 