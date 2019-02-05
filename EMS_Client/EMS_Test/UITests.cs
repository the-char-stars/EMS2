using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EMS_Client;

namespace EMS_Test_UI
{
    [TestClass]
    public class UITests
    {
        [TestMethod]
        public void TestKeyToNumberConversion()
        {
            Assert.AreEqual(1, Input.KeyToNumber(ConsoleKey.D1));
        }

        [TestMethod]
        public void TestValidChoiceWithInvalidKey()
        {
            Assert.AreEqual(false, Input.IsValidChoice(ConsoleKey.K, 5));
        }

        [TestMethod]
        public void TestValidChoiceWithValidKey()
        {
            Assert.AreEqual(true, Input.IsValidChoice(ConsoleKey.D1, 5));
        }

        [TestMethod]
        public void TestValidChoiceWithOutsideRangeKey()
        {
            Assert.AreEqual(false, Input.IsValidChoice(ConsoleKey.D0, 5));
        }
    }
}
