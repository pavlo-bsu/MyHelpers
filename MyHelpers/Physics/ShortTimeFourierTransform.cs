using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Pavlo.MyHelpers.Physics
{
    /// <summary>
    /// represents result of ShortTimeFourierTransform. Use Hamming window
    /// </summary>
    public class ShortTimeFourierTransform
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="signalTime">origin signal time values</param>
        /// <param name="signalValue">origin signal Y-axis values</param>
        /// <param name="timeShift">shift for time segments. Should be multiple of signal dt, otherwise it'll be automatically floored (Math.Floor)</param>
        /// <param name="timeWindowWidth">length of each time segment for STFT. Should be multiple of signal dt, otherwise it'll be automatically floored (Math.Floor)</param>
        public ShortTimeFourierTransform(double[] signalTime, double[] signalValue, double timeShift, double timeWindowWidth)
        {
            SignalTime = signalTime;
            SignalValue = signalValue;
            TimeShift = timeShift;
            TimeWindowWidth = timeWindowWidth;
        }

        /// <summary>
        /// stft time values
        /// </summary>
        public double[] Time
        { get; private set; }

        /// <summary>
        /// stft frequency values
        /// </summary>
        public double[] Frequency
        { get; private  set; }

        /// <summary>
        /// stft values. First dimension - Time, second - Frequency
        /// </summary>
        public double[,] Amplitude
        { get; private set; }

        /// <summary>
        /// origin signal time values
        /// </summary>
        public double[] SignalTime
        {
            get; private set;
        }

        /// <summary>
        /// origin signal Y-axis values
        /// </summary>
        public double[] SignalValue
        {
            get; private set;
        }

        /// <summary>
        /// time increment
        /// </summary>
        public double dt
        {
            get => SignalTime[1] - SignalTime[0];
        }

        private double _TimeShift;
        /// <summary>
        /// shift for time segments. Should be multiple of signal dt, otherwise it'll be automatically floored (Math.Floor)
        /// </summary>
        public double TimeShift
        {
            get => _TimeShift;
            private set
            {
                timeShiftInSamples = Convert.ToInt32(Math.Floor(value / dt));
                _TimeShift = dt * timeShiftInSamples;
            }
        }

        /// <summary>
        /// time shift in samples (not in seconds)
        /// </summary>
        private int timeShiftInSamples;

        private double _TimeWindowWidth;
        /// <summary>
        /// length of each time segment for STFT. Should be multiple of signal dt, otherwise it'll be automatically floored (Math.Floor)
        /// </summary>
        public double TimeWindowWidth
        {
            get => _TimeWindowWidth;
            private set
            {
                timeWindowWidthInSamples = Convert.ToInt32(Math.Floor(value / dt));
                _TimeWindowWidth = dt * timeWindowWidthInSamples;
            }
        }

        /// <summary>
        /// length of each time segment for STFT in samples (not in seconds)
        /// </summary>
        private int timeWindowWidthInSamples;

        /// <summary>
        /// Calculate Time, Frequency and Amplitude 
        /// </summary>
        public void CalculateSTFT()
        {
            //calc intervalsCount
            double totalTimeInterval = SignalTime[SignalTime.Length - 1] - SignalTime[0];
            int shiftsInInterval = Convert.ToInt32(Math.Floor(TimeWindowWidth / TimeShift));
            int intervalsCount = Convert.ToInt32(Math.Floor(totalTimeInterval / TimeShift));
            intervalsCount -= shiftsInInterval + 1;

            //set times of stft
            Time = new double[intervalsCount];
            for (int i = 0; i < intervalsCount; i++)
            {
                Time[i] = SignalTime[0] + i * TimeShift;
            }


            //set frequencies of stft
            Frequency = Pavlo.MyHelpers.Physics.Fourier.CalcFrequenciesIntensionalPart(dt, timeWindowWidthInSamples);

            //calculate Amplitude
            Amplitude = new double[Time.Length, Frequency.Length];
            Complex[][] timeSegmentsValues = new Complex[intervalsCount][];
            for (int j = 0; j < intervalsCount; j++)
            {

                //get time domain for each time segment
                timeSegmentsValues[j] = new Complex[timeWindowWidthInSamples];
                var window = MathNet.Numerics.Window.Hamming(timeWindowWidthInSamples);
                for (int k = 0; k < timeSegmentsValues[j].Length; k++)
                {
                    timeSegmentsValues[j][k] = new Complex(SignalValue[j * timeShiftInSamples + k]*window[k], 0.0);
                }

                Pavlo.MyHelpers.MyMath.Fourier.FFTBluesteinForward(timeSegmentsValues[j]);
                var ampl = Pavlo.MyHelpers.Physics.Fourier.GetAmplitudeOfIntensionalAFC(timeSegmentsValues[j], false);
                for (int k = 0; k < ampl.Length; k++)
                {
                    Amplitude[j, k] = ampl[k];
                }
            }
        }

        /// <summary>
        /// normalize stft to 1
        /// </summary>
        public void NormalizeSTFT()
        {
            double max = Amplitude[0, 0];
            foreach (double d in Amplitude)
            {
                if (d > max)
                    max = d;
            }
            for (int j = 0; j < Amplitude.GetLength(0); j++)
                for (int k = 0; k < Amplitude.GetLength(1); k++)
                    Amplitude[j, k] /= max;
        }

        /// <summary>
        /// normalize to 1 stft values of each time segment
        /// </summary>
        public void NormalizeEachSegmentOfSTFT()
        {
            for (int j = 0; j < Amplitude.GetLength(0); j++)
            {
                double max = Amplitude[j, 0];
                for (int k = 0; k < Amplitude.GetLength(1); k++)
                {
                    if (max < Amplitude[j, k])
                        max = Amplitude[j, k];
                }
                for (int k = 0; k < Amplitude.GetLength(1); k++)
                {
                    Amplitude[j, k] /= max;
                }
            }
        }
    }
}
