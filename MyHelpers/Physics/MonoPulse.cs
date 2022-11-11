using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pavlo.MyHelpers.Physics
{
    /// <summary>
    /// Calculate of the monopulse characteristics. The time must be ascending sorted equidistant array. The data between samples are interpolated!
    /// </summary>
    public class MonoPulse
    {
        private double[] _time;
        /// <summary>
        /// pulse time domain. 
        /// </summary>
        public double[] time
        {
            get { return _time; }
            protected set { _time = value; }
        }

        private double[] _YValue;
        /// <summary>
        /// Y axis values of the pulse
        /// </summary>
        public double[] YValue
        {
            get { return _YValue; }
            protected set { _YValue = value; }
        }

        /// <summary>
        /// store original input YValues in case of "negative pulse"
        /// </summary>
        private double[] YValueOriginal;

        private double _Amplitude;
        /// <summary>
        /// amplitude of YValues
        /// </summary>
        public double Amplitude
        {
            get { return _Amplitude; }
            protected set { _Amplitude = value; }
        }

        private double _AmplitudeTime;
        /// <summary>
        /// Time corresponding to amplitude of YValues
        /// </summary>
        public double AmplitudeTime
        {
            get { return _AmplitudeTime; }
            protected set { _AmplitudeTime = value; }
        }

        private double _d1Amplitude;
        /// <summary>
        /// 0.1*amplitude of YValues
        /// </summary>
        public double d1Amplitude
        {
            get { return _d1Amplitude; }
            protected set { _d1Amplitude = value; }
        }

        private double _d1AmplitudeTime;
        /// <summary>
        /// Time corresponding to d1Amplitude
        /// </summary>
        public double d1AmplitudeTime
        {
            get { return _d1AmplitudeTime; }
            protected set { _d1AmplitudeTime = value; }
        }

        private double _d9Amplitude;
        /// <summary>
        /// 0.9*amplitude of YValues
        /// </summary>
        public double d9Amplitude
        {
            get { return _d9Amplitude; }
            protected set { _d9Amplitude = value; }
        }

        private double _d9AmplitudeTime;
        /// <summary>
        /// Time corresponding to d9Amplitude
        /// </summary>
        public double d9AmplitudeTime
        {
            get { return _d9AmplitudeTime; }
            protected set { _d9AmplitudeTime = value; }
        }

        private double _HalfAmplitude;
        /// <summary>
        /// 0.5*amplitude of YValues
        /// </summary>
        public double HalfAmplitude
        {
            get { return _HalfAmplitude; }
            protected set { _HalfAmplitude = value; }
        }

        private double _HalfAmplitudeTime1;
        /// <summary>
        /// Time corresponding to "left" HalfAmplitude
        /// </summary>
        public double HalfAmplitudeTime1
        {
            get { return _HalfAmplitudeTime1; }
            protected set { _HalfAmplitudeTime1 = value; }
        }

        private double _HalfAmplitudeTime2;
        /// <summary>
        /// Time corresponding to "right" HalfAmplitude
        /// </summary>
        public double HalfAmplitudeTime2
        {
            get { return _HalfAmplitudeTime2; }
            protected set { _HalfAmplitudeTime2 = value; }
        }

        /// <summary>
        ///  the time between d1AmplitudeTime and d9AmplitudeTime
        /// </summary>
        public double RiseTime
        {
            get { return this.d9AmplitudeTime - this.d1AmplitudeTime; }
        }

        /// <summary>
        /// the time between HalfAmplitudeTime2 and  HalfAmplitudeTime1
        /// </summary>
        public double FWHM
        {
            get { return this.HalfAmplitudeTime2-this.HalfAmplitudeTime1; }
        }

        /// <summary>
        /// is amplitude of the pulse is positive
        /// </summary>
        private bool isPulsePositive;

        /// <summary>
        /// is pulse an appropriate monopulse: there are d1Amplitude, "left" HalfAmplitude, d9Amplitude, Amplitude and "right" HalfAmplitude values in the pulse.
        /// </summary>
        public bool IsPulseInappropriate = false;

        /// <param name="time">ascending sorted equidistant array of times</param>
        /// <param name="yValue">Y axis values of the pulse</param>
        public MonoPulse(double[] time, double[] yValue)
        {
            this.time = time;
            this.YValue = yValue;

            CalcParameters();
        }


        private void CalcParameters()
        {
            Amplitude = YValue.Max<Double>();
            double minYValue = Math.Abs(YValue.Min<Double>());

            if (Amplitude >= minYValue)
                this.isPulsePositive = true;
            else /*if pulse is negative, then invert it*/
            {
                this.isPulsePositive = false;
                this.Amplitude = minYValue;
                YValueOriginal = YValue;
                YValue = new double[YValueOriginal.Length];
                for (int i = 0; i < YValue.Length; i++)
                {
                    YValue[i] = -1*YValueOriginal[i];
                }
            }
            d9Amplitude = 0.9 * Amplitude;
            d1Amplitude = 0.1 * Amplitude;
            HalfAmplitude = 0.5 * Amplitude;

            int maxYValueIndex = Array.IndexOf<double>(YValue, Amplitude);
            this.AmplitudeTime = time[maxYValueIndex];
            
            int zeroIndex = MyMath.Arrays.SearchPositionToTheLeftFirstLower(YValue, 0.0, maxYValueIndex);
            zeroIndex = zeroIndex == -1 ? 0 : zeroIndex;

            int d1maxYValueIndex = MyMath.Arrays.SearchPositionToTheRightFirstHigher(YValue, d1Amplitude, zeroIndex);
            if (d1maxYValueIndex == -1)
            {
                this.IsPulseInappropriate = true;
                return;
            }

            int halfMaxYValueIndex1 = MyMath.Arrays.SearchPositionToTheRightFirstHigher(YValue, HalfAmplitude, d1maxYValueIndex);
            if (halfMaxYValueIndex1 == -1)
            {
                this.IsPulseInappropriate = true;
                return;
            }

            int d9maxYValueIndex = MyMath.Arrays.SearchPositionToTheRightFirstHigher(YValue, d9Amplitude, halfMaxYValueIndex1);
            if (d9maxYValueIndex == -1)
            {
                this.IsPulseInappropriate = true;
                return;
            }

            int halfMaxYValueIndex2 = -1;
            if (YValue[YValue.Length - 1] == HalfAmplitude)
            {
                halfMaxYValueIndex2 = YValue.Length - 1;
            }
            else if (YValue[YValue.Length - 1] < HalfAmplitude)
            {
                halfMaxYValueIndex2 = MyMath.Arrays.SearchPositionToTheLeftFirstHigher(YValue, HalfAmplitude, YValue.Length-1);
            }
            if (halfMaxYValueIndex2 == -1)
            {
                this.IsPulseInappropriate = true;
                return;
            }

            d1AmplitudeTime = MyMath.Common.GetXInterpolatingValue(time[d1maxYValueIndex - 1], YValue[d1maxYValueIndex - 1], time[d1maxYValueIndex], YValue[d1maxYValueIndex], d1Amplitude);
            HalfAmplitudeTime1 = MyMath.Common.GetXInterpolatingValue(time[halfMaxYValueIndex1 - 1], YValue[halfMaxYValueIndex1 - 1], time[halfMaxYValueIndex1], YValue[halfMaxYValueIndex1], HalfAmplitude);
            d9AmplitudeTime = MyMath.Common.GetXInterpolatingValue(time[d9maxYValueIndex - 1], YValue[d9maxYValueIndex - 1], time[d9maxYValueIndex], YValue[d9maxYValueIndex], d9Amplitude);
            HalfAmplitudeTime2 = MyMath.Common.GetXInterpolatingValue(time[halfMaxYValueIndex2], YValue[halfMaxYValueIndex2], time[halfMaxYValueIndex2 + 1], YValue[halfMaxYValueIndex2 + 1], HalfAmplitude);

            //invert variables in case of negative pulse
            if (!isPulsePositive)
            {
                YValue = YValueOriginal;
                this.Amplitude *= -1;
                this.d1Amplitude *= -1;
                this.d9Amplitude *= -1;
                this.HalfAmplitude *= -1;
            }
        }
    }
}
