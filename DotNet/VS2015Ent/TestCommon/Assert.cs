namespace Ereadian.MudSdk.Sdk.UnitTest.Common
{
    using System;
    using SystemAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

    public static class Assert
    {
        public static void AreEqual<T>(T expected, T actual)
        {
            SystemAssert.AreEqual<T>(expected, actual);
        }

        public static void IsTrue(bool condition)
        {
            SystemAssert.IsTrue(condition);
        }

        public static void IsInstanceOfType(object value, Type expectedType)
        {
            SystemAssert.IsInstanceOfType(value, expectedType);
        }
    }
}
