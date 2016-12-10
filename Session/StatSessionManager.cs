using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core;
using Core.Enumerations;
using Core.Exceptions;
using Core.Attributes;
using Core.Result;
using Core.Settings;

namespace Session
{
    /// <summary>
    /// Statistics organization and manipulation interface.
    /// </summary>
    public static class StatSessionManager
    {
        private static Dictionary<Guid, ResearchResult> existingResults;
        private static Dictionary<int, List<Guid>> existingResultsByGroups;
        private static AbstractResultStorage storage;

        static StatSessionManager()
        {
            existingResults = new Dictionary<Guid, ResearchResult>();
            existingResultsByGroups = new Dictionary<int, List<Guid>>();
            InitializeStorage();
        }

        /// <summary>
        /// Clears all loaded data and reinitializes storage.
        /// </summary>
        public static void Clear()
        {
            existingResults.Clear();
            existingResultsByGroups.Clear();
            InitializeStorage();
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
        /// Retrieves available analyze options for specified research type and model type.
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
        /// Checks if specified option has specified option type/
        /// </summary>
        /// <param name="opt">Analyze option.</param>
        /// <param name="ot">Analyze option type.</param>
        /// <returns></returns>
        public static bool HasOptionType(AnalyzeOption opt, OptionType ot)
        {
            AnalyzeOptionInfo[] info = (AnalyzeOptionInfo[])opt.GetType().GetField(opt.ToString()).GetCustomAttributes(typeof(AnalyzeOptionInfo), false);
            return (info[0].OptionType == ot);
        }

        /// <summary>
        /// Refreshes existing results repository.
        /// <note>Sorts existing results by groups also.</note>
        /// </summary>
        public static void RefreshExistingResults()
        {
            existingResults.Clear();
            List<ResearchResult> results = storage.LoadAllResearchInfo();
            foreach (ResearchResult r in results)
            {
                existingResults.Add(r.ResearchID, r);
            }

            SortExistingResultsByGroups();
        }

        /// <summary>
        /// Deletes specified research from repository.
        /// </summary>
        /// <param name="id">ID of research.</param>
        public static void DeleteResearch(Guid id)
        {
            storage.Delete(id);
            RefreshExistingResults();
        }

        /// <summary>
        /// Gets research result of specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Research result.</returns>
        public static ResearchResult GetResearchResult(Guid id)
        {
            try
            {
                return existingResults[id];
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
                return existingResults[id].ResearchName;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
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
                return existingResults[id].ResearchType;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
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
                return existingResults[id].ModelType;
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
                return existingResults[id].RealizationCount;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Gets the network size for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Size of network.</returns>
        public static uint GetResearchNetworkSize(Guid id)
        {
            try
            {
                return existingResults[id].Size;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Gets the date for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Date of research.</returns>
        public static DateTime GetResearchDate(Guid id)
        {
            try
            {
                return existingResults[id].Date;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Gets the network's edges count for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Edges count of network.</returns>
        public static double GetResearchNetworkEdgesCount(Guid id)
        {
            try
            {
                return existingResults[id].Edges;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Gets research parameter values for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Research parameters with values.</returns>
        public static Dictionary<ResearchParameter, object> GetResearchParameterValues(Guid id)
        {
            try
            {
                return existingResults[id].ResearchParameterValues;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Gets generation parameter values for specified research.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>Generation parameters with values.</returns>
        public static Dictionary<GenerationParameter, object> GetGenerationParameterValues(Guid id)
        {
            try
            {
                return existingResults[id].GenerationParameterValues;
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Loads ensemble results for specified research.
        /// </summary>
        /// <param name="id">ID of research</param>
        /// <note>Method checks if result is not loaded yet.</note>
        public static void LoadResearchResult(Guid id)
        {
            try
            {
                if (existingResults[id].EnsembleResults.Count() == 0)
                    existingResults[id] = storage.Load(id);
            }
            catch (KeyNotFoundException)
            {
                throw new CoreException("Specified research does not exists.");
            }
        }

        /// <summary>
        /// Gets results by groups.
        /// </summary>
        /// <returns>Groups of research ids.</returns>
        public static Dictionary<int, List<Guid>> GetResultsByGroup()
        {
            return existingResultsByGroups;
        }

        /// <summary>
        /// Returns filtered researches by specified research type and model type.
        /// </summary>
        /// <param name="rt">Research type.</param>
        /// <param name="mt">Model type.</param>
        /// <returns>Groups of research ids.</returns>
        public static Dictionary<int, List<Guid>> GetFilteredResultsByGroups(ResearchType rt, ModelType mt)
        {
            Dictionary<int, List<Guid>> result = new Dictionary<int, List<Guid>>();

            foreach (int i in existingResultsByGroups.Keys)
            {
                Guid id = existingResultsByGroups[i].First();
                if (existingResults[id].ResearchType == rt && existingResults[id].ModelType == mt)
                    result.Add(i, existingResultsByGroups[i]);
            }

            return result;
        }

        public static void InitializeStorage()
        {
            storage = CreateStorage();
            RefreshExistingResults();
        }

        /// <summary>
        /// Creates a storage of specified type using metadata information of enumeration value.
        /// </summary>
        /// <param name="st">Type of storage to create.</param>
        /// <param name="storageStr">Connection string or file path for data storage.</param>
        /// <returns>Newly created storage.</returns>
        private static AbstractResultStorage CreateStorage()
        {
            StorageType st = RandNetStatSettings.StorageType;
            string storageStr = null;
            switch (st)
            {
                case StorageType.XMLStorage:
                    storageStr = RandNetStatSettings.XMLStorageDirectory;
                    break;
                case StorageType.ExcelStorage:
                    storageStr = RandNetStatSettings.ExcelStorageDirectory;
                    break;
                default:
                    break;
            }

            Type[] patametersType = { typeof(String) };
            object[] invokeParameters = { storageStr };
            StorageTypeInfo[] info = (StorageTypeInfo[])st.GetType().GetField(st.ToString()).GetCustomAttributes(typeof(StorageTypeInfo), false);
            Type t = Type.GetType(info[0].Implementation, true);
            return (AbstractResultStorage)t.GetConstructor(patametersType).Invoke(invokeParameters);
        }

        private static void SortExistingResultsByGroups()
        {
            existingResultsByGroups.Clear();
            List<ResearchResult> temp = storage.LoadAllResearchInfo();
            int k = 0;
            while (temp.Count != 0)
            {
                List<Guid> current = new List<Guid>();
                ResearchResult currentResult = temp[0];
                current.Add(currentResult.ResearchID);
                temp.Remove(currentResult);
                int i = 0;
                while(i < temp.Count())
                {
                    if (temp[i].ModelType == currentResult.ModelType &&
                        temp[i].ResearchType == currentResult.ResearchType &&
                        temp[i].Size == currentResult.Size &&
                        AreParametersCompatible(temp[i], currentResult))
                    {
                        current.Add(temp[i].ResearchID);
                        temp.Remove(temp[i]);
                    }
                    else
                    {
                        ++i;
                    }
                }
                existingResultsByGroups.Add(k, current);
                ++k;
            }

            return;
        }

        /// <summary>
        /// Checks if research with specified id exists.
        /// </summary>
        /// <param name="id">ID of research.</param>
        /// <returns>True if research exists, false otherwise.</returns>
        public static bool ResearchExists(Guid id)
        {
            return existingResults.ContainsKey(id);
        }

        /// <summary>
        /// Checks if specified results are compatible for statistic analyze.
        /// </summary>
        /// <param name="r1">First result.</param>
        /// <param name="r2">Second result</param>
        /// <returns>True, if results are compatible. False otherwise</returns>
        /// <note>Research result must have same researchType and modelType.</note>
        private static bool AreParametersCompatible(ResearchResult r1, ResearchResult r2)
        {
            foreach (GenerationParameter gp in r1.GenerationParameterValues.Keys)
            {
                if (r1.GenerationParameterValues[gp].ToString() != r2.GenerationParameterValues[gp].ToString())
                    return false;
            }

            foreach (ResearchParameter rp in r1.ResearchParameterValues.Keys)
            {
                if (r1.ResearchParameterValues[rp].ToString() != r2.ResearchParameterValues[rp].ToString())
                    return false;
            }

            return true;
        }
    }
}
