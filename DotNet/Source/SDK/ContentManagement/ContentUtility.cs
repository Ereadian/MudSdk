//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="ContentUtility.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    /// <summary>
    /// Content operation utility
    /// </summary>
    public static class ContentUtility
    {
        /// <summary>
        /// Skip blank
        /// </summary>
        /// <param name="content">content to process</param>
        /// <param name="start">start location</param>
        /// <param name="end">end location</param>
        /// <returns>new location after skipped blank</returns>
        public static int SkipBlank(string content, int start, int end)
        {
            while ((start < end) && !char.IsLetterOrDigit(content[start]))
            {
                start++;
            }

            return start;
        }

        /// <summary>
        /// Get token
        /// </summary>
        /// <param name="content">content to process</param>
        /// <param name="start">start point</param>
        /// <param name="end">end point</param>
        /// <returns>token from content</returns>
        public static string GetToken(string content, int start, int end)
        {
            int current;
            for (current = start; (current < end) && char.IsLetterOrDigit(content[current]); current++)
            {
            }

            var length = current - start;
            return length == 0 ? string.Empty : content.Substring(start, length);
        }

        public static Resource GetResource<T>(T id) where T : struct
        {
            return Singleton<Resources<T>>.Instance[id];
        }

        public static Message CreateMessage<T>(T resourceId)
            where T : struct
        {
            return CreateMessage(resourceId, LocaleManager.DefaultLocaleId, null);
        }

        public static Message CreateMessage<T>(T resourceId, int localeId, params object[] parameters)
            where T : struct
        {
            var resource = GetResource<T>(resourceId);
            return new Message(resource, parameters);
        }
    }
}
