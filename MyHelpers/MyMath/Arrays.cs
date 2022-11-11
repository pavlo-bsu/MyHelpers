using System;
using System.Numerics;

namespace Pavlo.MyHelpers.MyMath
{
    public static class Arrays
    {
        /// <summary>
        /// Clone (deep copy) input jaggedArray
        /// </summary>
        /// <param name="arr">array to clone in new instance</param>
        /// <returns>cloned array</returns>
        public static Complex[][] CloneJaggedArray(Complex[][] arr)
        {
            Complex[][] newArr = new Complex[arr.Length][];
            for (int i = 0; i < arr.Length; i++)
            {
                newArr[i] = new Complex[arr[i].Length];
                arr[i].CopyTo(newArr[i], 0);
            }
            return newArr;
        }

        /// <summary>
        /// Clone (deep copy) input jaggedArray
        /// </summary>
        /// <param name="arr">array to clone in new instance</param>
        /// <returns>cloned array</returns>
        public static double[][] CloneJaggedArray(double[][] arr)
        {
            double[][] newArr = new double[arr.Length][];
            for (int i = 0; i < arr.Length; i++)
            {
                newArr[i] = new double[arr[i].Length];
                arr[i].CopyTo(newArr[i], 0);
            }
            return newArr;
        }

        /// <summary>
        /// create an 2D double jagged array from input Complex array. Takes only Real part of Complex values.
        /// </summary>
        /// <param name="cArray"></param>
        /// <returns></returns>
        public static double[][] ComplexRPtoDouble(Complex[][] cArray)
        {
            double[][] dArray = new double[cArray.Length][];
            for (int i = 0; i < dArray.Length; i++)
            {
                dArray[i] = new double[cArray[i].Length];
                for (int j = 0; j < dArray[i].Length; j++)
                {
                    dArray[i][j] = cArray[i][j].Real;
                }
            }
            return dArray;
        }

        /// <summary>
        /// get index of "value" in equidistant "array"
        /// </summary>
        /// <param name="array">ascending sorted equidistant array</param>
        /// <param name="value">value of searched index</param>
        /// <returns>index or "-1" if 1) "value" > array[LastElement]+increment/2 or 2) array[FirstElement]-increment/2 > "value"  </returns>
        public static int GetIndexByValueInEquidistantArray(double[] array, double value)
        {
            if (array.Length == 1)
                return 0;
            double increment = array[1] - array[0];
            if (value > array[array.Length - 1]+increment/2)
                return -1;
            if (value < array[0]-increment/2)
                return -1;

            int index = (int)Math.Round((value - array[0]) / increment);
            return index;
        }

        /// <summary>
        /// search to the left in "array" position of the first element lower than "value"
        /// </summary>
        /// <param name="array">an array</param>
        /// <param name="value">searched value</param>
        /// <param name="startPosition">start position of the search</param>
        /// <returns>index or "-1" if search is unsuccessful</returns>
        public static int SearchPositionToTheLeftFirstLower(double[] array, double value, int startPosition)
        {
            int i = startPosition;
            while (value <= array[i])
            {
                if (i == 0)
                    return -1;
                i--;
            }
            return i;
        }

        /// <summary>
        /// search to the left in "array" position of the first element higher than "value"
        /// </summary>
        /// <param name="array">an array</param>
        /// <param name="value">searched value</param>
        /// <param name="startPosition">start position of the search</param>
        /// <returns>index or "-1" if search is unsuccessful</returns>
        public static int SearchPositionToTheLeftFirstHigher(double[] array, double value, int startPosition)
        {
            int i = startPosition;
            while (value >= array[i])
            {
                if (i == 0)
                    return -1;
                i--;
            }
            return i;
        }

        /// <summary>
        /// search to the right in "array" position of the first element lower than "value"
        /// </summary>
        /// <param name="array">an array</param>
        /// <param name="value">searched value</param>
        /// <param name="startPosition">start position of the search</param>
        /// <returns>index or "-1" if search is unsuccessful</returns>
        public static int SearchPositionToTheRightFirstLower(double[] array, double value, int startPosition)
        {
            int i = startPosition;
            while (value <= array[i])
            {
                if (i == array.Length-1)
                    return -1;
                i++;
            }
            return i;
        }

        /// <summary>
        /// search to the right in "array" position of the first element higher than "value"
        /// </summary>
        /// <param name="array">an array</param>
        /// <param name="value">searched value</param>
        /// <param name="startPosition">start position of the search</param>
        /// <returns>index or "-1" if search is unsuccessful</returns>
        public static int SearchPositionToTheRightFirstHigher(double[] array, double value, int startPosition)
        {
            int i = startPosition;
            while (value >= array[i])
            {
                if (i == array.Length - 1)
                    return -1;
                i++;
            }
            return i;
        }


        /// <summary>
        /// Create an array in which element equal to the average value by the first dimension of input 2D array (averages of each column)
        /// </summary>
        /// <param name="array2D">input jagged array. Each column has the same length!</param>
        /// <returns></returns>
        public static double[] GetArrayWithAveragesByFirstDimention(double[][] array2D)
        {
            if (array2D == null || array2D.Length == 0 || array2D[0].Length == 0)
                return null;
            double[] averages = new double[array2D[0].Length];
            for (int j = 0; j < averages.Length; j++)
            {
                double sum = 0;
                for (int i = 0; i < array2D.Length; i++)
                {
                    sum += array2D[i][j];
                }
                averages[j] = sum / array2D.Length;
            }
            return averages;
        }
    }
}
