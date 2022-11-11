using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pavlo.MyHelpers.Physics
{
    /// <summary>
    /// Calculate of the characteristics of the spectrum(DFT). The frequencies must be ascending sorted equidistant array. The data between samples are interpolated!
    /// </summary>
    public class DFTCharacteristics
    {
        private double[] _frequencies;
        /// <summary>
        /// pulse fr. domain. 
        /// </summary>
        public double[] frequencies
        {
            get { return _frequencies; }
            protected set { _frequencies = value; }
        }

        private double[] _YValue;
        /// <summary>
        /// Y axis values of the pulse (i.e. spectrum values)
        /// </summary>
        public double[] YValue
        {
            get { return _YValue; }
            protected set { _YValue = value; }
        }

        private double _Amplitude;
        /// <summary>
        /// amplitude of YValues
        /// </summary>
        public double Amplitude
        {
            get { return _Amplitude; }
            protected set { _Amplitude = value; }
        }

        private double _AmplitudeFrequency;
        /// <summary>
        /// Frequency corresponding to amplitude of YValues
        /// </summary>
        public double AmplitudeFrequency
        {
            get { return _AmplitudeFrequency; }
            protected set { _AmplitudeFrequency = value; }
        }

        private double _Value3dbAmplitude;
        /// <summary>
        /// 3 db decrease of the amplitude of YValues
        /// </summary>
        public double Value3dbAmplitude
        {
            get { return _Value3dbAmplitude; }
            protected set { _Value3dbAmplitude = value; }
        }

        private double _LeftBorder3db;
        /// <summary>
        /// Frequency corresponding to the left border of 3db band
        /// </summary>
        public double LeftBorder3db
        {
            get { return _LeftBorder3db; }
            protected set { _LeftBorder3db = value; }
        }

        private double _RightBorder3db;
        /// <summary>
        /// Frequency corresponding to the right border of 3db band
        /// </summary>
        public double RightBorder3db
        {
            get { return _RightBorder3db; }
            protected set { _RightBorder3db = value; }
        }

        /// <summary>
        /// 3db bandwidth
        /// </summary>
        public double Bandwidth3db
        {
            get { return this.RightBorder3db-this.LeftBorder3db; }
        }

        /// <param name="frequency">ascending sorted equidistant array of frequencies</param>
        /// <param name="yValue"> frequency amplitude values of the pulse</param>
        public DFTCharacteristics(double[] frequency, double[] yValue)
        {
            this.frequencies = frequency;
            this.YValue = yValue;

            CalcCharacteristics();
        }


        private void CalcCharacteristics()
        {
            Amplitude = YValue.Max<Double>();

            Value3dbAmplitude = Amplitude*MyMath.Common.dBToVoltageRatio(-3);

            int maxYValueIndex = Array.IndexOf<double>(YValue, Amplitude);
            this.AmplitudeFrequency = frequencies[maxYValueIndex];

            

            int leftBorder3dbIndex = MyMath.Arrays.SearchPositionToTheLeftFirstLower(YValue, Value3dbAmplitude, maxYValueIndex);
            if (leftBorder3dbIndex == -1)
            {
                LeftBorder3db = frequencies[0];
            }
            else
            {
                LeftBorder3db = MyMath.Common.GetXInterpolatingValue(frequencies[leftBorder3dbIndex], YValue[leftBorder3dbIndex], frequencies[leftBorder3dbIndex+1], YValue[leftBorder3dbIndex+1], Value3dbAmplitude);
            }

            int rigthBorder3dbIndex = MyMath.Arrays.SearchPositionToTheRightFirstLower(YValue, Value3dbAmplitude, maxYValueIndex);
            if (rigthBorder3dbIndex == -1)
            {
                RightBorder3db = frequencies[YValue.Length - 1];
            }
            else
            {
                RightBorder3db = MyMath.Common.GetXInterpolatingValue(frequencies[rigthBorder3dbIndex-1], YValue[rigthBorder3dbIndex-1], frequencies[rigthBorder3dbIndex], YValue[rigthBorder3dbIndex], Value3dbAmplitude);
            }
        }
    }
}
