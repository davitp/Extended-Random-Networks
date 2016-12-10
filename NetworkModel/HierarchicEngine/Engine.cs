using System;
using System.Collections.Generic;

namespace NetworkModel.HierarchicEngine
{
    // Вспомогательный класс-инженер (для реализации алгоритма Флойда).
    public class Engine
    {
        static public int[,] MinPath(int[,] graphMatrix)
        {
            int n = Convert.ToInt32(Math.Sqrt(graphMatrix.Length));
            int[,] distance = new int[n, n];

            //set all 0 elements an infinity values
            for (int v = 0; v < n; ++v)
            {
                for (int w = 0; w < n; ++w)
                {
                    if (graphMatrix[v, w] == 0)
                    {
                        distance[v, w] = int.MaxValue;
                    }
                    else
                    {
                        distance[v, w] = 1;
                    }
                }
            }

            for (int i = 0; i < n; ++i)
            {
                for (int v = 0; v < n; ++v)
                {
                    for (int w = 0; w < n; ++w)
                    {
                        if (distance[v, i] != int.MaxValue && distance[i, w] != int.MaxValue)
                        {
                            int d = distance[v, i] + distance[i, w];
                            if (distance[v, w] > d)
                            {
                                distance[v, w] = d;
                            }
                        }
                    }
                }
            }

            return distance;
        }

        // Статическая функция, реализующая алгоритм Флойда, для вычисления минимальных путей между всеми вершинами графа.
        static public long[] FloydMinPath(int[,] graphMatrix)
        {
            int n = Convert.ToInt32(Math.Sqrt(graphMatrix.Length));
            int[,] distance = new int[n, n];

            //set all 0 elements an infinity values
            for (int v = 0; v < n; ++v)
            {
                for (int w = 0; w < n; ++w)
                {
                    if (graphMatrix[v, w] == 0) 
                    {
                        distance[v, w] = int.MaxValue;
                    }
                    else 
                    {
                        distance[v, w] = 1;
                    }
                }
            }

            for (int i = 0; i < n; ++i)
            {
                for (int v = 0; v < n; ++v)
                {
                    for (int w = 0; w < n; ++w)
                    {
                        if (distance[v, i] != int.MaxValue && distance[i, w] != int.MaxValue)
                        {
                            int d = distance[v, i] + distance[i, w];
                            if (distance[v, w] > d)
                            {
                                distance[v, w] = d;
                            }
                        }
                    }
                }
            }

            int[] info = {0, 0, 0};
            long[] retInfo = {0, 0, 0};
            for (int v = 0; v < n; ++v)
            {
                for (int w = 0; w < n; ++w)
                {
                    if (distance[v, w] != int.MaxValue && v != w)
                    {
                        if (distance[v, w] <= 2)
                        {
                            info[0] += distance[v, w];
                        }
                        else
                        {
                            info[1]++;
                            info[2] += distance[v, w];
                        }
                    }
                    else if (v != w)
                    { 
                        info[1]++;
                    }
                }
            }

            retInfo[0] = info[0] / 2;
            retInfo[1] = info[1] / 2;
            retInfo[2] = info[2] / 2;

            return retInfo;
        }
    }
}
