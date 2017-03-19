namespace Ereadian.MudSdk.Sdk.UnitTest.ContentManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class ContentTestRunner : ContentUnitTest
    {
        [TestMethod]
        public override void Content_Constructor_EmptyContent()
        {
            base.Content_Constructor_EmptyContent();
        }

        [TestMethod]
        public override void Content_Constructor_TextOnly()
        {
            base.Content_Constructor_TextOnly();
        }

        [TestMethod]
        public override void Content_Constructor_ColorAroundText()
        {
            base.Content_Constructor_ColorAroundText();
        }
    }
}
