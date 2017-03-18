namespace Ereadian.MudSdk.Sdk.UnitTest.Runtime
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Xml;
    using Ereadian.MudSdk.Sdk.Runtime;
    using Ereadian.MudSdk.Sdk.UnitTest.Common;

    public class DefaultObjectFactoryUnitTest
    {
        public virtual void DefaultObjectFactory_Convert_Directly()
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

        public virtual void DefaultObjectFactory_Convert_FromConfiguration()
        {
            // Arrange
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

            // Action
            string actual;
            var success = factory.TryGetInstance<string>(out actual);

            // Assert
            Assert.IsTrue(success);
            Assert.AreEqual(value, actual);
        }

        public virtual void DefaultObjectFactory_CreateInstance_DefaultConstructor()
        {
            // Arrange
            var rootElement = CreateRootElement();
            AddXmlForDefaultConstructor(rootElement);
            var factory = new DefaultObjectFactory(rootElement, null);

            // Action
            ITest instance;
            var success = factory.TryGetInstance<ITest>(out instance);

            // Assert
            Assert.IsTrue(success);
            Assert.IsInstanceOfType(instance, typeof(TestClass));
            Assert.AreEqual(ActionToken.DefaultConstructor,  instance.ActionWasToken);
            Assert.AreEqual(0, instance.Value);
        }

        public virtual void DefaultObjectFactory_CreateInstance_WithParameter()
        {
            // Arrange
            var parameter = 123456;
            var rootElement = CreateRootElement();
            AddXmlForParameterizedConstructor(rootElement, parameter.ToString(CultureInfo.InvariantCulture));
            var factory = new DefaultObjectFactory(rootElement, null);

            // Action
            ITest instance;
            var success = factory.TryGetInstance<ITest>(out instance);

            // Assert
            Assert.IsTrue(success);
            Assert.IsInstanceOfType(instance, typeof(TestClass));
            Assert.AreEqual(ActionToken.WithParameter, instance.ActionWasToken);
            Assert.AreEqual(parameter, instance.Value);
        }

        public virtual void DefaultObjectFactory_CreateInstance_WithParameter_FromConfiguration()
        {
            // Arrange
            var parameter = 123456;
            var configuration = new NameValueCollection();
            var key = Guid.NewGuid().ToString("N");
            configuration.Add(key, parameter.ToString(CultureInfo.InvariantCulture));
            var rootElement = CreateRootElement();
            var itemElement = AddXmlForParameterizedConstructor(rootElement, key, true);
            var factory = new DefaultObjectFactory(rootElement, configuration);

            // Action
            ITest instance;
            var success = factory.TryGetInstance<ITest>(out instance);

            // Assert
            Assert.IsTrue(success);
            Assert.IsInstanceOfType(instance, typeof(TestClass));
            Assert.AreEqual(ActionToken.WithParameter, instance.ActionWasToken);
            Assert.AreEqual(parameter, instance.Value);
        }

        public virtual void DefaultObjectFactory_CreateMultipleInstances()
        {
            // Arrange
            var parameter = 56789;
            var name = "another";
            var rootElement = CreateRootElement();
            AddXmlForDefaultConstructor(rootElement);
            var itemElement = AddXmlForParameterizedConstructor(rootElement, parameter.ToString(CultureInfo.InvariantCulture));
            AddAttribute(itemElement, DefaultObjectFactory.FactoryNameAttributeName, name);
            var factory = new DefaultObjectFactory(rootElement, null);

            // Action
            ITest instanceDefault;
            var successDefault = factory.TryGetInstance<ITest>(out instanceDefault);
            ITest instanceParameterized;
            var successParameterized = factory.TryGetInstance<ITest>(out instanceParameterized, name);

            // Assert
            Assert.IsTrue(successDefault);
            Assert.IsInstanceOfType(instanceDefault, typeof(TestClass));
            Assert.AreEqual(ActionToken.DefaultConstructor, instanceDefault.ActionWasToken);
            Assert.AreEqual(0, instanceDefault.Value);

            Assert.IsTrue(successParameterized);
            Assert.IsInstanceOfType(instanceParameterized, typeof(TestClass));
            Assert.AreEqual(ActionToken.WithParameter, instanceParameterized.ActionWasToken);
            Assert.AreEqual(parameter, instanceParameterized.Value);
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

        private static XmlElement AddXmlForDefaultConstructor(XmlElement rootElement)
        {
            var itemElement = rootElement.OwnerDocument.CreateElement(DefaultObjectFactory.FactoryElementName);
            rootElement.AppendChild(itemElement);
            AddAttribute(itemElement, DefaultObjectFactory.SourceTypeAttributeName, typeof(ITest).AssemblyQualifiedName);
            AddAttribute(itemElement, DefaultObjectFactory.TargetTypeAttributeName, typeof(TestClass).AssemblyQualifiedName);
            return itemElement;
        }

        private static XmlElement AddXmlForParameterizedConstructor(XmlElement rootElement, string parameter, bool fromConfiguration = false)
        {
            var itemElement = rootElement.OwnerDocument.CreateElement(DefaultObjectFactory.FactoryElementName);
            rootElement.AppendChild(itemElement);
            AddAttribute(itemElement, DefaultObjectFactory.SourceTypeAttributeName, typeof(ITest).AssemblyQualifiedName);
            AddAttribute(itemElement, DefaultObjectFactory.TargetTypeAttributeName, typeof(TestClass).AssemblyQualifiedName);
            var constructorElement = rootElement.OwnerDocument.CreateElement(DefaultObjectFactory.ConstructorElementName);
            itemElement.AppendChild(constructorElement);
            var parameterElement = constructorElement.AddTextElement(DefaultObjectFactory.ParameterElementName,parameter);
            AddAttribute(parameterElement, DefaultObjectFactory.TargetTypeAttributeName, typeof(int).FullName);
            AddAttribute(parameterElement, DefaultObjectFactory.ConvertAttributeName, bool.TrueString);
            if (fromConfiguration)
            {
                AddAttribute(parameterElement, DefaultObjectFactory.FromConfigurationAttributeName, bool.TrueString);
            }
            return itemElement;
        }

        public enum ActionToken
        {
            DefaultConstructor,
            WithParameter,
        }

        public interface ITest
        {
            ActionToken ActionWasToken { get;}
            int Value { get; }
        }

        public class TestClass : ITest
        {
            public TestClass()
            {
                this.ActionWasToken = ActionToken.DefaultConstructor;
                this.Value = 0;
            }

            public TestClass(int value)
            {
                this.ActionWasToken = ActionToken.WithParameter;
                this.Value = value;
            }

            public ActionToken ActionWasToken { get; private set; }
            public int Value { get; private set; }
        }
    }
}
