using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Common.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ZY.Common.Tools.Tests
{
    [TestClass()]
    public class FormateToolTests
    {
        [TestMethod()]
        public void ToStandardRadianTest()
        {
            //1角度 = Pi/180 弧度
            //360角度为 360*Pi/180 (即 2 Pi) 弧度

            //1. Not a number
            var result = FormateTool.ToStandardRadian(double.NaN);
            Assert.AreEqual(result, double.NaN);

            //2. 弧度为2Pi+1，结果应为1
            var result1 = FormateTool.ToStandardRadian(2 * Math.PI + 1);
            Assert.AreEqual(result1, 1);

            //3. 弧度为4Pi+4，结果应为4
            var result2 = FormateTool.ToStandardRadian(4 * Math.PI + 4);
            Assert.AreEqual(result2, 4);

            //4. 弧度为-4Pi-4，结果应为Pi(标准弧度为 0-2Pi)
            var result3 = FormateTool.ToStandardRadian(-4 * Math.PI - Math.PI);
            Assert.AreEqual(result3, Math.PI);
        }

        [TestMethod]
        public void Test_ToStandardByPrecisionDouble()
        {
            double doubleValueBase = 37.8;
            double doubleValueEqual = 37.800001;

            int baseInt = doubleValueBase.ToStandardByPrecision(OverrallVraTool.DoublePrecision);
            int equalInt = doubleValueEqual.ToStandardByPrecision(OverrallVraTool.DoublePrecision);

            Assert.IsTrue(Math.Abs(doubleValueBase - doubleValueEqual) < OverrallVraTool.DoublePrecision);
            Assert.AreEqual(baseInt, equalInt);
        }

        [TestMethod]
        public void Test_ToStandardByPrecisionSpeed()
        {
            double speedValueBase = 380;
            double speedValueEqual = 380.003;

            int baseInt = speedValueBase.ToStandardByPrecision(OverrallVraTool.SpeedPrecision);
            int equalInt = speedValueEqual.ToStandardByPrecision(OverrallVraTool.SpeedPrecision);

            Assert.IsTrue(Math.Abs(speedValueBase - speedValueEqual) < OverrallVraTool.SpeedPrecision);
            Assert.AreEqual(baseInt, equalInt);
        }

        [TestMethod]
        public void Test_ToStandardByPrecisionGradient()
        {
            double gradientValueBase = 380;
            double gradientValueEqual = 380.0004;

            int baseInt = gradientValueBase.ToStandardByPrecision(OverrallVraTool.GradientPrecision);
            int equalInt = gradientValueEqual.ToStandardByPrecision(OverrallVraTool.GradientPrecision);

            Assert.IsTrue(Math.Abs(gradientValueBase - gradientValueEqual) < OverrallVraTool.GradientPrecision);
            Assert.AreEqual(baseInt, equalInt);
        }

        [TestMethod]
        public void Test_ToStandardByPrecisionHorizontalDistance()
        {
            double horizontalDistanceValueBase = 380;
            double horizontalDistanceValueEqual = 380.00004;

            int baseInt = horizontalDistanceValueBase.ToStandardByPrecision(OverrallVraTool.HorizontalDistancePrecision);
            int equalInt = horizontalDistanceValueEqual.ToStandardByPrecision(OverrallVraTool.HorizontalDistancePrecision);

            Assert.IsTrue(Math.Abs(horizontalDistanceValueBase - horizontalDistanceValueEqual) < OverrallVraTool.HorizontalDistancePrecision);
            Assert.AreEqual(baseInt, equalInt);
        }

        [TestMethod]
        public void Test_ToStandardByPrecisionVerticalDistance()
        {
            double verticalDistanceValueBase = 380;
            double verticalDistanceValueEqual = 380.00004;

            int baseInt = verticalDistanceValueBase.ToStandardByPrecision(OverrallVraTool.VerticalDistancePrecision);
            int equalInt = verticalDistanceValueEqual.ToStandardByPrecision(OverrallVraTool.VerticalDistancePrecision);

            Assert.IsTrue(Math.Abs(verticalDistanceValueBase - verticalDistanceValueEqual) < OverrallVraTool.VerticalDistancePrecision);
            Assert.AreEqual(baseInt, equalInt);
        }

        [TestMethod]
        public void Test_ToStandardByPrecisionBearing()
        {
            double bearingValueBase = 380;
            double bearingValueEqual = 380.003;

            int baseInt = bearingValueBase.ToStandardByPrecision(OverrallVraTool.BearingPrecision);
            int equalInt = bearingValueEqual.ToStandardByPrecision(OverrallVraTool.BearingPrecision);

            Assert.IsTrue(Math.Abs(bearingValueBase - bearingValueEqual) < OverrallVraTool.BearingPrecision);
            Assert.AreEqual(baseInt, equalInt);
        }

        [TestMethod]
        public void Test_ToStandardByPrecisionFrequency()
        {
            double frequencyValueBase = 380;
            double frequencyValueEqual = 380.0003;

            int baseInt = frequencyValueBase.ToStandardByPrecision(OverrallVraTool.FrequencyPrecision);
            int equalInt = frequencyValueEqual.ToStandardByPrecision(OverrallVraTool.FrequencyPrecision);

            Assert.IsTrue(Math.Abs(frequencyValueBase - frequencyValueEqual) < OverrallVraTool.FrequencyPrecision);
            Assert.AreEqual(baseInt, equalInt);
        }

        [TestMethod]
        public void Test_ToStandardByPrecisionTemperature()
        {
            double temperatureValueBase = 380;
            double temperatureValueEqual = 380.0003;

            int baseInt = temperatureValueBase.ToStandardByPrecision(OverrallVraTool.TemperaturePrecision);
            int equalInt = temperatureValueEqual.ToStandardByPrecision(OverrallVraTool.TemperaturePrecision);

            Assert.IsTrue(Math.Abs(temperatureValueBase - temperatureValueEqual) < OverrallVraTool.FrequencyPrecision);
            Assert.AreEqual(baseInt, equalInt);
        }
    }
}
