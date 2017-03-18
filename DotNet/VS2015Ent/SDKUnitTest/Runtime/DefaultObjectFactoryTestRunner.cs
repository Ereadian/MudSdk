namespace Ereadian.MudSdk.Sdk.UnitTest.Runtime
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DefaultObjectFactoryTestRunner : DefaultObjectFactoryUnitTest
    {
        [TestMethod]
        public override void DefaultObjectFactory_Convert_Directly()
        {
            base.DefaultObjectFactory_Convert_Directly();
        }

        [TestMethod]
        public override void DefaultObjectFactory_Convert_FromConfiguration()
        {
            base.DefaultObjectFactory_Convert_FromConfiguration();
        }

        [TestMethod]
        public override void DefaultObjectFactory_CreateInstance_DefaultConstructor()
        {
            base.DefaultObjectFactory_CreateInstance_DefaultConstructor();
        }

        [TestMethod]
        public override void DefaultObjectFactory_CreateInstance_WithParameter()
        {
            base.DefaultObjectFactory_CreateInstance_WithParameter();
        }

        [TestMethod]
        public override void DefaultObjectFactory_CreateInstance_WithParameter_FromConfiguration()
        {
            base.DefaultObjectFactory_CreateInstance_WithParameter_FromConfiguration();
        }

        [TestMethod]
        public override void DefaultObjectFactory_CreateMultipleInstances()
        {
            base.DefaultObjectFactory_CreateMultipleInstances();
        }
    }
}
