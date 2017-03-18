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
    public class ContentUtilityTestRunner : ContentUtilityUnitTest
    {
        [TestMethod]
        public override void ContentUtility_FormalizeContent_EmptyContent()
        {
            base.ContentUtility_FormalizeContent_EmptyContent();
        }

        [TestMethod]
        public override void ContentUtility_FormalizeContent_TextOnly()
        {
            base.ContentUtility_FormalizeContent_TextOnly();
        }

        [TestMethod]
        public override void ContentUtility_FormalizeContent_ColorAroundText()
        {
            base.ContentUtility_FormalizeContent_ColorAroundText();
        }
    }
}
