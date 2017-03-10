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
        private static readonly IReadOnlyDictionary<string, string> BlankTextMapping
            = new Dictionary<string, string>
            {
                { "l", Environment.NewLine },
                { "b", " " },
            };

        private readonly Action DisconnectConectClient;
        private readonly Game game;

        public ConsoleClient(Game game, Action disconnectConectClient)
        {
            this.game = game;
            this.DisconnectConectClient = disconnectConectClient;
        }

        /// <summary>
        /// Gets or sets connect id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Render message
        /// </summary>
        /// <param name="message">message to render</param>
        /// <param name="colorIndex">color index</param>
        public void RenderMessage(Message message)
        {
            ColorIndex colorIndex = this.game.Colors;
            var currentForegroundColor = Console.ForegroundColor;
            var currenBackgroundColor = Console.BackgroundColor;
            var content = message.Template;
            var parameters = message.Parameters;
            lock(this)
            {
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
                        case ContentType.Blank:
                            var blankContent = data as BlankContent;
                            string output;
                            if (!BlankTextMapping.TryGetValue(blankContent.Text, out output))
                            {
                                output = Environment.NewLine;
                            }

                            for (var j = 0; j < blankContent.Count; j++)
                            {
                                Console.Write(output);

                                if (output == Environment.NewLine)
                                {
                                    for (var k = 1; k < this.game.Settings.LineSpace; k++)
                                    {
                                        Console.WriteLine();
                                    }
                                }
                            }

                            break;
                        default:
                            var text = data as TextContent;
                            Console.Write(text.Text);
                            break;
                    }
                }
            }

            Console.ForegroundColor = currentForegroundColor;
            Console.BackgroundColor = currenBackgroundColor;
            for (var i = 0; i < this.game.Settings.LineSpace; i++)
            {
                Console.WriteLine();
            }
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
