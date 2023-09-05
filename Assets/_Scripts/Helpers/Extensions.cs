using System;
using System.Numerics;
using System.Text;
using UnityEngine;

namespace _Scripts.Helpers
{
    public static class Extensions
    {
        private static readonly StringBuilder SB = new();

        public static string ToScientificNotationString(this BigInteger number, int digitsAfterComa = 2,
            int maxDigitsAtStart = 3)
        {
            SB.Clear();

            string numberString = number.ToString();
            Span<char> span = numberString.ToCharArray();
            int digits = numberString.Length;
            int digitsAtStart = digits % maxDigitsAtStart == 0 ? maxDigitsAtStart : digits % maxDigitsAtStart;

            Span<char> mantissa = span.Slice(0, digitsAtStart);
            AppendMantissa(mantissa);

            int exponent = digits - digitsAtStart;
            Debug.Assert(exponent >= 0);

            if (RequireExponent())
            {
                Span<char> decimalFraction = GetDecimalFraction(digitsAfterComa, span, digitsAtStart);
                AppendDecimalFraction(decimalFraction);

                AppendExponent(exponent);
            }

            return SB.ToString();

            bool RequireExponent()
            {
                return exponent > 0;
            }
        }

        private static void AppendMantissa(Span<char> mantissa)
        {
            SB.Append(mantissa);
        }

        private static void AppendExponent(int exponent)
        {
            SB.Append('E');
            SB.Append(exponent);
        }

        private static void AppendDecimalFraction(Span<char> decimalFraction)
        {
            SB.Append('.');

            SB.Append(decimalFraction);
        }

        private static Span<char> GetDecimalFraction(int digitsAfterComa, Span<char> span, int digitsAtStart)
        {
            return span.Slice(digitsAtStart, digitsAfterComa);
        }

        public static float ToTotalSeconds(this TimeSpan timeSpan)
        {
            return (float)timeSpan.TotalSeconds;
        }
    }
}