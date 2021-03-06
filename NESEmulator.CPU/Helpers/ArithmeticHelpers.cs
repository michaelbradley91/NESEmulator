﻿using System;
using NESEmulator.CPU.Models;

namespace NESEmulator.CPU.Helpers
{
    /**
     * Manages the complex arithmetic operators in the 6502. This should handle
     * undocumented arithmetic operations as well.
     */
    public static class ArithmeticHelpers
    {
        public static ArithmeticResult AddBinary(byte firstByte, byte secondByte, bool carry)
        {
            var carryOccurred = false;

            var trueUnsignedResult = firstByte + secondByte + (carry ? 1 : 0);
            if (trueUnsignedResult > 255)
            {
                carryOccurred = true;
            }

            var result = (byte)(trueUnsignedResult % 256);
            var isNegative = result >= 128;

            var trueSignedResult = (sbyte)firstByte + (sbyte)secondByte + (carry ? 1 : 0);
            var overflowOccurred = trueSignedResult < 0 && !isNegative || trueSignedResult >= 0 && isNegative;
            
            return new ArithmeticResult(overflowOccurred, carryOccurred, result == 0, isNegative, result);
        }

        public static ArithmeticResult AddDecimal(byte a, byte b, bool carry)
        {
            // See http://www.6502.org/tutorials/decimal_mode.html#A
            // Handling of invalid decimal values is covered in Appendix A
            // The carry flag is the only flag considered to have meaning in the 6502,
            // but the other flags are set in an undocumented way.

            // Sequence one
            var al = (a & 0x0F) + (b & 0x0F) + (carry ? 1 : 0);
            if (al >= 0x0A)
            {
                al = (byte)(((al + 0x06) & 0x0F) + 0x10);
            }

            var intermediateResult = (a & 0xF0) + (b & 0xF0) + al;
            if (intermediateResult >= 0xA0) intermediateResult += 0x60;

            var result = (byte)(intermediateResult % 256);
            var carryOccurred = intermediateResult >= 0x100;

            // Some other details: http://visual6502.org/wiki/index.php?title=6502DecimalMode
            // All about the overflow flag specifically: http://www.6502.org/tutorials/vflag.html

            // Sequence two (al is the same)
            var otherIntermediateResult = (sbyte)(a & 0xF0) + (sbyte)(b & 0xF0) + (sbyte)al;
            Console.WriteLine("Other intermediate: " + otherIntermediateResult);
            var overflowOccurred = otherIntermediateResult < -128 || otherIntermediateResult > 127;

            // If overflow occurred, then the sign of the result would have flipped
            var negativeResult = (overflowOccurred ? -otherIntermediateResult : otherIntermediateResult) < 0;

            // The zero flag in decimal mode is based on the result in binary, not decimal
            var binaryResult = AddBinary(a, b, carry);
            var zeroed = binaryResult.Result == 0;

            return new ArithmeticResult(overflowOccurred, carryOccurred, zeroed, negativeResult, result);
        }

        public static ArithmeticResult SubtractBinary(byte a, byte b, bool carry)
        {
            // Deduct the value of the complemented carry
            var trueSignedResult = (sbyte)a - (sbyte)b + ((carry ? 1 : 0) - 1);
            var trueUnsignedResult = a - (sbyte) b + ((carry ? 1 : 0) - 1);
            var result = (byte) (trueUnsignedResult % 256);

            var carryOccurred = trueUnsignedResult >= 0;
            var overflowOccurred = trueSignedResult > 127 || trueSignedResult < -127;
            var negativeResult = result >= 128;
            var zeroResult = result == 0;

            return new ArithmeticResult(overflowOccurred, carryOccurred, zeroResult, negativeResult, result);
        }

        public static ArithmeticResult SubtractDecimal(byte a, byte b, bool carry)
        {
            // See http://www.6502.org/tutorials/decimal_mode.html
            // for an explanation in decimal subtraction. This handles invalid
            // decimals in as close a way to the hardware as possible.
            var c = carry ? 1 : 0;

            var al = (a & 0x0F) - (b & 0x0F) + (c - 1);
            if (al < 0)
            {
                al = ((al - 0x06) & 0x0F) - 0x10;
            }

            var intermediateResult = (a & 0xF0) - (b & 0xF0) + al;
            if (intermediateResult < 0)
            {
                intermediateResult -= 0x60;
            }

            var result = (byte)(intermediateResult % 256);

            var binaryResult = SubtractBinary(a, b, carry);

            return new ArithmeticResult(binaryResult.Overflowed, binaryResult.Carried, binaryResult.Zeroed, binaryResult.Negative, result);
        }
    }
}
