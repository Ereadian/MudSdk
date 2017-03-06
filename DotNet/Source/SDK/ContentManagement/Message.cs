namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    using System.Collections.Generic;

    public struct Message
    {
        public IReadOnlyList<IContent> Template;
        public IReadOnlyList<object> Parameters;

        public Message(IReadOnlyList<IContent> template, IReadOnlyList<object> parameters)
        {
            this.Template = template;
            this.Parameters = parameters;
        }
    }
}
