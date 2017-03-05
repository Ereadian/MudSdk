//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Serializer.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// XML serializer
    /// </summary>
    /// <typeparam name="T">type of target</typeparam>
    public class Serializer<T>
    {
        /// <summary>
        /// XML serializer
        /// </summary>
        private XmlSerializer serializer = new XmlSerializer(typeof(T));

        /// <summary>
        /// Serialize object
        /// </summary>
        /// <param name="path">target file path</param>
        /// <param name="data">object to serialize</param>
        public void Serialize(string path, T data)
        {
            var folder = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                this.serializer.Serialize(stream, data);
            }
        }

        /// <summary>
        /// Deserialize  object
        /// </summary>
        /// <param name="path">data file path</param>
        /// <returns>deserialized object</returns>
        public T Deserialize(string path)
        {
            var data = default(T);

            if (File.Exists(path))
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    data = (T)this.serializer.Deserialize(stream);
                }
            }

            return data;
        }
    }
}
