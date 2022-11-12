using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pavlo.MyHelpers.MyMath;

namespace Pavlo.Tests
{
    [TestClass]
    public class PLInterpolating
    {
        //permissible delta for signals comparison
        double delta = 1e-12;

        [TestMethod]
        public void TestGetYAsPLInterpolatingFor2DarrayWithEquidistantX_CheckBorderValues()
        {
            //INPUT DATA
            double[] xValues = new double[] { 0, 1, 2, 3, 4, 5 };
            double[] yValues = new double[] { 2, 3, 10, 5, 5.5, 6 };

            double xBorderLow = 0.0;
            double xBorderHigh = 5.0;

            double expectedYBorderLow = 2.0;
            double expectedYBorderHigh = 6.0;

            //ACTION EXECUTION
            double yBorderLow = Common.GetYAsPLInterpolatingFor2DarrayWithEquidistantX(xValues, yValues, xBorderLow);
            double yBorderHigh = Common.GetYAsPLInterpolatingFor2DarrayWithEquidistantX(xValues, yValues, xBorderHigh); ;

            //COMPARISON
            Assert.AreEqual(expectedYBorderLow, yBorderLow, delta);
            Assert.AreEqual(expectedYBorderHigh, yBorderHigh, delta);
        }

        [TestMethod]
        public void TestGetYAsPLInterpolatingFor2DarrayWithEquidistantX_CheckValues()
        {
            //INPUT DATA
            double[] xValues = new double[] { 0, 1, 2, 3, 4, 5 };
            double[] yValues = new double[] { 2, 3, 10, 5, 5.5, 6 };

            //all expected values were calculated by Mathematica
            double x1 = 0.33;
            double expectedY1 = 2.33;

            double x2 = 1.73;
            double expectedY2 = 8.11;

            double x3 = 2.87;
            double expectedY3 = 5.65;

            double x4 = 4.123456789;
            double expectedY4 = 5.561728394499999;

            //ACTION EXECUTION
            double y1 = Common.GetYAsPLInterpolatingFor2DarrayWithEquidistantX(xValues, yValues, x1);
            double y2 = Common.GetYAsPLInterpolatingFor2DarrayWithEquidistantX(xValues, yValues, x2);
            double y3 = Common.GetYAsPLInterpolatingFor2DarrayWithEquidistantX(xValues, yValues, x3);
            double y4 = Common.GetYAsPLInterpolatingFor2DarrayWithEquidistantX(xValues, yValues, x4);

            //COMPARISON
            Assert.AreEqual(expectedY1, y1, delta);
            Assert.AreEqual(expectedY2, y2, delta);
            Assert.AreEqual(expectedY3, y3, delta);
            Assert.AreEqual(expectedY4, y4, delta);
        }

        [TestMethod]
        public void TestTextFileTwoVariablesStorage_Load_CheckValues()
        {
            //INPUT DATA
            string fileName = @"..\..\DataFilesInterpolation\tmpWire.csv";

            //ACTION EXECUTION
            double[] expectedXValues = new double[] { 0, 1, 2, 3, 4, 5 };
            double[] expectedYValues = new double[] { 2, 3, 10, 5, 5.5, 6 };

            var fileStorage = new EFSCalculator.DAL.TextFileTwoVariablesStorage(fileName);
            fileStorage.Load();

            var xValues = fileStorage.FirstVariableArray;
            var yValues = fileStorage.SecondVariableArray;

            //COMPARISON
            Assert.AreEqual(xValues.Length, yValues.Length);
            Assert.AreEqual(xValues.Length, expectedXValues.Length);
            for (int i = 0; i < xValues.Length; i++)
            {
                Assert.AreEqual(expectedXValues[i], xValues[i], delta);
                Assert.AreEqual(expectedYValues[i], yValues[i], delta);
            }
        }

        //tests for UNequidistant array
        [TestMethod]
        public void TestGetYAsPLInterpolatingFor2Darray_CheckBorderValues()
        {
            //INPUT DATA
            double[] xValues = new double[] { 0, 0.1, 0.3, 1, 4, 5 };
            double[] yValues = new double[] { 2, 3, 10, 5, 5.5, 6 };

            double xBorderLow = 0.0;
            double xBorderHigh = 5.0;

            double expectedYBorderLow = 2.0;
            double expectedYBorderHigh = 6.0;

            //ACTION EXECUTION
            double yBorderLow = Common.GetYAsPLInterpolatingFor2DarrayWithEquidistantX(xValues, yValues, xBorderLow);
            double yBorderHigh = Common.GetYAsPLInterpolatingFor2DarrayWithEquidistantX(xValues, yValues, xBorderHigh); ;

            //COMPARISON
            Assert.AreEqual(expectedYBorderLow, yBorderLow, delta);
            Assert.AreEqual(expectedYBorderHigh, yBorderHigh, delta);
        }

        [TestMethod]
        public void TestGetYAsPLInterpolatingFor2Darray_CheckValues()
        {
            //INPUT DATA
            double[] xValues = new double[] { 0, 0.1, 0.3, 1, 4, 5 };
            double[] yValues = new double[] { 2, 3, 10, 5, 5.5, 6 };

            //all expected values were calculated by Mathematica
            double x1 = 0.33;
            double expectedY1 = 9.78571428571429;

            double x2 = 1.73;
            double expectedY2 = 5.121666666666667;

            double x3 = 2.87;
            double expectedY3 = 5.311666666666667;

            double x4 = 4.123456789;
            double expectedY4 = 5.561728394499999;

            //ACTION EXECUTION
            double y1 = Common.GetYAsPLInterpolatingFor2Darray(xValues, yValues, x1);
            double y2 = Common.GetYAsPLInterpolatingFor2Darray(xValues, yValues, x2);
            double y3 = Common.GetYAsPLInterpolatingFor2Darray(xValues, yValues, x3);
            double y4 = Common.GetYAsPLInterpolatingFor2Darray(xValues, yValues, x4);

            //COMPARISON
            Assert.AreEqual(expectedY1, y1, delta);
            Assert.AreEqual(expectedY2, y2, delta);
            Assert.AreEqual(expectedY3, y3, delta);
            Assert.AreEqual(expectedY4, y4, delta);
        }

        [TestMethod]
        public void TestGetEndpointsOfSegmentForValueIn1Darray_CheckValues()
        {
            //INPUT DATA
            double[] xValues = new double[] { 0, 0.1, 0.3, 1, 4, 5 };

            double x1 = 0.33;
            double x1_expectedLeftBorder = 0.3;
            double x1_expectedRightBorder = 1;

            double x2 = 0;
            double x2_expectedLeftBorder = 0;
            double x2_expectedRightBorder = 0.1;

            double x3 = 5;
            double x3_expectedLeftBorder = 4;
            double x3_expectedRightBorder = 5;

            //ACTION EXECUTION
            double x1_LeftBorder;
            double x1_RightBorder;

            double x2_LeftBorder;
            double x2_RightBorder;

            double x3_LeftBorder;
            double x3_RightBorder;

            Common.GetEndpointsOfSegmentForValueIn1Darray(xValues, x1, out x1_LeftBorder, out x1_RightBorder);
            Common.GetEndpointsOfSegmentForValueIn1Darray(xValues, x2, out x2_LeftBorder, out x2_RightBorder);
            Common.GetEndpointsOfSegmentForValueIn1Darray(xValues, x3, out x3_LeftBorder, out x3_RightBorder);

            //COMPARISON
            Assert.AreEqual(x1_expectedLeftBorder, x1_LeftBorder, delta);
            Assert.AreEqual(x1_expectedRightBorder, x1_RightBorder, delta);
            Assert.AreEqual(x2_expectedLeftBorder, x2_LeftBorder, delta);
            Assert.AreEqual(x2_expectedRightBorder, x2_RightBorder, delta);
            Assert.AreEqual(x3_expectedLeftBorder, x3_LeftBorder, delta);
            Assert.AreEqual(x3_expectedRightBorder, x3_RightBorder, delta);
        }
    }
}
