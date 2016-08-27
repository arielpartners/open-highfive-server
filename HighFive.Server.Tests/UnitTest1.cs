using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HighFive.Server.Services.Models;

namespace HighFive.Server.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private int result { get; set; }
        private Calculator calculator = new Calculator();

        [TestMethod]
        public void TestMethod1()
        {
            calculator.FirstNumber = 10;
            calculator.SecondNumber = 10;
            Assert.AreEqual(20, calculator.Add());
        }
    }
}
