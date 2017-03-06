﻿using Ereadian.MudSdk.Sdk.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    public struct Text
    {
        public int LocaleId;
        public IReadOnlyList<IContent> Content;

        public Text(int localeId, IReadOnlyList<IContent> content)
        {
            this.LocaleId = localeId;
            this.Content = content;
        }

        public Text(ContentData content, LocaleIndex locales, ColorIndex colors)
            : this(locales.GetLocaleId(content.Locale), ContentUtility.FormalizeContent(content.Data, colors))
        {
        }
    }
}