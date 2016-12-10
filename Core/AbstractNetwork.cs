using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Practices.EnterpriseLibrary.Logging;

using Core.Attributes;
using Core.Enumerations;
using Core.Exceptions;
using Core.Events;
using Core.Model;
using Core.Result;
using Core.Utility;
using Core.Settings;

namespace Core
{
    /// <summary>
    /// Abstract class presenting random network.
    /// </summary>
    public abstract class AbstractNetwork
    {
        public String ResearchName { get; private set; }
        public GenerationType GenerationType { get; private set; }
        public Dictionary<ResearchParameter, object> ResearchParameterValues { get; private set; }
        public Dictionary<GenerationParameter, object> GenerationParameterValues { get; private set; }
        public AnalyzeOption AnalyzeOptions { get; private set; }

        protected INetworkGenerator networkGenerator;
        protected INetworkAnalyzer networkAnalyzer;

        public int NetworkID { get; set; }
        public bool SuccessfullyCompleted { get; private set; }
        public RealizationResult NetworkResult { get; protected set; }

        public event NetworkStatusUpdateHandler OnUpdateStatus;

        public static AbstractNetwork CreateNetworkByType(ModelType mt, String rName,
            GenerationType gType,
            Dictionary<ResearchParameter, object> rParams,
            Dictionary<GenerationParameter, object> genParams,
            AnalyzeOption AnalyzeOptions)
        {
            ModelTypeInfo[] info = (ModelTypeInfo[])mt.GetType().GetField(mt.ToString()).GetCustomAttributes(typeof(ModelTypeInfo), false);
            Type t = Type.GetType(info[0].Implementation);
            Type[] constructTypes = new Type[] {
                    typeof(String),
                    typeof(GenerationType),
                    typeof(Dictionary<ResearchParameter, object>),
                    typeof(Dictionary<GenerationParameter, object>), 
                    typeof(AnalyzeOption) };
            object[] invokeParams = new object[] {
                    rName,
                    gType,
                    rParams,
                    genParams, 
                    AnalyzeOptions };
            return (AbstractNetwork)t.GetConstructor(constructTypes).Invoke(invokeParams);
        }

        public AbstractNetwork(String rName,
            GenerationType gType,
            Dictionary<ResearchParameter, object> rParams,
            Dictionary<GenerationParameter, object> genParams,
            AnalyzeOption AnalyzeOptions)
        {
            ResearchName = rName;
            GenerationType = gType;
            ResearchParameterValues = rParams;
            GenerationParameterValues = genParams;
            this.AnalyzeOptions = AnalyzeOptions;

            NetworkResult = new RealizationResult();
        }

        /// <summary>
        /// Generates random network from generation parameters.
        /// </summary>
        public bool Generate()
        {
            try
            {
                Logger.Write("Research - " + ResearchName +
                    ". GENERATION STARTED for network - " + NetworkID.ToString());

                if (GenerationType == GenerationType.Static)
                {
                    Debug.Assert(GenerationParameterValues.ContainsKey(GenerationParameter.AdjacencyMatrix));
                    MatrixPath fp = (MatrixPath)GenerationParameterValues[GenerationParameter.AdjacencyMatrix];
                    Debug.Assert(fp.Path != null && fp.Path != "");
                    MatrixInfoToRead matrix = FileManager.Read(fp.Path, fp.Size);
                    networkGenerator.StaticGeneration(matrix);

                    Logger.Write("Research - " + ResearchName +
                        ". Static GENERATION FINISHED for network - " + NetworkID.ToString());
                }
                else
                {
                    Debug.Assert(!GenerationParameterValues.ContainsKey(GenerationParameter.AdjacencyMatrix));
                    networkGenerator.RandomGeneration(GenerationParameterValues);

                    Logger.Write("Research - " + ResearchName +
                        ". Random GENERATION FINISHED for network - " + NetworkID.ToString());
                }

                UpdateStatus(NetworkStatus.StepCompleted);
            }
            catch(CoreException ex)
            {
                UpdateStatus(NetworkStatus.Failed);

                Logger.Write("Research - " + ResearchName +
                    "GENERATION FAILED for network - " + NetworkID.ToString() +
                    ". Exception message: " + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Calculates specified analyze options values.
        /// </summary>
        public bool Analyze()
        {
            if (networkAnalyzer.Container == null)
                networkAnalyzer.Container = networkGenerator.Container;
            
            try
            {
                Logger.Write("Research - " + ResearchName +
                    ". ANALYZE STARTED for network - " + NetworkID.ToString());

                NetworkResult.NetworkSize = networkAnalyzer.Container.Size;
                NetworkResult.EdgesCount = networkAnalyzer.CalculateEdgesCount();

                Array existingOptions = Enum.GetValues(typeof(AnalyzeOption));
                foreach (AnalyzeOption opt in existingOptions)
                {
                    if (opt != AnalyzeOption.None && (AnalyzeOptions & opt) == opt)
                    {
                        NetworkResult.Result.Add(opt, networkAnalyzer.CalculateOption(opt));
                        UpdateStatus(NetworkStatus.StepCompleted);

                        Logger.Write("Research - " + ResearchName +
                            ". CALCULATED analyze option: " +  opt.ToString() +
                            " for network - " + NetworkID.ToString());
                    }
                }

                SuccessfullyCompleted = true;

                Logger.Write("Research - " + ResearchName +
                        ". ANALYZE FINISHED for network - " + NetworkID.ToString());
            }
            catch (SystemException ex)
            {
                UpdateStatus(NetworkStatus.Failed);

                Logger.Write("Research - " + ResearchName +
                    "ANALYZE FAILED for network - " + NetworkID.ToString() +
                    ". Exception message: " + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Traces the adjacency matrix of generated network to file.
        /// </summary>
        public bool Trace(string tracingPath)
        {
            try
            {
                Logger.Write("Research - " + ResearchName +
                    ". TRACING STARTED for network - " + NetworkID.ToString());

                if (TracingIsNotSupported())
                    return true;

                if (RandNetSettings.TracingType == TracingType.Matrix)
                {
                    MatrixInfoToWrite matrixInfo = new MatrixInfoToWrite();
                    matrixInfo.Matrix = networkGenerator.Container.GetMatrix();
                    if (networkGenerator.Container is AbstractHierarchicContainer)
                        matrixInfo.Branches = (networkGenerator.Container as AbstractHierarchicContainer).GetBranches();

                    FileManager.Write(matrixInfo, tracingPath);
                }
                else if (RandNetSettings.TracingType == TracingType.Neighbourship)
                {
                    NeighbourshipInfoToWrite neighbourshipInfo = new NeighbourshipInfoToWrite();
                    neighbourshipInfo.Neighbourship = networkGenerator.Container.GetNeighbourship();
                    if (networkGenerator.Container is AbstractHierarchicContainer)
                        neighbourshipInfo.Branches = (networkGenerator.Container as AbstractHierarchicContainer).GetBranches();

                    FileManager.Write(neighbourshipInfo, tracingPath);
                }

                UpdateStatus(NetworkStatus.StepCompleted);

                Logger.Write("Research - " + ResearchName +
                        ". TRACING FINISHED for network - " + NetworkID.ToString());
            }
            catch (CoreException ex)
            {
                UpdateStatus(NetworkStatus.Failed);

                Logger.Write("Research - " + ResearchName +
                    "TRACING FAILED for network - " + NetworkID.ToString() +
                    ". Exception message: " + ex.Message);
                return false;
            }

            return true;
        }

        private bool TracingIsNotSupported()
        {
            bool support = false;
            if (networkGenerator.Container is AbstractHierarchicContainer)
            {
                networkAnalyzer.Container = networkGenerator.Container;
                support = (networkAnalyzer.CalculateEdgesCount() > 10000000);
            }
            else support = (networkGenerator.Container.Size > 10000);

            if (support)
            {
                UpdateStatus(NetworkStatus.StepCompleted);

                Logger.Write("Research - " + ResearchName +
                        ". TRACING is SKIPPED for network - " + NetworkID.ToString());
            }

            return support;
        }

        public void UpdateStatus(NetworkStatus status)
        {
            // Make sure someone is listening to event
            if (OnUpdateStatus == null) 
                return;

            // Invoke event for AbstractEnsembleManager
            OnUpdateStatus(this, new NetworkEventArgs(status));
        }
    }
}
