using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pavlo.MyHelpers.MyMath.Statistics
{
    /// <summary>
    /// Calculate some statistics serieses and for each series 
    /// </summary>
    public class Serieses
    {
        protected double[][] seriesesData;
        
        private double[] _magnitudes;
        /// <summary>
        /// Magnitude for each series
        /// </summary>
        public double[] Magnitudes
        {
            get { return _magnitudes; }
            protected set { _magnitudes = value; }
        }

        private int[] _IndexesOfMagnitudesSortedInAscendingOrder;
        /// <summary>
        /// array with indexes of "Magnitudes"-array sorted in ascending order
        /// </summary>
        public int[] IndexesOfMagnitudesSortedInAscendingOrder
        {
            get { return _IndexesOfMagnitudesSortedInAscendingOrder; }
            protected set { _IndexesOfMagnitudesSortedInAscendingOrder = value; }
        }

        
        private bool _IsAllMagnitudesHaveSameSign=false;
        /// <summary>
        /// is magnitudes all positive or all negative
        /// </summary>
        public bool IsAllMagnitudesHaveSameSign
        {
            get { return _IsAllMagnitudesHaveSameSign; }
            protected set { _IsAllMagnitudesHaveSameSign = value; }
        }

        private double _MeanMagnitude;
        /// <summary>
        /// mean magnitude across magnitudes of all serieses
        /// </summary>
        public double MeanMagnitude
        {
            get { return _MeanMagnitude; }
            protected set { _MeanMagnitude = value; }
        }

        private double _sd;
        /// <summary>
        /// standard deviation of magnitude across magnitudes all serieses
        /// </summary>
        public double SD
        {
            get { return _sd; }
            protected set { _sd= value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seriesesData">serieses to process</param>
        public Serieses(double[][] seriesesData)
        {
            this.seriesesData = seriesesData;
            CalculateMagnitudes();
            this.MeanMagnitude = this.Magnitudes.Sum() / this.Magnitudes.Length;
            this.SD = MathNet.Numerics.Statistics.Statistics.StandardDeviation(this.Magnitudes);
            CalculateAscendingOrderOfMagnitudes();
        }

        private void CalculateMagnitudes()
        {
            //arrays with values for each series
            double[] maxValue = new double[seriesesData.Length];
            double[] minValue = new double[seriesesData.Length];

            double[] magnitude = new double[seriesesData.Length];
            for (int i = 0; i < seriesesData.Length; i++)
            {
                maxValue[i] = seriesesData[i].Max<double>();
                minValue[i] = Math.Abs(seriesesData[i].Min<double>());
                magnitude[i] = maxValue[i] > minValue[i] ? maxValue[i] : minValue[i];
            }

            this.Magnitudes = magnitude;

            bool isAllMagnitudesPositive = magnitude.SequenceEqual<double>(maxValue);
            bool isAllMagnitudesNegative = magnitude.SequenceEqual<double>(minValue);

            if (isAllMagnitudesNegative || isAllMagnitudesPositive)
                this.IsAllMagnitudesHaveSameSign = true;
            else
                this.IsAllMagnitudesHaveSameSign = false;
        }

        private void CalculateAscendingOrderOfMagnitudes()
        {
            this.IndexesOfMagnitudesSortedInAscendingOrder = this.Magnitudes.Select((x, i) => new KeyValuePair<double, int>(x, i))
                .OrderBy(x => x.Key).ToList()
                .Select(x => x.Value).ToArray();
        }
    }
}
