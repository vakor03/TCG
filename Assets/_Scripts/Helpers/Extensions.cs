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

            SB.Append(span.Slice(0, digitsAtStart));
            
            int eCount = digits - digitsAtStart;
            Debug.Assert(eCount >= 0);

            if (eCount > 0)
            {
                SB.Append('.');
                SB.Append(span.Slice(digitsAtStart, digitsAfterComa));
                
                SB.Append('E');
                SB.Append(eCount);
            }

            return SB.ToString();
        }
    }
}