﻿//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Program.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.UI.Console
{
    using System;
    using System.Text;

    class Program
    {
        private static readonly Encoding DefaultEncoding = Encoding.Unicode;

        static void Main(string[] args)
        {
            Console.InputEncoding = DefaultEncoding;
            Console.OutputEncoding = DefaultEncoding;
        }
    }
}
