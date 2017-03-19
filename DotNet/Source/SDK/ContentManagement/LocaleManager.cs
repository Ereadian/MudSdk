//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="LocaleIndex.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Locale index
    /// </summary>
    public class LocaleManager
    {
        /// <summary>
        /// Default locale id
        /// </summary>
        public const int DefaultLocaleId = 0;

        /// <summary>
        /// Culture name to culture information mapping
        /// </summary>
        private static readonly IDictionary<string, CultureInfo> CultureNames
            = CultureInfo.GetCultures(CultureTypes.AllCultures)
            .ToDictionary(culture => culture.Name, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// locale name aliases
        /// </summary>
        private static readonly IReadOnlyDictionary<string, string> LocaleAlias
            = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "zh-cn", "zh-hans" },
                { "zh-chs", "zh-hans" },
                { "zh-tw", "zh-hant" },
                { "zh-cht", "zh-hant" },
            };

        /// <summary>
        /// Locale name to locale id mapping
        /// </summary>
        private readonly IDictionary<string, int> localeIds;

        /// <summary>
        /// Culture information list (ordered by locale id)
        /// </summary>
        private readonly IList<CultureInfo> cultures = new List<CultureInfo>();

        /// <summary>
        /// Default locale name
        /// </summary>
        private readonly string defaultLocaleName;

        /// <summary>
        /// Default culture information
        /// </summary>
        private readonly CultureInfo defaultCultureInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocaleManager" /> class.
        /// </summary>
        /// <param name="defaultLocaleName">default locale name</param>
        public LocaleManager(string defaultLocaleName)
        {
            this.defaultLocaleName = MapAlias(defaultLocaleName.Trim());
            this.defaultCultureInfo = CultureNames[this.defaultLocaleName];

            this.localeIds = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { this.defaultLocaleName, DefaultLocaleId }
            };

            this.cultures = new List<CultureInfo>()
            {
                this.defaultCultureInfo
            };
        }

        /// <summary>
        /// Gets locale count
        /// </summary>
        public int LocaleCount
        {
            get
            {
                return this.cultures.Count;
            }
        }

        /// <summary>
        /// Get locale id by name
        /// </summary>
        /// <param name="localeName">locale name</param>
        /// <returns>locale id</returns>
        public int GetLocaleId(string localeName)
        {
            int localeId = DefaultLocaleId;

            if (string.IsNullOrEmpty(localeName))
            {
                localeName = this.defaultLocaleName;
            }
            else
            {
                localeName = MapAlias(localeName.Trim());
            }

            lock (this)
            {
                if (!this.localeIds.TryGetValue(localeName, out localeId))
                {
                    CultureInfo culture;
                    if (!CultureNames.TryGetValue(localeName, out culture))
                    {
                        localeId = DefaultLocaleId;
                    }
                    else
                    {
                        localeId = this.cultures.Count;
                        this.localeIds.Add(localeName, localeId);
                        this.cultures.Add(culture);
                    }
                }
            }

            return localeId;
        }

        /// <summary>
        /// Get culture information by locale id
        /// </summary>
        /// <param name="localeId">locale id</param>
        /// <returns>culture information</returns>
        public CultureInfo GetCulture(int localeId)
        {
            return this.cultures[localeId];
        }

        /// <summary>
        /// Map alias
        /// </summary>
        /// <param name="localeName">locale name</param>
        /// <returns>convert it to alias if it has</returns>
        private static string MapAlias(string localeName)
        {
            string alias;
            return LocaleAlias.TryGetValue(localeName, out alias) ? alias : localeName;
        }
    }
}
