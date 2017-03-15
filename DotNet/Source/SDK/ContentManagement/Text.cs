//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Text.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    using System.Collections.Generic;
    using Ereadian.MudSdk.Sdk.Globalization;

    public struct Text
    {
        public int LocaleId;
        public IReadOnlyList<IContent> Content;

        public Text(int localeId, IReadOnlyList<IContent> content)
        {
            this.LocaleId = localeId;
            this.Content = content;
        }

        public Text(ContentData content, LocaleManager locales, ColorManager colors)
            : this(locales.GetLocaleId(content.Locale), ContentUtility.FormalizeContent(content.Data, colors))
        {
        }
    }
}
