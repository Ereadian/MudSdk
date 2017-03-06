//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="ConsoleClient.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.IO
{
    using System;
    using System.Collections.Generic;
    using Ereadian.MudSdk.Sdk.ContentManagement;

    /// <summary>
    /// Console connector
    /// </summary>
    public class ConsoleClient : IClient
    {
        private readonly Action DisconnectConectClient;

        public ConsoleClient(Action disconnectConectClient)
        {
            this.DisconnectConectClient = disconnectConectClient;
        }

        /// <summary>
        /// Gets or sets connect id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Receive message
        /// </summary>
        /// <param name="content">incoming message</param>
        public void ReceiveMessage(IReadOnlyList<IContent> content, ColorIndex colorIndex, IReadOnlyList<object> parameters)
        {
            Console.ResetColor();
            for (var i = 0; i < content.Count; i++)
            {
                var data = content[i];
                switch (data.Type)
                {
                    case ContentType.Color:
                        var colorContent = data as ColorContent;
                        Console.ResetColor();
                        var color = GetColor(colorContent.ForegroundColorId, colorIndex);
                        if (color.HasValue)
                        {
                            Console.ForegroundColor = color.Value;
                        }

                        color = GetColor(colorContent.BackgroundColorId, colorIndex);
                        if (color.HasValue)
                        {
                            Console.BackgroundColor = color.Value;
                        }

                        break;
                    case ContentType.Parameter:
                        var parameterContent = data as ParameterContent;
                        var value = parameters[parameterContent.ParameterId];
                        if (value != null)
                        {
                            Console.Write(value.ToString());
                        }

                        break;
                    default:
                        var text = data as TextContent;
                        Console.Write(text);
                        break;
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Disconnect from game
        /// </summary>
        public virtual void Disconnect()
        {
            this.DisconnectConectClient();
        }

        /// <summary>
        /// Get console color
        /// </summary>
        /// <param name="colorId">color id</param>
        /// <param name="colorIndex">color index</param>
        /// <returns>console color</returns>
        private static ConsoleColor? GetColor(int colorId, ColorIndex colorIndex)
        {
            var name = colorIndex[colorId];
            ConsoleColor color;
            if (Enum.TryParse(name, out color))
            {
                return color;
            }

            return null;
        }
    }
}
