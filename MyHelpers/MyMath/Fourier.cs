using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Pavlo.MyHelpers.MyMath
{
    public static class Fourier
    {
        /// <summary>
        /// Calculate Forward FFT of the "array" and return it by the same variable "array"! Need for MathNet.Numerics.
        /// </summary>
        /// <param name="array">original array (input) and Forward FFT (output)</param>
        public static void FFTBluesteinForward(Complex[] array)
        {
            MathNet.Numerics.IntegralTransforms.Fourier.BluesteinForward(array, MathNet.Numerics.IntegralTransforms.FourierOptions.Default);
        }

        /// <summary>
        /// Calculate Inverse FFT of the "array" and return it by the same variable "array"! Need for MathNet.Numerics.
        /// </summary>
        /// <param name="array">original array (input) and Inverse FFT (output)</param>
        public static void FFTBluesteinInverse(Complex[] array)
        {
            MathNet.Numerics.IntegralTransforms.Fourier.BluesteinInverse(array, MathNet.Numerics.IntegralTransforms.FourierOptions.Default);
        }
    }
}
