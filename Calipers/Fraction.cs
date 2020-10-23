using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text;

namespace Calipers
{
    public class Fraction
    {
        public BigInteger Numerator { get; set; }
        public BigInteger Denominator { get; set; }

        public Fraction(BigInteger numerator, BigInteger denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public static Fraction Max(Fraction f1, Fraction f2)
        {
            return f1.Numerator * f2.Denominator > f2.Numerator * f1.Denominator ? f1 : f2;
        }

        public static int Compare(Fraction f1, Fraction f2)
        {
            BigInteger bi1 = f1.Numerator * f2.Denominator;
            BigInteger bi2 = f2.Numerator * f1.Denominator;
            return bi1 == bi2 ? 0 : bi1 > bi2 ? 1 : -1;
        }
    }
}
