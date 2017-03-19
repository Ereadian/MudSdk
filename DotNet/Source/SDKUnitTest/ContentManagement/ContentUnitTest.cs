namespace Ereadian.MudSdk.Sdk.UnitTest.ContentManagement
{
    using System;
    using System.Globalization;
    using Ereadian.MudSdk.Sdk.UnitTest.Common;
    using Ereadian.MudSdk.Sdk.ContentManagement;

    public class ContentUnitTest
    {
        public virtual void Content_Constructor_EmptyContent()
        {
            // Arrange
            var colorManager = new ColorManager();

            // Action
            var content = new Content(null, colorManager);

            // Assert
            Assert.AreEqual(0, content.Data.Count);
        }

        public virtual void Content_Constructor_TextOnly()
        {
            // Arrange
            var text = "This is a text with numbers like 1 2 33 44 and so on.";
            var colorManager = new ColorManager();

            // Action
            var content = new Content(text, colorManager);

            // Assert
            var data = content.Data;
            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(ContentType.Text, data[0].Type);
            var expectedText = data[0] as TextContent;
            Assert.AreEqual(text, expectedText.Text);
        }

        public virtual void Content_Constructor_ColorAroundText()
        {
            // Arrange
            var colorManager = new ColorManager();
            var foregroundColor = Guid.NewGuid().ToString("N");
            var backgroundColor = Guid.NewGuid().ToString("N");
            var text = Guid.NewGuid().ToString("N");
            var rawContent = string.Format(
                CultureInfo.InvariantCulture,
                "{0}{2}:{3}{1}{4}{0}{1}",
                Content.ColorTagStart,
                Content.ColorTagEnd,
                foregroundColor,
                backgroundColor,
                text);

            // Action
            var content = new Content(rawContent, colorManager);
            var data = content.Data;

            // Assert
            Assert.AreEqual(3, data.Count);
            Assert.AreEqual(ContentType.Color, data[0].Type);
            Assert.AreEqual(foregroundColor, colorManager[(data[0] as ColorContent).ForegroundColorId]);
            Assert.AreEqual(backgroundColor, colorManager[(data[0] as ColorContent).BackgroundColorId]);

            Assert.AreEqual(ContentType.Text, data[1].Type);
            Assert.AreEqual(text, (data[1] as TextContent).Text);

            Assert.AreEqual(ContentType.Color, data[2].Type);
            Assert.AreEqual(ContentType.Color, data[2].Type);
            Assert.AreEqual(ColorManager.UndefinedColorId, (data[2] as ColorContent).ForegroundColorId);
            Assert.AreEqual(ColorManager.UndefinedColorId, (data[2] as ColorContent).BackgroundColorId);
        }
    }
}
