//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Game.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Ereadian.MudSdk.Sdk.IO;
    using Ereadian.MudSdk.Sdk.ContentManagement;

    public class Game
    {
        public ColorIndex Colors { get; private set; }

        public virtual void Start(string gameFolder)
        {
        }

        public virtual void Stop()
        {
        }

        public IConnector Connect(IClient connector)
        {
            return new Connector(); ;
        }

        public void Disconnect(IConnector connector)
        {
        }

        protected virtual void WriteConsole(string message)
        {
            if (message != null)
            {
                Console.WriteLine(message);
            }
        }
    }
}
