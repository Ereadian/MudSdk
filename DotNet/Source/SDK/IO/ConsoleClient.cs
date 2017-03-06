﻿//------------------------------------------------------------------------------------------------------------------------------------------ 
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
        public void RenderMessage(IReadOnlyList<IContent> content, ColorIndex colorIndex, IReadOnlyList<object> parameters)
        {
            var currentForegroundColor = Console.ForegroundColor;
            var currenBackgroundColor = Console.BackgroundColor;
            for (var i = 0; i < content.Count; i++)
            {
                var data = content[i];
                switch (data.Type)
                {
                    case ContentType.Color:
                        var colorContent = data as ColorContent;
                        var color = GetColor(colorContent.ForegroundColorId, colorIndex);
                        Console.ForegroundColor = color.HasValue ? color.Value : currentForegroundColor;

                        color = GetColor(colorContent.BackgroundColorId, colorIndex);
                        Console.BackgroundColor = color.HasValue ? color.Value : currenBackgroundColor;

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
                        Console.Write(text.Text);
                        break;
                }
            }

            Console.ForegroundColor = currentForegroundColor;
            Console.BackgroundColor = currenBackgroundColor;
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
            if (Enum.TryParse(name, true, out color))
            {
                return color;
            }

            return null;
        }
    }
}
