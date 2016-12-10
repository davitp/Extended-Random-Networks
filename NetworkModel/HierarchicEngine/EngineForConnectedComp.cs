using System;
using System.Collections.Generic;
using System.Collections;

namespace NetworkModel.HierarchicEngine
{
    // Вспомогательный класс-инженер (для вычисления связанных компонентов).
    public class EngineForConnectedComp
    {
        // Количество вершин в графе.
        private int N;
        // Граф, в котором ищем сильно связные компоненты.
        private int[][] edges;  
        private int[] edges_c;
        // Транспонированый граф
        private int[][] edgesT; 
        private int[] edgesT_c;
        // Ипользуется в поиске для того, чтобы отмечать посещенные вершины.
        private int[] state;
        // Список предварительной расстановки вершин.
        private int[] f;        
        private int last_f;
        // Номер компоненты (увеличиваем его, когда находим новую)
        private int c;

        private Dictionary<int, ArrayList> conn_comp;

        public EngineForConnectedComp() { }

        public ArrayList GetCountConnSGraph(Dictionary<int, ArrayList> graph, int countNodes)
        {
            int[][] grp = new int[countNodes][];
            for (int i = 0; i < countNodes; i++)
            {
                grp[i] = new int[graph[i].Count];
                for (int j = 0; j < grp[i].Length; j++)
                    grp[i][j] = Convert.ToInt32(graph[i][j]);
            }
            FindConnSGruph(grp, countNodes);

            ArrayList arr = new ArrayList();
            for (int i = 0; i < conn_comp.Count; i++)
                if (conn_comp[i].Count > 1)
                    arr.Add(conn_comp[i].Count);
            return arr;
        }

        public Dictionary<int, ArrayList> GetConnSGraph(Dictionary<int, ArrayList> graph, 
            int countNodes)
        {
            int[][] grp = new int[countNodes][];
            for (int i = 0; i < countNodes; ++i)
            {
                grp[i] = new int[graph[i].Count];
                for (int j = 0; j < grp[i].Length; ++j)
                    grp[i][j] = Convert.ToInt32(graph[i][j]);
            }
            FindConnSGruph(grp, countNodes);
            Dictionary<int, ArrayList> retconn_comp = conn_comp;
            return retconn_comp;
        }
        
        // Утилиты.

        private void FindConnSGruph(int[][] graph, int countNodes)
        {
            N = countNodes;
            edges = graph;
            edges_c = new int[N];
            edgesT_c = new int[N];
            edgesT = new int[N][];
            for (int i = 0; i < N; i++)
                edgesT[i] = new int[N];
            state = new int[N];
            f = new int[N];
            last_f = 0;
            c = 0;
            conn_comp = new Dictionary<int, ArrayList>();

            for (int i = 0; i < N; i++)
            {
                edges_c[i] = edges[i].Length;
                for (int j = 0; j < edges_c[i]; j++)
                {
                    int to = edges[i][j];
                    // Построение транспонированного графа.
                    edgesT[to][edgesT_c[to]++] = i;
                }
            }

            // Нажохдение и вывод сильно связанных компонентов.
            scc();
        }

        // Выполняет обыкнавенный поиск в глубину (проход по всем непосещенным вершинам).
        private void dfs(int node)
        {
            state[node] = 1;

            for (int i = 0; i < edges_c[node]; i++)
                if (state[edges[node][i]] == 0)
                    dfs(edges[node][i]);

            // Предварительная расстановка вершин в списке.
            f[last_f++] = node;                  
        }

        // Выполняет обыкнавенный поиск в глубину в транспонированном графе (проход по всем непосещенным вершинам).
        private void dfsT(int node)
        {
            state[node] = 1;

            for (int i = 0; i < edgesT_c[node]; i++)
                if (state[edgesT[node][i]] == 0)
                    dfsT(edgesT[node][i]);
            
            // Наполнение. 
            if (conn_comp.ContainsKey(c))
                conn_comp[c].Add(node);
            else
            {
                conn_comp.Add(c, new ArrayList());
                conn_comp[c].Add(node);
            }
        }

        // Выденяет сильно связанные компоненты графа.
        private void scc()
        {
            int i;
            for (i = 0; i < N; i++) state[i] = 0;

            // 1-ый поиск в глубину, получение предворительной расстановки вершин.
            for (i = 0; i < N; i++)                  
                if (state[i]++ == 0)
                    dfs(i);

            for (i = 0; i < N; i++) state[i] = 0;

            // 2-ой поиск в глубину, окончательное выделение сильно связанных компонентов.
            for (last_f--; last_f >= 0; last_f--)  
                if (state[f[last_f]] == 0)
                {
                    dfsT(f[last_f]);
                    // Увеличивается номер следующего компонента.
                    c++;                          
                }
        }
    }
}
