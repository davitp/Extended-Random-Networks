using QuickGraph;

namespace NetworkModel
{
    public interface IQuickGraphConverter
    {
        UndirectedGraph<int, Edge<int>> ToQuickGraph();
    }
}