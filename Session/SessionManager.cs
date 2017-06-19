using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Logging;

using Core;
using Core.Enumerations;
using Core.Attributes;
using Core.Exceptions;
using Core.Events;
using Core.Result;
using Core.Settings;

namespace Session
{
    /// <summary>
    /// Research organization and manipulation interface.
    /// </summary>
    public static class SessionManager
    {
        private static Dictionary<Guid, AbstractResearch> existingResearches;

        static SessionManager()
        {
            Logger.Write("---------------------- SESSION MANAGER STARTED ----------------------");
            existingResearches = new Dictionary<Guid, AbstractResearch>();
        }

        /// <summary>
        /// Creates a default research and adds to existingResearches.
        /// </summary>
        /// <param name="researchType">The type of research to create.</param>
        /// <returns>ID of created Research.</returns>
        public static Guid CreateResearch(ResearchType researchType)
        {
            AbstractResearch r = AbstractResearch.CreateResearchByType(researchType);
            existingResearches.Add(r.ResearchID, r);
            r.ModelType = GetAvailableModelTypes(r.ResearchID)[0];
            r.ResearchName = "Default";
            r.Storage = AbstractResultStorage.CreateStorage(StorageType.XMLStorage, RandNetSettings.StorageDirectory);
            r.TracingPath = "";
            r.CheckConnected = false;

            return r.ResearchID;
        }

        /// <summary>
        /// Creates a research and adds to existingResearches.
        /// </summary>
        /// <param name="researchType">The type of research to create.</param>
        /// <param name="modelType">The model type of research to create.</param>
        /// <param name="researchName">The name of research.</param>
        /// <param name="storage">The storage type for saving results of analyze.</param>
        /// <param name="storageString">Connection string or file path for data storage.</param>
        /// <param name="tracingPath">Path, if tracing is on, and empty string otherwise.</param>
        /// <param name="checkConnected">Specifies if check for network's connected is on.</param>
        /// <returns>ID of created Research.</returns>
        public static Guid CreateResearch(ResearchType researchType,
            ModelType modelType,
            string researchName,
            StorageType storage,
            string storageString,
            GenerationType generationType,
            string tracingPath,
            bool checkConnected)
        {
            AbstractResearch r = AbstractResearch.CreateResearchByType(researchType);
            existingResearches.Add(r.ResearchID, r);
            r.ModelType = modelType;
            r.ResearchName = researchName;
            r.Storage = AbstractResultStorage.CreateStorage(storage, storageString);
            r.GenerationType = generationType;
            r.TracingPath = tracingPath;
            r.CheckConnected = checkConnected;

            return r.ResearchID;
        }

        /// <summary>
        /// Removes a research from existingResearches, without save.
        /// </summary>
        /// <param name="id">ID of research to destroy.</param>
        public static void DestroyResearch(Guid id)
        {
            try
            {
                existingResearches.Remove(id);
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Clones the specified research.
        /// </summary>
        /// <param name="id">ID of research to clone.</param>
        /// <returns>ID of created Research.</returns>
        public static Guid CloneResearch(Guid id)
        {
            try
            {
                AbstractResearch researchToClone = existingResearches[id];

                AbstractResearch r = AbstractResearch.CreateResearchByType(researchToClone.GetResearchType());
                existingResearches.Add(r.ResearchID, r);
                r.ModelType = researchToClone.ModelType;
                r.Storage = AbstractResultStorage.CreateStorage(researchToClone.Storage.GetStorageType(),
                    researchToClone.Storage.StorageString);
                r.GenerationType = researchToClone.GenerationType;
                r.TracingPath = researchToClone.TracingPath;
                r.CheckConnected = researchToClone.CheckConnected;

                r.RealizationCount = researchToClone.RealizationCount;

                r.ResearchParameterValues = researchToClone.ResearchParameterValues;
                r.GenerationParameterValues = researchToClone.GenerationParameterValues;
                r.AnalyzeOption = researchToClone.AnalyzeOption;
                
                return r.ResearchID;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exist.");
            }
        }

        /// <summary>
        /// Checks if research with specified id exists.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>True if research exists, false otherwise.</returns>
        public static bool ResearchExists(Guid id)
        {
            return existingResearches.ContainsKey(id);
        }

        /// <summary>
        /// Starts a research - Generation, Analyzing, Saving.
        /// </summary>
        /// <param name="id">ID of research to start.</param>
        public static void StartResearch(Guid id)
        {
            try
            {
                if (existingResearches[id].StatusInfo.Status == ResearchStatus.NotStarted)
                    existingResearches[id].StartResearch();
                else
                    throw new CoreException("Unable to start the specified research.");
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Stops a research. Results have to be saved.
        /// </summary>
        /// <param name="id">ID of research to stop</param>
        public static void StopResearch(Guid id)
        {
            try
            {
                if (existingResearches[id].StatusInfo.Status == ResearchStatus.Running)
                    existingResearches[id].StopResearch();
                else
                    throw new CoreException("Unable to stop the specified research");
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Stops all running researches.
        /// </summary>
        public static void StopAllRunningResearches()
        {
            foreach (Guid id in existingResearches.Keys)
            {
                if (existingResearches[id].StatusInfo.Status == ResearchStatus.Running)
                    existingResearches[id].StopResearch();
            }
        }

        /// <summary>
        /// Checks if exists any running research.
        /// </summary>
        /// <returns>'true', if exists. 'false' otherwise.</returns>
        public static bool ExistsAnyRunningResearch()
        {
            if (existingResearches.Count == 0)
                return false;
            else
            {
                foreach (Guid id in existingResearches.Keys)
                {
                    if (existingResearches[id].StatusInfo.Status == ResearchStatus.Running)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Retrieved the type of specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Type of research.</returns>
        public static ResearchType GetResearchType(Guid id)
        {
            try
            {
                return existingResearches[id].GetResearchType();
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Gets the name of specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Name of research.</returns>
        public static string GetResearchName(Guid id)
        {
            try
            {
                return existingResearches[id].ResearchName;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Sets name for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <param name="modelType">Name to set.</param>
        public static void SetResearchName(Guid id, string researchName)
        {
            try
            {
                existingResearches[id].ResearchName = researchName;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Retrieves available model types for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>List of available model types.</returns>
        public static List<ModelType> GetAvailableModelTypes(Guid id)
        {
            try
            {
                return AvailableModelTypes(existingResearches[id].GetType());
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Retrieves available model types for specified research type.
        /// </summary>
        /// <param name="rt">Research type.</param>
        /// <returns>List of available model types.</returns>
        public static List<ModelType> GetAvailableModelTypes(ResearchType rt)
        {
            ResearchTypeInfo[] info = (ResearchTypeInfo[])rt.GetType().GetField(rt.ToString()).GetCustomAttributes(typeof(ResearchTypeInfo), false);
            Type t = Type.GetType(info[0].Implementation, true);

            return AvailableModelTypes(t);
        }

        private static List<ModelType> AvailableModelTypes(Type t)
        {
            List<ModelType> r = new List<ModelType>();
            AvailableModelType[] rAvailableModelTypes = (AvailableModelType[])t.GetCustomAttributes(typeof(AvailableModelType), true);
            for (int i = 0; i < rAvailableModelTypes.Length; ++i)
                r.Add(rAvailableModelTypes[i].ModelType);

            return r;
        }

        /// <summary>
        /// Gets model type for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Model type.</returns>
        public static ModelType GetResearchModelType(Guid id)
        {
            try
            {
                return existingResearches[id].ModelType;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Sets model type for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <param name="modelType">Model type to set.</param>
        public static void SetResearchModelType(Guid id, ModelType modelType)
        {
            try
            {
                existingResearches[id].ModelType = modelType;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Retrieves the storage type for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Storage type of research.</returns>
        public static StorageType GetResearchStorageType(Guid id)
        {
            try
            {
                return existingResearches[id].Storage.GetStorageType();
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Retrieves the storage string for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Storage string.</returns>
        public static string GetResearchStorageString(Guid id)
        {
            try
            {
                return existingResearches[id].Storage.StorageString;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Sets storage for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <param name="storage">The storage type for saving results of analyze.</param>
        /// <param name="storageString">Connection string or file path for data storage.</param>
        public static void SetResearchStorage(Guid id, StorageType storageType, string storageString)
        {
            try
            {
                existingResearches[id].Storage = AbstractResultStorage.CreateStorage(storageType, storageString);
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Retrieves available generation types for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>List of available generation types.</returns>
        public static List<GenerationType> GetAvailableGenerationTypes(Guid id)
        {
            try
            {
                return AvailableGenerationTypes(existingResearches[id].GetType());
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Retrieves available generation types for specified research type
        /// </summary>
        /// <param name="rt">Research type.</param>
        /// <returns>List of available generation types.</returns>
        public static List<GenerationType> GetAvailableGenerationTypes(ResearchType rt)
        {
            ResearchTypeInfo[] info = (ResearchTypeInfo[])rt.GetType().GetField(rt.ToString()).GetCustomAttributes(typeof(ResearchTypeInfo), false);
            Type t = Type.GetType(info[0].Implementation, true);

            return AvailableGenerationTypes(t);
        }

        private static List<GenerationType> AvailableGenerationTypes(Type t)
        {
            List<GenerationType> r = new List<GenerationType>();
            AvailableGenerationType[] rAvailableGenerationTypes = (AvailableGenerationType[])t.GetCustomAttributes(typeof(AvailableGenerationType), true);
            for (int i = 0; i < rAvailableGenerationTypes.Length; ++i)
                r.Add(rAvailableGenerationTypes[i].GenerationType);

            return r;
        }

        /// <summary>
        /// Gets the type of generation for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Generation type.</returns>
        public static GenerationType GetResearchGenerationType(Guid id)
        {
            try
            {
                return existingResearches[id].GenerationType;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Sets the type of generation for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <param name="generationType">Generation type to set.</param>
        public static void SetResearchGenerationType(Guid id, GenerationType generationType)
        {
            try
            {
                existingResearches[id].GenerationType = generationType;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Gets the tracing path for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Tracing path of research.</returns>
        public static string GetResearchTracingPath(Guid id)
        {
            try
            {
                return existingResearches[id].TracingPath;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Sets tracing path for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <param name="modelType">Path to set. Empty, if tracing is off.</param>
        public static void SetResearchTracingPath(Guid id, string tracingPath)
        {
            try
            {
                existingResearches[id].TracingPath = tracingPath;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Gets the check connected setting for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Tracing path of research.</returns>
        public static bool GetResearchCheckConnected(Guid id)
        {
            try
            {
                return existingResearches[id].CheckConnected;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Sets check connected setting for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <param name="modelType">Path to set. Empty, if tracing is off.</param>
        public static void SetResearchCheckConnectedh(Guid id, bool checkConnected)
        {
            try
            {
                existingResearches[id].CheckConnected = checkConnected;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Gets the realization count for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Realization count of research.</returns>
        public static int GetResearchRealizationCount(Guid id)
        {
            try
            {
                return existingResearches[id].RealizationCount;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Sets realization count for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <param name="modelType">Realization count to set. Not less then 1.</param>
        public static void SetResearchRealizationCount(Guid id, int realizationCount)
        {
            try
            {
                existingResearches[id].RealizationCount = realizationCount;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Gets status for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Status.</returns>
        public static ResearchStatusInfo GetResearchStatus(Guid id)
        {
            try
            {
                return existingResearches[id].StatusInfo;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Adds handler for research status updates.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <param name="method">Handler method to add.</param>
        public static void AddResearchUpdateHandler(Guid id,
            ResearchStatusUpdateHandler method)
        {
            try
            {
                existingResearches[id].OnUpdateResearchStatus += method;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Remove handler for research status updates.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <param name="method">Handler method to remove.</param>
        public static void RemoveResearchUpdateHandler(Guid id,
            ResearchStatusUpdateHandler method)
        {
            try
            {
                existingResearches[id].OnUpdateResearchStatus -= method;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Returns count of steps in research working process.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Count of steps.</returns>
        /// <note>Steps are {generation, tracing, each analyze option calculating, save}</note>
        public static int GetProcessStepsCount(Guid id)
        {
            try
            {
                return existingResearches[id].GetProcessStepsCount();
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Returns list of research parameters which are required for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>List of research parameters.</returns>
        public static List<ResearchParameter> GetRequiredResearchParameters(ResearchType rt)
        {
            ResearchTypeInfo[] info = (ResearchTypeInfo[])rt.GetType().GetField(rt.ToString()).GetCustomAttributes(typeof(ResearchTypeInfo), false);
            Type t = Type.GetType(info[0].Implementation, true);

            List<ResearchParameter> rp = new List<ResearchParameter>();
            RequiredResearchParameter[] rRequiredResearchParameters = (RequiredResearchParameter[])t.GetCustomAttributes(typeof(RequiredResearchParameter), true);
            for (int i = 0; i < rRequiredResearchParameters.Length; ++i)
                rp.Add(rRequiredResearchParameters[i].Parameter);

            return rp;
        }

        /// <summary>
        /// Gets research parameters for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Research parameters with values.</returns>
        public static Dictionary<ResearchParameter, object> GetResearchParameterValues(Guid id)
        {
            try
            {
                return existingResearches[id].ResearchParameterValues;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Sets value of specified research parameter for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <param name="p">Research parameter.</param>
        /// <param name="value">Value to set.</param>
        public static void SetResearchParameterValue(Guid id, 
            ResearchParameter p, 
            object value)
        {
            try
            {
                if (existingResearches[id].StatusInfo.Status == ResearchStatus.NotStarted)
                    existingResearches[id].ResearchParameterValues[p] = value;
                else
                    throw new CoreException("Unable to modify research after start.");
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Returns list of generation parameters which are required for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>List of generation parameters.</returns>
        public static List<GenerationParameter> GetRequiredGenerationParameters(ResearchType rt,
            ModelType mt, GenerationType gt)
        {
            List<GenerationParameter> gp = new List<GenerationParameter>();

            if (rt == ResearchType.Collection ||
                rt == ResearchType.Structural)
                return gp;

            if (gt == GenerationType.Static)
            {
                gp.Add(GenerationParameter.AdjacencyMatrix);
                return gp;
            }
            
            ModelTypeInfo[] info = (ModelTypeInfo[])mt.GetType().GetField(mt.ToString()).GetCustomAttributes(typeof(ModelTypeInfo), false);
            Type t = Type.GetType(info[0].Implementation, true);

            RequiredGenerationParameter[] rRequiredGenerationParameters = (RequiredGenerationParameter[])t.GetCustomAttributes(typeof(RequiredGenerationParameter), true);
            for (int i = 0; i < rRequiredGenerationParameters.Length; ++i)
            {
                GenerationParameter g = rRequiredGenerationParameters[i].Parameter;
                if (g != GenerationParameter.AdjacencyMatrix)
                    gp.Add(g);
            }

            return gp;
        }

        /// <summary>
        /// Gets generation parameters for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Generation parameters with values.</returns>
        public static Dictionary<GenerationParameter, object> GetGenerationParameterValues(Guid id)
        {
            try
            {
                return existingResearches[id].GenerationParameterValues;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Sets value of specified generation parameter for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <param name="p">Generation parameter.</param>
        /// <param name="value">Value to set.</param>
        public static void SetGenerationParameterValue(Guid id, 
            GenerationParameter p, 
            object value)
        {
            try
            {
                if (existingResearches[id].StatusInfo.Status == ResearchStatus.NotStarted)
                    existingResearches[id].GenerationParameterValues[p] = value;
                else
                    throw new CoreException("Unable to modify research after start.");
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Gets available analyze options for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Available analyze options.</returns>
        /// <note>Analyze option is available for research, if it is available 
        /// both for research type and model type.</note>
        public static AnalyzeOption GetAvailableAnalyzeOptions(Guid id)
        {
            try
            {
                AvailableAnalyzeOption rAvailableOptions = ((AvailableAnalyzeOption[])existingResearches[id].GetType().GetCustomAttributes(typeof(AvailableAnalyzeOption), true))[0];

                ModelType t = existingResearches[id].ModelType;
                ModelTypeInfo[] info = (ModelTypeInfo[])t.GetType().GetField(t.ToString()).GetCustomAttributes(typeof(ModelTypeInfo), false);
                Type mt = Type.GetType(info[0].Implementation, true);

                AvailableAnalyzeOption mAvailableOptions = ((AvailableAnalyzeOption[])mt.GetCustomAttributes(typeof(AvailableAnalyzeOption), true))[0];

                return rAvailableOptions.Options & mAvailableOptions.Options;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Gets available analyze options for specified research type and model type.
        /// </summary>
        /// <param name="rt">Research type.</param>
        /// <param name="mt">Model type.</param>
        /// <returns>Available analyze options.</returns>
        /// <note>Analyze option is available for research, if it is available 
        /// both for research type and model type.</note>
        public static AnalyzeOption GetAvailableAnalyzeOptions(ResearchType rt, ModelType mt)
        {
            ResearchTypeInfo[] rInfo = (ResearchTypeInfo[])rt.GetType().GetField(rt.ToString()).GetCustomAttributes(typeof(ResearchTypeInfo), false);
            Type researchType = Type.GetType(rInfo[0].Implementation, true);

            AvailableAnalyzeOption rAvailableOptions = ((AvailableAnalyzeOption[])researchType.GetCustomAttributes(typeof(AvailableAnalyzeOption), true))[0];

            ModelTypeInfo[] mInfo = (ModelTypeInfo[])mt.GetType().GetField(mt.ToString()).GetCustomAttributes(typeof(ModelTypeInfo), false);
            Type modelType = Type.GetType(mInfo[0].Implementation, true);

            AvailableAnalyzeOption mAvailableOptions = ((AvailableAnalyzeOption[])modelType.GetCustomAttributes(typeof(AvailableAnalyzeOption), true))[0];

            return rAvailableOptions.Options & mAvailableOptions.Options;
        }

        /// <summary>
        /// Gets analyze options for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Analyze options (flag).</returns>
        public static AnalyzeOption GetAnalyzeOptions(Guid id)
        {
            try
            {
                return existingResearches[id].AnalyzeOption;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Sets analyze options for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <param name="o">Analyze options to set (flag).</param>
        /// <node>param o must be subset from available analyze options.</node>
        public static void SetAnalyzeOptions(Guid id, AnalyzeOption o)
        {
            try
            {
                if (existingResearches[id].StatusInfo.Status == ResearchStatus.NotStarted)
                {
                    AnalyzeOption opt = GetAvailableAnalyzeOptions(id);
                    if ((opt | o) != opt)
                        throw new CoreException("Specified option is not available for current research and model type.");
                    else
                        existingResearches[id].AnalyzeOption = o;
                }
                else
                    throw new CoreException("Unable to modify research after start.");
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }
    }
}