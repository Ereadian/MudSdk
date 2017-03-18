namespace Ereadian.MudSdk.Sdk.UnitTest.ContentManagement
{
    using System;
    using System.Globalization;
    using Ereadian.MudSdk.Sdk.UnitTest.Common;
    using Ereadian.MudSdk.Sdk.ContentManagement;

    public class ContentUtilityUnitTest
    {
        public virtual void ContentUtility_EmptyContent()
        {
            // Arrange
            var colorManager = new ColorManager();

            // Action
            var text = ContentUtility.FormalizeContent(null, colorManager);

            // Assert
            Assert.AreEqual(0, text.Count);
        }

        public virtual void ContentUtility_TextOnly()
        {
            // Arrange
            var data = "This is a text with numbers like 1 2 33 44 and so on.";
            var colorManager = new ColorManager();

            // Action
            var contents = ContentUtility.FormalizeContent(data, colorManager);

            // Assert
            Assert.AreEqual(1, contents.Count);
            Assert.AreEqual(ContentType.Text, contents[0].Type);
            var content = contents[0] as TextContent;
            Assert.AreEqual(data, content.Text);
        }

        public virtual void ContentUtility_ColorAroundText()
        {
            // Arrange
            var colorManager = new ColorManager();
            var foregroundColor = Guid.NewGuid().ToString("N");
            var backgroundColor = Guid.NewGuid().ToString("N");
            var text = Guid.NewGuid().ToString("N");
            var data = string.Format(
                CultureInfo.InvariantCulture,
                "{0}{2}:{3}{1}{4}{0}{1}",
                ContentUtility.ColorTagStart,
                ContentUtility.ColorTagEnd,
                foregroundColor,
                backgroundColor,
                text);

            // Action
            var contents = ContentUtility.FormalizeContent(data, colorManager);

            // Assert
            Assert.AreEqual(3, contents.Count);
            Assert.AreEqual(ContentType.Color, contents[0].Type);
            Assert.AreEqual(foregroundColor, colorManager[(contents[0] as ColorContent).ForegroundColorId]);
            Assert.AreEqual(backgroundColor, colorManager[(contents[0] as ColorContent).BackgroundColorId]);

            Assert.AreEqual(ContentType.Text, contents[1].Type);
            Assert.AreEqual(text, (contents[1] as TextContent).Text);

            Assert.AreEqual(ContentType.Color, contents[2].Type);
            Assert.AreEqual(ContentType.Color, contents[2].Type);
            Assert.AreEqual(ColorManager.UndefinedColorId, (contents[2] as ColorContent).ForegroundColorId);
            Assert.AreEqual(ColorManager.UndefinedColorId, (contents[2] as ColorContent).BackgroundColorId);
        }
    }
}
