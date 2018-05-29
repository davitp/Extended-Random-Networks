using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.Practices.EnterpriseLibrary.Logging;

using Core;
using Core.Attributes;
using Core.Enumerations;
using Core.Events;
using Core.Exceptions;
using Core.Result;

namespace Manager
{
    /// <summary>
    /// Implementation of manager, which distributes work on local machine.
    /// </summary>
    public class LocalEnsembleManager : AbstractEnsembleManager
    {
        private Thread[] threads;
        private AutoResetEvent[] waitHandles;
        private ThreadEntryData[] threadData;

        private class ThreadEntryData
        {
            public int ThreadCount { get; private set; }
            public int ThreadIndex { get; set; }

            public ThreadEntryData(int tCount, int tIndex)
            {
                ThreadCount = tCount;
                ThreadIndex = tIndex;
            }
        }

        public override void Run()
        {
            PrepareData();

            for (int i = 0; i < threads.Length; ++i)
            {
                threads[i].Start(threadData[i]);
            }

            AutoResetEvent.WaitAll(waitHandles);

            List<RealizationResult> results = new List<RealizationResult>();
            for (int i = 0; i < networks.Length; ++i)
            {
                if (networks[i].SuccessfullyCompleted)
                    results.Add(networks[i].NetworkResult);
            }

            if(results.Count != 0)
                Result = EnsembleResult.AverageResults(results);
        }

        public override void Cancel()
        {
            for (int i = 0; i < threads.Length && i < Environment.ProcessorCount; ++i)
            {
                if (threads[i] != null)
                {
                    try
                    {
                        threads[i].Abort();
                    }
                    catch (ThreadAbortException ex) { }
                    finally
                    {
                        foreach (AutoResetEvent handle in waitHandles)
                            handle.Set();
                    }
                }
            }
        }

        private void PrepareData()
        {
            networks = new AbstractNetwork[RealizationCount];
            for (int i = 0; i < RealizationCount; ++i)
            {
                networks[i] = AbstractNetwork.CreateNetworkByType(ModelType,
                    ResearchName,
                    ResearchType,
                    GenerationType,
                    ResearchParamaterValues,
                    GenerationParameterValues,
                    AnalyzeOptions,
                    ContainerMode.Fast);

                networks[i].NetworkID = i;
                networks[i].OnUpdateStatus += new NetworkStatusUpdateHandler(NetworkStatusUpdateHandlerMethod);
            }

            int threadCount = Math.Min(networks.Length, Environment.ProcessorCount);
            // Creating thread related members
            threads = new Thread[threadCount];
            waitHandles = new AutoResetEvent[threadCount];
            threadData = new ThreadEntryData[threadCount];

            // Initialize threads and handles
            for (int i = 0; i < threadCount; ++i)
            {
                waitHandles[i] = new AutoResetEvent(false);
                threadData[i] = new ThreadEntryData(threadCount, i);
                threads[i] = new Thread(new ParameterizedThreadStart(ThreadEntry)) { Priority = ThreadPriority.Lowest };
            }
        }

        private void ThreadEntry(object p)
        {
            ThreadEntryData d = (ThreadEntryData)p;

            try
            {
                for (int i = 0; (d.ThreadIndex + i * d.ThreadCount) < networks.Length; ++i)
                {
                    int networkToRun = d.ThreadIndex + i * d.ThreadCount;
                    if (!networks[networkToRun].Generate())
                        continue;
                    if (CheckConnected)
                    {
                        if (!networks[networkToRun].CheckConnected())
                            continue;
                    }
                    if (TracingPath != "")
                    {
                        if (!networks[networkToRun].Trace(TracingPath + "_" + networkToRun.ToString()))
                            continue;
                    }
                    if (!networks[networkToRun].Analyze())
                        continue;

                    Interlocked.Increment(ref realizationsDone);
                }
            }
            catch (SystemException ex)
            {
                Logger.Write("Exception is thrown in LocalEnsembleManager. Exception message is: " + ex.Message);
            }
            finally
            {
                waitHandles[d.ThreadIndex].Set();
            }
        }
    }
}
