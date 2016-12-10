using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.Attributes;
using Core.Enumerations;
using Core.Result;

namespace Core
{
    /// <summary>
    /// Abstract class representing result storage.
    /// </summary>
    public abstract class AbstractResultStorage
    {
        protected string storageStr;

        public string StorageString
        {
            get { return storageStr; }
        }

        /// <summary>
        /// Creates a storage of specified type using metadata information of enumeration value.
        /// </summary>
        /// <param name="st">Type of storage to create.</param>
        /// <param name="storageStr">Connection string or file path for data storage.</param>
        /// <returns>Newly created storage.</returns>
        public static AbstractResultStorage CreateStorage(StorageType st, string storageStr)
        {
            Type[] patametersType = { typeof(String) };
            object[] invokeParameters = { storageStr };
            StorageTypeInfo[] info = (StorageTypeInfo[])st.GetType().GetField(st.ToString()).GetCustomAttributes(typeof(StorageTypeInfo), false);
            Type t = Type.GetType(info[0].Implementation, true);
            return (AbstractResultStorage)t.GetConstructor(patametersType).Invoke(invokeParameters);
        }

        public AbstractResultStorage(string str)
        {
            storageStr = str;
        }

        /// <summary>
        /// Returns storage type.
        /// </summary>
        /// <returns>Storage type.</returns>
        public abstract StorageType GetStorageType();

        /// <summary>
        /// Saves specified research result to repository.
        /// </summary>
        /// <param name="result">Research result.</param>
        public abstract void Save(ResearchResult result);

        /// <summary>
        /// Deletes specified research result from repository.
        /// </summary>
        /// <param name="result">Research result.</param>
        public abstract void Delete(Guid researchID);

        /// <summary>
        /// Loads all research information from repository.
        /// </summary>
        /// <returns>List of research results with loaded information.</returns>
        public abstract List<ResearchResult> LoadAllResearchInfo();

        /// <summary>
        /// Loads specified research's data from repository.
        /// </summary>
        /// <param name="researchID">ID of research</param>
        /// <returns>Loaded research result data.</returns>
        public abstract ResearchResult Load(Guid researchID);

        /// <summary>
        /// Loads specified data from repository.
        /// </summary>
        /// <param name="name">File name for XML, EXCEL and research name for SQL.</param>
        /// <returns>Loaded research result data.</returns>
        public abstract ResearchResult Load(string name);
    }
}
