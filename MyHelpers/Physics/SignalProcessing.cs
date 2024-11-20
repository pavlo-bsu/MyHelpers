using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pavlo.MyHelpers.Physics
{
    public static class SignalProcessing
    {
        /// <summary>
        /// Cut an constant offset from "signal". The offset is counted as an mean value of the signal from the beginning of the signal to the tFinish
        /// </summary>
        /// <param name="signal">signal</param>
        /// <param name="times">array of times of the signal</param>
        /// <param name="tFinish">end time for offset counting </param>
        public static void CutSignalOffsetAsMean(double[] signal, double[] times, double tFinish)
        {
            int indextFinish = MyMath.Arrays.GetIndexByValueInEquidistantArray(times, tFinish);
            if (indextFinish == -1)
                throw new Exception("Mismatch of the end time and times array!");
            double sum = 0;
            for (int i = 0; i <= indextFinish; i++)
                sum += signal[i];
            double offset = sum / (indextFinish + 1);
            for (int i = 0; i < signal.Length; i++)
            {
                signal[i] -= offset;
            }
        }

        /// <summary>
        /// Cut an constant offset from "signal". The offset is the value of the signal at the time = theTime
        /// </summary>
        /// <param name="signal">signal</param>
        /// <param name="times">array of times of the signal</param>
        /// <param name="theTime">Time, the signal value in which will be taken as the offset</param>
        public static void CutSignalOffsetAtTheTime(double[] signal, double[] times, double theTime)
        {
            int indexTheTime = MyMath.Arrays.GetIndexByValueInEquidistantArray(times, theTime);
            if (indexTheTime == -1)
                throw new Exception("Mismatch of the end time and times array!");
            double offset = signal[indexTheTime];
            for (int i = 0; i < signal.Length; i++)
            {
                signal[i] -= offset;
            }
        }

        /// <summary>
        /// Cut the segment from tStart to tFinish from the time domain of the signal
        /// </summary>
        /// <param name="signal">original signal</param>
        /// <param name="times">original times</param>
        /// <param name="tStart">initial time of the cutted-off times</param>
        /// <param name="tFinish">final time of the cutted-off times</param>
        /// <param name="cuttedSignal">cutted-off signal</param>
        /// <param name="cuttedTimes">cutted-off times</param>
        public static void CutOffSignalTimeDomain(double[] signal, double[] times, double tStart, double tFinish, out double[] cuttedSignal, out double[] cuttedTimes)
        {
            int indexStart = MyMath.Arrays.GetIndexByValueInEquidistantArray(times, tStart);
            int indexFinish = MyMath.Arrays.GetIndexByValueInEquidistantArray(times, tFinish);

            if (indexStart >= indexFinish||indexStart<0||indexFinish<0)
                throw new Exception("Mismatch at least of one of the input times!");

            int newSamplesCount = indexFinish - indexStart + 1;
            cuttedTimes = new double[newSamplesCount];
            Array.Copy(times, indexStart, cuttedTimes, 0, newSamplesCount);

            cuttedSignal = new double[newSamplesCount];
            Array.Copy(signal, indexStart, cuttedSignal, 0, newSamplesCount);
        }

        /// <summary>
        /// add zeros to signal (is used to improve the resolution of the DFT)
        /// </summary>
        /// <param name="signal">original signal</param>
        /// <param name="times">original times</param>
        /// <param name="numberOfZeros">number of zeros</param>
        /// <param name="isAddBefore">is zeros add before the each signal</param>
        /// <param name="newSignal">new signal with zeros</param>
        /// <param name="newTimes">new time</param>
        public static void ZeroPadding (double[] signal, double[] times, int numberOfZeros, bool isAddBefore, out double[] newSignal, out double[] newTimes)
        {
            newSignal = new double[signal.Length + numberOfZeros];
            newTimes = new double[times.Length + numberOfZeros];

            //fill the time
            double dt = times[1] - times[0];
            double t0;
            if (isAddBefore)
                t0 = times[0] - numberOfZeros * dt;
            else
                t0 = times[0];

            for (int i = 0; i < newTimes.Length; i++)
                newTimes[i] = t0 + i * dt;

            //add zeros to the signals
            if (isAddBefore)
            {
                for (int i = 0; i < numberOfZeros; i++)
                    newSignal[i] = 0d;
                for (int i = 0; i < signal.Length; i++)
                    newSignal[i + numberOfZeros] = signal[i];
            }
            else
            {
                for (int i = 0; i < signal.Length; i++)
                    newSignal[i] = signal[i];
                for (int i = signal.Length; i < newSignal.Length; i++)
                    newSignal[i] = 0d;
            }
        }

        /// <summary>
        /// multuply each value of a signal
        /// </summary>
        /// <param name="signal">original signal</param>
        /// <param name="theFactor">factor of multiplication</param>
        /// <param name="newSignal">multiplied signal</param>
        public static void Multiply(double[] signal, double theFactor, out double[] newSignal)
        {
            newSignal = new double[signal.Length];

            for (int i = 0; i < signal.Length; i++)
                newSignal[i] = signal[i]*theFactor;
        }
    }
}
