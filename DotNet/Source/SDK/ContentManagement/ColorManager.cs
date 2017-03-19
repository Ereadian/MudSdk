//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="ColorManager.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Color management
    /// </summary>
    public class ColorManager : IDisposable
    {
        /// <summary>
        /// undefined/default color id
        /// </summary>
        public const int UndefinedColorId = 0;

        /// <summary>
        /// colors by id "mapping"
        /// </summary>
        private readonly IList<string> colors = new List<string>() { UndefinedColorName };

        /// <summary>
        /// color name/id id mapping
        /// </summary>
        private readonly IDictionary<string, int> nameIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { UndefinedColorName, UndefinedColorId }
        };

        /// <summary>
        /// read/write lock
        /// </summary>
        private ReaderWriterLockSlim readWriteLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Initializes static members of the <see cref="ColorManager" /> class.
        /// </summary>
        static ColorManager()
        {
            UndefinedColorName = string.Empty;
        }

        /// <summary>
        /// Gets undefined/default color name
        /// </summary>
        public static string UndefinedColorName { get; private set; }

        /// <summary>
        /// Gets color id by name
        /// </summary>
        /// <param name="name">color name</param>
        /// <returns>color id</returns>
        public int this[string name]
        {
            get
            {
                int id;
                this.readWriteLock.EnterUpgradeableReadLock();
                try
                {
                    if (!this.nameIndex.TryGetValue(name, out id))
                    {
                        this.readWriteLock.EnterWriteLock();
                        try
                        {
                            if (!this.nameIndex.TryGetValue(name, out id))
                            {
                                id = this.colors.Count;
                                this.nameIndex.Add(name, id);
                                this.colors.Add(name);
                            }
                        }
                        finally
                        {
                            this.readWriteLock.ExitWriteLock();
                        }
                    }
                }
                finally
                {
                    this.readWriteLock.ExitUpgradeableReadLock();
                }

                return id;
            }
        }

        /// <summary>
        /// Gets color name by id
        /// </summary>
        /// <param name="id">color id</param>
        /// <returns>color name</returns>
        public string this[int id]
        {
            get
            {
                string name;
                this.readWriteLock.EnterReadLock();
                try
                {
                    name = this.colors[id];
                }
                finally
                {
                    this.readWriteLock.ExitReadLock();
                }

                return name;
            }
        }

        /// <summary>
        /// Dispose the object
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (this.readWriteLock != null)
                {
                    this.DisposeItems();
                }
            }
        }

        /// <summary>
        /// Dispose items
        /// </summary>
        protected virtual void DisposeItems()
        {
            this.readWriteLock.Dispose();
            this.readWriteLock = null;
        }
    }
}
