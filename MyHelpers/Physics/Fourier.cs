using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Pavlo.MyHelpers.Physics
{
    public static class Fourier
    {
        /// <summary>
        /// Calculate only intensional part of frequencies (from 0[Hz] to Nyquist frequency)
        /// </summary>
        /// <param name="dt">time increment</param>
        /// <param name="samplesCount">times amoun</param>
        /// <returns>intensional part of frequencies (from 0[Hz] to Nyquist frequency)</returns>
        public static double[] CalcFrequenciesIntensionalPart(double dt, int samplesCount)
        {

            double fMax = 1.0 / dt;
            double df = fMax / samplesCount;
            int indexNyquist = GetIndexNyquist(samplesCount);
            double[] frequencies = new double[indexNyquist + 1];
            for (int i = 0; i < frequencies.Length; i++)
            {
                frequencies[i] = i * df;
            }
            return frequencies;
        }

        /// <summary>
        /// get index of the Nyquist frequency in the array with forward FFT of the signal
        /// </summary>
        /// <param name="samplesCount">signal length</param>
        /// <returns></returns>
        public static int GetIndexNyquist(int signalLength)
        {
            return signalLength / 2;
        }

        /// <summary>
        /// Get amplitudes of frequencies from 0 Hz to NyquistFrequency
        /// </summary>
        /// <param name="totalAFC">total AFC of the signal (i.e. result of the forward FFT)</param>
        /// <param name="normalizeAFC">Should amplitudes be normalized to 1</param>
        /// <returns></returns>
        public static double[] GetAmplitudeOfIntensionalAFC(Complex[] totalAFC, bool normalizeAFC)
        {
            double[] normAFC = null;
            int indexNyquist = GetIndexNyquist(totalAFC.Length);

            normAFC = new double[indexNyquist + 1];
            for (int i = 0; i < normAFC.Length; i++)
            {
                normAFC[i] = totalAFC[i].Magnitude;
            }
            
            //set true value for frequency 0 Hz (0 index of the signal)
            normAFC[0] /= 2;
            //normalization to 1
            if (normalizeAFC)
            {
                double maxVal = normAFC.Max();
                for (int i = 0; i < normAFC.Length; i++)
                {
                    normAFC[i] /= maxVal;
                }
            }

            return normAFC;
        }

        /// <summary>
        /// Get amplitudes of frequencies from 0 Hz to NyquistFrequency. And normalize amplitudes to 1 for each signal.
        /// </summary>
        /// <param name="totalAFC">total AFC of the signals (i.e. results of the forward FFT)</param>
        /// <param name="normalizeAFC">Should amplitudes be normalized to 1</param>
        /// <returns></returns>
        public static double[][] GetAmplitudeOfIntensionalAFC2D(Complex[][] totalAFC, bool normalizeAFC)
        {
            double[][] normAFC = new double[totalAFC.Length][];

            for (int i = 0; i < normAFC.Length; i++)
            {
                normAFC[i] = GetAmplitudeOfIntensionalAFC(totalAFC[i], normalizeAFC);
            }
            
            return normAFC;
        }

        /// <summary>
        /// Cut frequencies higher "highestFr"
        /// </summary>
        /// <param name="afc">total AFC of the signal (i.e. result of the forward FFT)</param>
        /// <param name="intensionalFrequencies">intensional part of frequencies (from 0[Hz] to Nyquist frequency)</param>
        /// <param name="highestFr">highest frequency</param>
        public static void LowpassFilter(Complex[] afc, double[] intensionalFrequencies, double highestFr)
        {
            if (highestFr <= intensionalFrequencies[intensionalFrequencies.Length - 1] && highestFr >= 0)
            {
                int highestFrIndex = MyMath.Arrays.GetIndexByValueInEquidistantArray(intensionalFrequencies, highestFr);
                if (highestFrIndex == 0)//solve issue with afc[afc.Length - i] = 0; below
                {
                    afc[0] = 0;
                    highestFrIndex++;
                }
                for (int i = highestFrIndex; i < intensionalFrequencies.Length; i++)
                {
                    afc[i] = 0;
                    afc[afc.Length - i] = 0;
                }
            }
        }

        /// <summary>
        /// Cut frequencies lower "lowestFr"
        /// </summary>
        /// <param name="afc">total AFC of the signal (i.e. result of the forward FFT)</param>
        /// <param name="intensionalFrequencies">intensional part of frequencies (from 0[Hz] to Nyquist frequency)</param>
        /// <param name="lowestFr">lowest frequency</param>
        public static void HighpassFilter(Complex[] afc, double[] intensionalFrequencies, double lowestFr)
        {
            if (lowestFr >= 0 && lowestFr<=intensionalFrequencies[intensionalFrequencies.Length-1])
            {
                int lowFrIndex = Pavlo.MyHelpers.MyMath.Arrays.GetIndexByValueInEquidistantArray(intensionalFrequencies, lowestFr);
                afc[0] = 0;
                for (int i = 1; i <= lowFrIndex; i++)
                {
                    afc[i] = 0;
                    afc[afc.Length - i] = 0;
                }
            }
        }

        /// <summary>
        /// Cut frequencies between "lowFR" and "highFr"
        /// </summary>
        /// <param name="afc">total AFC of the signal (i.e. result of the forward FFT)</param>
        /// <param name="intensionalFrequencies">intensional part of frequencies (from 0[Hz] to Nyquist frequency)</param>
        /// <param name="lowFr">lower frequency</param>
        /// <param name="highFr">higher frequency</param>
        public static void BandstopFilter(Complex[] afc, double[] intensionalFrequencies, double lowFr, double highFr)
        {
            if (lowFr > highFr ||
                lowFr < 0 || lowFr > intensionalFrequencies[intensionalFrequencies.Length - 1]||
                highFr < 0 || highFr > intensionalFrequencies[intensionalFrequencies.Length - 1])
                throw new Exception("Mismatch of the input frequencies!");

            int lowFrIndex = MyMath.Arrays.GetIndexByValueInEquidistantArray(intensionalFrequencies, lowFr);
            int highFrIndex = MyMath.Arrays.GetIndexByValueInEquidistantArray(intensionalFrequencies, highFr);
            if (lowFrIndex == 0)//to solve the issue with afc[afc.Length - i] = 0; below
            {
                afc[0] = 0;
                lowFrIndex++;
            }
            for (int i = lowFrIndex; i <= highFrIndex; i++)
            {
                afc[i] = 0;
                afc[afc.Length - i] = 0;
            }
        }
    }
}
