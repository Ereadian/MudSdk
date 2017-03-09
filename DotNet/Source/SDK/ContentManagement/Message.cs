namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    using System;
    using System.Collections.Generic;

    public struct Message
    {
        public static Message NewLineMessage = new Message(
            new IContent[] { new TextContent(Environment.NewLine) },
            null);

        public IReadOnlyList<IContent> Template;
        public IReadOnlyList<object> Parameters;
        
        public Message(IReadOnlyList<IContent> template, IReadOnlyList<object> parameters)
        {
            this.Template = template;
            this.Parameters = parameters;
        }

        public Message(IReadOnlyList<Text> template, int localeId, IReadOnlyList<object> parameters)
        {
            this.Template = ContentUtility.GetContent(template, localeId);
            this.Parameters = parameters;
        }
    }
}
