using System;
using System.Runtime.InteropServices;
using PdfSharp.Drawing;
#if !EDF_CORE
namespace PdfSharp.Internal
#endif
{
    internal static class DoubleUtil
    {
        const double Epsilon = 2.2204460492503131E-16;         
        private const double TenTimesEpsilon = 10.0 * Epsilon;
        const float FloatMinimum = 1.175494E-38f;

        public static bool AreClose(double value1, double value2)
        {
            if (value1.Equals(value2))
                return true;
            double eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * Epsilon;
            double delta = value1 - value2;
            return (-eps < delta) && (eps > delta);
        }

        public static bool AreRoughlyEqual(double value1, double value2, int decimalPlace)
        {
            if (value1 == value2)
                return true;
            return Math.Abs(value1 - value2) < decs[decimalPlace];
        }
        static readonly double[] decs = { 1, 1E-1, 1E-2, 1E-3, 1E-4, 1E-5, 1E-6, 1E-7, 1E-8, 1E-9, 1E-10, 1E-11, 1E-12, 1E-13, 1E-14, 1E-15, 1E-16 };

        public static bool AreClose(XPoint point1, XPoint point2)
        {
            return AreClose(point1.X, point2.X) && AreClose(point1.Y, point2.Y);
        }

        public static bool AreClose(XRect rect1, XRect rect2)
        {
            if (rect1.IsEmpty)
                return rect2.IsEmpty;
            return !rect2.IsEmpty && AreClose(rect1.X, rect2.X) && AreClose(rect1.Y, rect2.Y) &&
              AreClose(rect1.Height, rect2.Height) && AreClose(rect1.Width, rect2.Width);
        }

        public static bool AreClose(XSize size1, XSize size2)
        {
            return AreClose(size1.Width, size2.Width) && AreClose(size1.Height, size2.Height);
        }

        public static bool AreClose(XVector vector1, XVector vector2)
        {
            return AreClose(vector1.X, vector2.X) && AreClose(vector1.Y, vector2.Y);
        }

        public static bool GreaterThan(double value1, double value2)
        {
            return value1 > value2 && !AreClose(value1, value2);
        }

        public static bool GreaterThanOrClose(double value1, double value2)
        {
            return value1 > value2 || AreClose(value1, value2);
        }

        public static bool LessThan(double value1, double value2)
        {
            return value1 < value2 && !AreClose(value1, value2);
        }

        public static bool LessThanOrClose(double value1, double value2)
        {
            return value1 < value2 || AreClose(value1, value2);
        }

        public static bool IsBetweenZeroAndOne(double value)
        {
            return GreaterThanOrClose(value, 0) && LessThanOrClose(value, 1);
        }

        public static bool IsNaN(double value)
        {
            NanUnion t = new NanUnion();
            t.DoubleValue = value;

            ulong exp = t.UintValue & 0xfff0000000000000;
            ulong man = t.UintValue & 0x000fffffffffffff;

            return (exp == 0x7ff0000000000000 || exp == 0xfff0000000000000) && (man != 0);
        }

        public static bool RectHasNaN(XRect r)
        {
            return IsNaN(r.X) || IsNaN(r.Y) || IsNaN(r.Height) || IsNaN(r.Width);
        }

        public static bool IsOne(double value)
        {
            return Math.Abs(value - 1.0) < TenTimesEpsilon;
        }

        public static bool IsZero(double value)
        {
            return Math.Abs(value) < TenTimesEpsilon;
        }

        public static int DoubleToInt(double value)
        {
            return 0 < value ? (int)(value + 0.5) : (int)(value - 0.5);
        }

        [StructLayout(LayoutKind.Explicit)]
        struct NanUnion
        {
            [FieldOffset(0)]
            internal double DoubleValue;
            [FieldOffset(0)]
            internal readonly ulong UintValue;
        }
    }
}
