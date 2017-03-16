namespace Ereadian.MudSdk.Sdk.UnitTest.Runtime
{
    using System;
    using System.Collections.Specialized;
    using System.Xml;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ereadian.MudSdk.Sdk.Runtime;

    [TestClass]
    public class DefaultObjectFactoryUnitTest
    {
        [TestMethod]
        public void DefaultObjectFactory_Convert()
        {
            // Arrange
            var rootElement = CreateRootElement();
            var value = Guid.NewGuid().ToString();
            var itemElement = rootElement.AddTextElement(DefaultObjectFactory.FactoryElementName, value);
            AddAttribute(itemElement, DefaultObjectFactory.SourceTypeAttributeName, typeof(string).FullName);
            AddAttribute(itemElement, DefaultObjectFactory.TargetTypeAttributeName, typeof(string).FullName);
            AddAttribute(itemElement, DefaultObjectFactory.ConvertAttributeName, bool.TrueString);
            var factory = new DefaultObjectFactory(rootElement, null);

            // Action
            string actual;
            var success = factory.TryGetInstance<string>(out actual);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(value, actual);
        }

        [TestMethod]
        public void DefaultObjectFactory_FromConfiguration()
        {
            var configuration = new NameValueCollection();
            var key = Guid.NewGuid().ToString("N");
            var value = Guid.NewGuid().ToString();
            configuration.Add(key, value);
            var rootElement = CreateRootElement();
            var itemElement = rootElement.AddTextElement(DefaultObjectFactory.FactoryElementName, key);
            AddAttribute(itemElement, DefaultObjectFactory.SourceTypeAttributeName, typeof(string).FullName);
            AddAttribute(itemElement, DefaultObjectFactory.TargetTypeAttributeName, typeof(string).FullName);
            AddAttribute(itemElement, DefaultObjectFactory.ConvertAttributeName, bool.TrueString);
            AddAttribute(itemElement, DefaultObjectFactory.FromConfigurationAttributeName, bool.TrueString);

            var factory = new DefaultObjectFactory(rootElement, configuration);
            string actual;
            var success = factory.TryGetInstance<string>(out actual);

            Assert.IsTrue(success);
            Assert.AreEqual(value, actual);
        }

        private static XmlElement CreateRootElement()
        {
            var document = new XmlDocument();
            return document.CreateElement(DefaultObjectFactory.RootElementName);
        }

        private static XmlAttribute AddAttribute(XmlElement element, string attributeName, string value)
        {
            var attribute = element.OwnerDocument.CreateAttribute(attributeName);
            if (value != null)
            {
                attribute.Value = value;
            }

            element.Attributes.Append(attribute);
            return attribute;
        }
    }
}
