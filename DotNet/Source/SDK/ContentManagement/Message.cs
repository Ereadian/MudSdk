namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    using System;
    using System.Collections.Generic;

    public struct Message
    {
        public static Message NewLineMessage = new Message(
            new Resource(
                new Text[]
                {
                    new Text(
                        LocaleManager.DefaultLocaleId,
                        new Content(
                            new IContent[]
                            {
                                new TextContent(Environment.NewLine)
                            }
                            ))
                }));

        public Resource Resource;
        public IReadOnlyList<object> Parameters;
        
        public Message(Resource resource, params object[] parameters)
        {
            this.Resource = resource;
            this.Parameters = parameters;
        }
    }
}
