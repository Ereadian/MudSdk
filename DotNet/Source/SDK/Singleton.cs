//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Singleton.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    /// <summary>
    /// Singleton support
    /// </summary>
    /// <typeparam name="T">type of singleton target</typeparam>
    public static class Singleton<T> where T : new()
    {
        /// <summary>
        /// Gets singleton instance
        /// </summary>
        public static T Instance
        {
            get
            {
                return Store.Instance;
            }
        }

        /// <summary>
        /// Singleton store
        /// </summary>
        private static class Store
        {
            /// <summary>
            /// Singleton data
            /// </summary>
            private static readonly T SingletonInstance;

            /// <summary>
            /// Initializes static members of the <see cref="Store" /> class.
            /// </summary>
            static Store()
            {
                SingletonInstance = new T();
            }

            /// <summary>
            /// Gets singleton instance
            /// </summary>
            internal static T Instance
            {
                get
                {
                    return SingletonInstance;
                }
            }
        }
    }
}