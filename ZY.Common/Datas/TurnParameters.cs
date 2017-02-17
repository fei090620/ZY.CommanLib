using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZY.Common.Datas
{
    public class TurnParameters
    {
        public TurnParameters(double ias, double altitude, double bankangle, double windspeed, double isa)
        {
            this._ias = ias;
            this._altitude = altitude;
            this._bankangle = bankangle;
            this._windspeed = windspeed;
            this._isa = isa;
        }

        //表速
        private double _ias;

        //海拔
        private double _altitude;

        //转弯坡度
        private double _bankangle;

        //风速
        private double _windspeed;

        //国际标准大气
        private double _isa;

        #region Output
        public double TAS  //真空速
        {
            get
            {
                double _tas = _ias * this.K;// unit: km/h
                return _tas;
            }
        }

        public double K
        {
            get
            {
                double _k = 171233 * Math.Sqrt(((288 + _isa) - 0.006496 * _altitude)) / Math.Pow((288 - 0.006496 * _altitude), 2.628);
                return _k;
            }
        }

        public double BankRate
        {
            get
            {
                double _bankRate = Math.Min(6355 * Math.Tan(_bankangle * Math.PI / 180) / (Math.PI * TAS), 3);
                return _bankRate;
            }
        }

        public double Radius
        {
            get
            {
                double _radius = 1000 * this.TAS / (20 * Math.PI * this.BankRate);// unit: m
                return _radius;
            }
        }

        public double Esita
        {
            get
            {
                double _esita = 1000 * _windspeed / (20 * Math.PI * this.BankRate);// unit: m/°  in RAD
                return _esita;
            }
        }

        public double DraftAngle  //偏流角
        {
            get
            {
                double _draftangle = Math.Asin(_windspeed / TAS);
                return _draftangle;
            }
        }
        #endregion

        #region Method
        //输出参数字符串
        public string GetParametersString()
        {
            string s = "=====Turn Parameters=====\n";
            s += "IAS = " + Math.Round(_ias * 100) / 100 + " km/h\n";
            s += "k = " + Math.Round(K * 10000) / 10000 + "\n";
            s += "TAS = " + Math.Round(TAS * 100) / 100 + " km/h\n";
            s += "BankAngle =" + _bankangle + "degree\n";
            s += "DraftAngle = " + Math.Round(DraftAngle * 180 / Math.PI * 100) / 100 + "degree\n";
            s += "R = " + Math.Round(BankRate * 100) / 100 + " degree/s\n";
            s += "Radius = " + Math.Round(Radius) + " m\n";
            s += "Esita = " + Math.Round(10 * Esita * Math.PI / 180) / 10 + " m /degree\n";
            s += "E45 = " + Math.Round(45 * Math.PI / 180 * Esita) + " m \n";
            s += "E90 = " + Math.Round(90 * Math.PI / 180 * Esita) + " m \n";
            s += "E135 = " + Math.Round(135 * Math.PI / 180 * Esita) + " m \n";
            s += "E180 = " + Math.Round(180 * Math.PI / 180 * Esita) + " m \n";
            s += "E235 = " + Math.Round(235 * Math.PI / 180 * Esita) + " m \n";
            return s;
        }
        #endregion
    }
}
