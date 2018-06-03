using System.Collections.Generic;
using System.Linq;
using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Helpers;
using NESEmulator.CPU.Models;

namespace NESEmulator.CPU.Operations
{
    /**
     * Add the value at a memory location to the A register with carry. By with carry, we mean adding
     * one if the carry flag is set.
     *
     * This instruction takes the decimal flag into account. If the decimal flag is set,
     * the numbers are added and stored in Binary Coded Decimal (BCD). Each of 4 bits determines the
     * value of a decimal number in BCD, so the largest number possible is 99:
     *
     * Examples:
     * 0000 0001 = 01
     * 0111 0010 = 72
     * 1001 1001 = 99
     *
     * Flags:
     *
     * All flags mentioned are set to zero if the condition is not met. All other flags
     * do not have their values changed.
     *
     * Set the Zero flag if the result is all zeroes in the A register (regardless of carry)
     * Set the Carry flag if the sum is too large to be stored. In the case of decimal, this is if
     * the sum is > 99. In the case of binary, this is if the sum is greater than 255.
     * Set the Overflow flag when the sign bit (bit 7) is incorrect.
     * Set the negative flag if bit 7 of the A register is 1. (both decimal and binary)
     *
     * Overflow:
     *
     * Overflow occurs when two positive numbers added together yields a negative number,
     * or when two negative numbers added together yields a positive number. Otherwise,
     * overflow has not occurred. See: http://sandbox.mc.edu/~bennet/cs110/tc/orules.html
     *
     * Addition in decimal mode is unsigned, so the overflow behaviour isn't well defined.
     * See http://www.6502.org/tutorials/decimal_mode.html#A
     *
     */
    public class ADC : IOperation
    {
        public static IDictionary<byte, IAddressingMode> OpcodeMap { get; } =
            new Dictionary<byte, IAddressingMode>
            {
                {0x60 + 0x01, IndexedIndirectX.Instance}, // Indexed indirect (d, x)
                {0x60 + 0x05, ZeroPage.Instance}, // Zero page d
                {0x60 + 0x09, Immediate.Instance}, // Immediate #i
                {0x60 + 0x0D, Absolute.Instance}, // Absolute a
                {0x60 + 0x11, IndirectIndexedY.Instance}, // Indirect Indexed (d), y
                {0x60 + 0x15, ZeroPageIndexedX.Instance}, // Zero page X d, x
                {0x60 + 0x19, AbsoluteIndexedY.Instance}, // Absolute, Y a, y
                {0x60 + 0x1D, AbsoluteIndexedX.Instance} // Absolute, X a, x
            };

        public ADC()
        {
            Opcodes = OpcodeMap.Keys.ToArray();
        }

        public byte[] Opcodes { get; }

        public void Execute(State state)
        {
            var addressingMode = OpcodeMap[state.OpCode];
            var (targetAddress, canSkipCycle) = addressingMode.GetAddress(state);
            
            var value = state.Memory[targetAddress];
            state.ClockCycle += addressingMode.StandardCpuCycles - (canSkipCycle ? 1 : 0);

            var arithmeticResult = state.Status.DecimalMode
                ? ArithmeticHelpers.AddDecimal(state.Registers.A, value, state.Status.Carry)
                : ArithmeticHelpers.AddBinary(state.Registers.A, value, state.Status.Carry);

            state.Registers.A = arithmeticResult.Result;
            state.Status.Carry = arithmeticResult.Carried;
            state.Status.Overflow = arithmeticResult.Overflowed;
            state.Status.ZeroResult = arithmeticResult.Zeroed;
            state.Status.NegativeResult = arithmeticResult.Negative;
        }
    }
}
