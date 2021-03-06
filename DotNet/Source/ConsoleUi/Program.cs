﻿//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Program.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.UI.Console
{
    using Ereadian.MudSdk.Sdk.Executors;

    class Program
    {
        static void Main(string[] args)
        {
            var executor = new ConsoleSinglePlayer();
            executor.Run(args);
        }
    }
}
