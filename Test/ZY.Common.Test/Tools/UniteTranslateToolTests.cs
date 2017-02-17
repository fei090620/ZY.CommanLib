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
    public class UniteTranslateToolTests
    {
        [TestMethod()]
        public void ToAngleTest()
        {
            double radian = 0.1;
            var angle = UniteTranslateTool.ToAngle(radian);
            Assert.AreEqual(angle, radian * 180 / Math.PI);
        }

        [TestMethod()]
        public void ToRadianTest()
        {
            double angle = 90;
            var radian = UniteTranslateTool.ToRadian(angle);
            Assert.AreEqual(radian, angle * Math.PI / 180);
        }

        [TestMethod()]
        public void MeterToFtTest()
        {
            double meter = 1;
            double ft = UniteTranslateTool.MeterToFt(meter);
            Assert.AreEqual(ft, meter * 3.28); ;
        }

        [TestMethod()]
        public void MeterToKilometerTest()
        {
            double meter = 1100;
            double kilometer = UniteTranslateTool.MeterToKilometer(meter);
            Assert.AreEqual(kilometer, meter / 1000);
        }

        [TestMethod()]
        public void KilometerToMeterTest()
        {
            double kilometer = 1.1;
            double meter = UniteTranslateTool.KilometerToMeter(kilometer);
            Assert.AreEqual(meter, kilometer * 1000);
        }

        [TestMethod()]
        public void FtToMeterTest()
        {
            double ft = 1;
            double meter = UniteTranslateTool.FtToMeter(ft);
            Assert.AreEqual(meter, ft / 3.28);
        }

        [TestMethod()]
        public void FtToKilometerTest()
        {
            double ft = 1;
            double kilometer = UniteTranslateTool.FtToKilometer(ft);
            Assert.AreEqual(kilometer, ft / 3.28 / 1000);
        }

        [TestMethod()]
        public void KilometerToFtTest()
        {
            double kilometer = 1;
            double ft = UniteTranslateTool.KilometerToFt(kilometer);
            Assert.AreEqual(ft, kilometer * 3.28 * 1000);
        }

        [TestMethod()]
        public void KilometerToMileTest()
        {
            double kilometer = 1;
            double nMile = UniteTranslateTool.KilometerToMile(kilometer);  //海里
            Assert.AreEqual(nMile, kilometer * 1000 / 1852);
        }

        [TestMethod()]
        public void MileToKilometerTest()
        {
            double nMile = 1;   //海里
            double kilometer = UniteTranslateTool.MileToKilometer(nMile);
            Assert.AreEqual(kilometer, nMile * 1852 / 1000);
        }

        [TestMethod()]
        public void MeterToMileTest()  //海里
        {
            double meter = 1;
            double nMile = UniteTranslateTool.MeterToMile(meter);
            Assert.AreEqual(nMile, meter / 1852);
        }

        [TestMethod()]
        public void MileToMeterTest() //海里
        {
            double nMile = 1;
            double meter = UniteTranslateTool.MileToMeter(nMile);
            Assert.AreEqual(meter, nMile * 1852);
        }

        [TestMethod()]
        public void KM_HtoM_STest()
        {
            double kmh = 1;
            double ms = UniteTranslateTool.KM_HtoM_S(kmh);
            Assert.AreEqual(ms, kmh * 1000 / 3600);
        }

        [TestMethod()]
        public void M_StoKM_HTest()
        {
            double ms = 1;
            double kmh = UniteTranslateTool.M_StoKM_H(ms);
            Assert.AreEqual(kmh, ms / 1000 * 3600);
        }

        [TestMethod()]
        public void KttoKM_HTest()  //海里速度（nm/h）
        {
            double kt = 1;
            double kmh = UniteTranslateTool.KttoKM_H(kt);
            Assert.AreEqual(kmh, kt * 1.852);
        }

        [TestMethod()]
        public void KM_HtoKtTest() //海里速度（nm/h）
        {
            double kmh = 1;
            double kt = UniteTranslateTool.KM_HtoKt(kmh);
            Assert.AreEqual(kt, kmh / 1.852);
        }

        [TestMethod()]
        public void M_StoKtTest() //海里速度（nm/h）
        {
            double ms = 1;
            double kt = UniteTranslateTool.M_StoKt(ms);
            Assert.AreEqual(Math.Round(kt, 4), Math.Round(ms / 1852 * 3600, 4));
        }

        [TestMethod()]
        public void KttoM_STest()
        {
            double kt = 1;
            double ms = UniteTranslateTool.KttoM_S(kt);
            Assert.AreEqual(Math.Round(ms, 4), Math.Round(kt * 1852 / 3600, 4));
        }

        [TestMethod()]
        public void KHzToMHzTest()  //千赫和兆赫
        {
            double kh = 1;
            double mh = UniteTranslateTool.KHzToMHz(kh);
            Assert.AreEqual(mh, kh / 1000);
        }

        [TestMethod()]
        public void MHzToKHzTest()  //千赫和兆赫
        {
            double mh = 1;
            double kh = UniteTranslateTool.MHzToKHz(mh);
            Assert.AreEqual(kh, mh * 1000);
        }
    }
}
