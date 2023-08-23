using System;
using System.Numerics;
using UnityEngine;

namespace _Scripts.Helpers
{
    public static class BigIntegerExtensions
    {
        public static BigInteger Multiply(this BigInteger value, float multiplier, int precision)
        {
            if (precision < 0)
            {
                throw new ArgumentException();
            }

            return value * new BigInteger(multiplier * Mathf.Pow(10, precision)) / new BigInteger(Mathf.Pow(10, precision));
        }
        
        public static BigInteger Divide(this BigInteger value, float divider, int precision)
        {
            if (precision < 0)
            {
                throw new ArgumentException();
            }

            return value * new BigInteger(Mathf.Pow(10, precision)) / new BigInteger(divider * Mathf.Pow(10, precision));
        }
    }
}