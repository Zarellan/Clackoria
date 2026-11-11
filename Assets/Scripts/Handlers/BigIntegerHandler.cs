using System;
using System.Numerics;
using UnityEngine;

public static class BigIntegerHandler
{
    public static BigInteger MultiplyFloat(BigInteger value1, float value2)
    {
        // Convert float to decimal with higher precision
        decimal decimalValue2 = (decimal)value2;

        // Scale value2 by 1000 (or more) to preserve more precision
        decimal scaledMultiplier = decimalValue2 * 1000m;
        
        // Convert scaled multiplier to BigInteger
        BigInteger scaledMultiplierBigInt = new BigInteger(Math.Round(scaledMultiplier));

        // Multiply value1 by scaled multiplier and divide by 1000 to preserve decimal precision
        BigInteger result = value1 * scaledMultiplierBigInt / 1000;

        return result;
    }
    public static BigInteger MultiplyDecimal(BigInteger value1, decimal value2)
    {
        // Scale value2 by 1000 (or a higher factor) to preserve more precision
        decimal scaledMultiplier = value2 * 1000m;
        
        // Convert scaled multiplier to BigInteger
        BigInteger scaledMultiplierBigInt = new BigInteger(Math.Round(scaledMultiplier));

        // Multiply value1 by scaled multiplier and divide by 1000 to preserve decimal precision
        BigInteger result = value1 * scaledMultiplierBigInt / 1000;

        return result;
    }
    public static BigInteger DivideFloat(BigInteger value1, float value2)
    {
        // Convert float to decimal with higher precision
        decimal decimalValue2 = (decimal)value2;

        // Scale value2 by 1000 to preserve decimal places
        decimal scaledDivisor = decimalValue2 * 1000m;
        
        // Convert scaled divisor to BigInteger
        BigInteger scaledDivisorBigInt = new BigInteger(Math.Round(scaledDivisor));

        // Multiply value1 by 1000 to match scaling, then divide by the scaled divisor
        BigInteger result = value1 * 1000 / scaledDivisorBigInt;

        return result;
    }

    public static BigInteger DivideDecimal(BigInteger value1, decimal value2)
    {
        // Scale value2 by 1000 to preserve decimal places
        decimal scaledDivisor = value2 * 1000m;
        
        // Convert scaled divisor to BigInteger
        BigInteger scaledDivisorBigInt = new BigInteger(Math.Round(scaledDivisor));

        // Multiply value1 by 1000 to match scaling, then divide by the scaled divisor
        BigInteger result = value1 * 1000 / scaledDivisorBigInt;

        return result;
    }
}
