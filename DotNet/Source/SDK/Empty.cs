//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Empty.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    /// <summary>
    /// Empty objects
    /// </summary>
    public static class Empty<T>
    {
        private static readonly T[] EmptyArray = new T[0];

        public static T[] Array
        {
            get
            {
                return EmptyArray;
            }
        }
    }
}