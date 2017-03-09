using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    public class BlankContent : IContent
    {
        public BlankContent(string text, int start, int end)
        {
            start = ContentUtility.SkipBlank(text, start, end);
            var data = ContentUtility.GetToken(text, start, end);
            this.Text = string.IsNullOrEmpty(data) ? string.Empty : data;

            start += data.Length;
            start = ContentUtility.SkipBlank(text, start, end);
            var countText = ContentUtility.GetToken(text, start, end);

            int count;
            this.Count = int.TryParse(countText, out count) ? count : 1;
        }

        public string Text { get; set; }

        public int Count { get; set; }

        /// <summary>
        /// Gets content type
        /// </summary>
        public ContentType Type
        {
            get
            {
                return ContentType.Blank;
            }
        }
    }
}
