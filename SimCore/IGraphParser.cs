namespace SimCore
{
    public interface IGraphParser
    {
        Graph OpenGraph(string graphPath);
        void SaveGraph(Graph graph, string filename);
    }
}