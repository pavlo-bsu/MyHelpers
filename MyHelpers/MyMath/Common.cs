using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pavlo.MyHelpers.MyMath
{    
    public delegate double SomeFunction(double x);
    public static class Common
    {
        /// <summary>
        /// Root-finding of "func" by bisection method
        /// </summary>
        /// <param name="func">function of one variable</param>
        /// <param name="x_start">begin of segment with root</param>
        /// <param name="x_finish">end of segment with root</param>
        /// <param name="root_eps">accuracy of root-finding</param>
        /// <param name="iMax">max value of stemps during search</param>
        /// <returns>root</returns>
        public static double SolutionByTheBisectionMethod(SomeFunction func, double x_start, double x_finish, double root_eps,int iMax)
        {
            //checks
            if ((x_finish <= x_start) || (root_eps <= 0) || (func(x_finish)*func(x_start)>0))
                throw new Exception("Wrong Input Parameters at SolutionByTheBisectionMethod");
            double x_middle = 0;

            int i = 0; //steps count
            while ((x_finish - x_start) >= root_eps)
            {
                x_middle = (x_start + x_finish) / 2;
                if (func(x_start) * func(x_middle) < 0)
                {
                    x_finish = x_middle;
                }
                else
                {
                    x_start = x_middle;
                }
                if (++i > iMax)
                {
                    throw new Exception("Steps count exceed max value!");
                }
            }
            double root = (x_start + x_finish) / 2;
            return root;
        }
        /// <summary>
        /// convert degree to radian
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static double DegreeToRadian(double degree)
        {
            return Math.PI * degree / 180;
        }
        /// <summary>
        /// convert radian to degree
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static double RadianToDegree(double radian)
        {
            return radian * 180 / Math.PI;
        }
        
        /// <summary>
        /// converting dB to the ratio of field quantity(e.g. voltages), not of power quantity!
        /// </summary>
        /// <param name="dB">dB value</param>
        /// <returns>ratio value</returns>
        public static double dBToVoltageRatio(double dB)
        {
            return Math.Pow(10, dB / 20.0);
        }

        /// <summary>
        /// converting dB to the power ratio, not ratio of field quantity(e.g. voltages)!
        /// </summary>
        /// <param name="dB">dB value</param>
        /// <returns>ratio value</returns>
        public static double dBToPowerRatio(double dB)
        {
            return Math.Pow(10, dB / 10.0);
        }

        /// <summary>
        /// Get the "x" correspinding to "y" by interpolating the interval between points (x1,y1) and (x2,y2)
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double GetXInterpolatingValue(double x1,double y1,double x2, double y2, double y)
        {
            double a = (y2-y1)/(x2-x1);
            double b = y1-a*x1;
            return (y - b) / a;
        }

        /// <summary>
        /// return y value for specified x value by using interpolating: Piecewise linear function 
        /// We have points (x,y). Points are represented as two arrays: xValues and yValues.
        /// NOTE xValues are equidistant and ascending!
        /// </summary>
        /// <param name="xValues">x values (EQUIDISTANT!)</param>
        /// <param name="yValues">y values</param>
        /// <param name="x">current x</param>
        /// <returns></returns>
        public static double GetYAsPLInterpolatingFor2DarrayWithEquidistantX(double[] xValues, double[] yValues, double x)
        {
            if ((x < xValues[0]) || (x > xValues[xValues.Length - 1]))
                throw new ArgumentOutOfRangeException();

            if (x == xValues[xValues.Length - 1])
                return yValues[yValues.Length - 1];

            double xIncrement = xValues[1] - xValues[0];

            int segmentStartIndex = (int)Math.Floor((x - xValues[0]) / xIncrement);

            double a = (yValues[segmentStartIndex+1] - yValues[segmentStartIndex]) / (xValues[segmentStartIndex + 1] - xValues[segmentStartIndex]);
            double b = yValues[segmentStartIndex] - a * xValues[segmentStartIndex];

            return a*x + b;
        }

        /// <summary>
        /// return y value for specified x value by using interpolating: Piecewise linear function 
        /// We have points (x,y). Points are represented as two arrays: xValues and yValues.
        /// NOTE xValues may NOT be equidistant! But is ascending!
        /// </summary>
        /// <param name="xValues">x values</param>
        /// <param name="yValues">y values</param>
        /// <param name="x">current x</param>
        /// <returns></returns>
        public static double GetYAsPLInterpolatingFor2Darray(double[] xValues, double[] yValues, double x)
        {
            if ((x < xValues[0]) || (x > xValues[xValues.Length - 1]))
                throw new ArgumentOutOfRangeException();

            if (x == xValues[xValues.Length - 1])
                return yValues[yValues.Length - 1];


            //searching for segment start index
            int segmentStartIndex = -1;
            for (int i = 0; i < xValues.Length - 1; i++)
                if (xValues[i + 1] > x)
                {
                    segmentStartIndex = i;
                    break;
                }
            
            double xIncrement = xValues[segmentStartIndex+1] - xValues[segmentStartIndex];


            double a = (yValues[segmentStartIndex + 1] - yValues[segmentStartIndex]) / (xValues[segmentStartIndex + 1] - xValues[segmentStartIndex]);
            double b = yValues[segmentStartIndex] - a * xValues[segmentStartIndex];

            return a * x + b;
        }

        /// <summary>
        /// return endpoints from xValues array for specified x value 
        /// We have x value and array of xValues.
        /// NOTE xValues may NOT be equidistant! But is ascending!
        /// </summary>
        /// <param name="xValues">x values</param>
        /// <param name="x">current x</param>
        /// <param name="leftEndpoint">OUTPUT: left border from xValues-array for the current x</param>
        /// <param name="rightEndpoint">OUTPUT: right border from xValues-array for the current x</param>
        public static void GetEndpointsOfSegmentForValueIn1Darray(double[] xValues, double x, out double leftEndpoint, out double rightEndpoint)
        {
            if ((x < xValues[0]) || (x > xValues[xValues.Length - 1]))
                throw new ArgumentOutOfRangeException();

            if (x == xValues[xValues.Length - 1])
            {
                leftEndpoint = xValues[xValues.Length - 2];
                rightEndpoint = xValues[xValues.Length - 1];
                return;
            }

            //searching for segment start index
            int segmentStartIndex = -1;
            for (int i = 0; i < xValues.Length - 1; i++)
                if (xValues[i + 1] > x)
                {
                    segmentStartIndex = i;
                    break;
                }

            leftEndpoint = xValues[segmentStartIndex];
            rightEndpoint = xValues[segmentStartIndex + 1];

            return;
        }
    }
    public delegate void ProgressChangedHandler(object sender, EventArgs arg);
     
    /// <summary>
    /// math calculation. With events
    /// </summary>
    public class MyMath_WE
    {
        public event ProgressChangedHandler ProgressChanged;
        //rought % of progress
        private int progressPercentage = 0;
        public int ProgressPercentage
        {
            get { return progressPercentage; }
        }
        /// <summary>
        /// Root-finding of "func" by bisection method. Based on static class MyMathюSolutionByTheBisectionMethod
        /// During the calculation rised event with step precentStep.
        /// 100% of progress is  iMax!
        /// </summary>
        /// <param name="func">function of one variable</param>
        /// <param name="x_start">begin of segment with root</param>
        /// <param name="x_finish">end of segment with root</param>
        /// <param name="root_eps">accuracy of root-finding</param>
        /// <param name="iMax">max value of stemps during search</param>
        /// <param name="precentStep">length of step fore rise event [%]</param>
        /// <returns>root</returns>
        public double SolutionByTheBisectionMethod(SomeFunction func, double x_start, double x_finish, double root_eps, int iMax, int precentStep)
        {
            //проверки
            if ((x_finish <= x_start) || (root_eps <= 0) || (func(x_finish) * func(x_start) > 0))
                throw new Exception("Wrong Input Parameters at SolutionByTheBisectionMethod");
            double x_middle = 0;
            this.progressPercentage = 0;

            int i = 0; //steps count
            while ((x_finish - x_start) >= root_eps)
            {
                x_middle = (x_start + x_finish) / 2;
                if (func(x_start) * func(x_middle) < 0)
                {
                    x_finish = x_middle;
                }
                else
                {
                    x_start = x_middle;
                }
                if (++i > iMax)
                {
                    throw new Exception("Превышено максимальное число шагов в цикле");
                }

                //rise event
                //rought % of cycle completed is i/iMax. Changes with step = precentStep
                int currentPercentage = 100 * i / iMax;
                if (currentPercentage - progressPercentage > precentStep)
                {
                    progressPercentage = currentPercentage;
                    if (ProgressChanged != null)
                    {
                        ProgressChanged(this, new EventArgs());
                    }
                }
                //System.Threading.Thread.Sleep(1000);
            }
            double root = (x_start + x_finish) / 2;
            return root;
        }
    }
}
