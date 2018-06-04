using System.Collections.Generic;
using System.Linq;
using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Helpers;
using NESEmulator.CPU.Operations.Bases;

namespace NESEmulator.CPU.Operations
{
    /**
     * Subtract the value at a memory location from the A register with borrow. Borrow is the complement of the carry.
     *
     * This instruction takes the decimal flag into account. If the decimal flag is set,
     * the numbers are subtracted and stored in Binary Coded Decimal (BCD). Each of 4 bits determines the
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
     * do not have their values changed. For SBC only, all flags behave in decimal mode identically to binary mode*.
     *
     * Set the Zero flag if the result is all zeroes in the A register (regardless of borrow)
     * Set the Carry flag if the result is >= 0 in the A register (so, specify borrow if it less than 0)
     * Set the Overflow flag when the sign bit (bit 7) is incorrect.
     * Set the negative flag if bit 7 of the A register is 1.
     *
     * * This is specific to 6502 and not true of variants. See:
     * http://www.6502.org/tutorials/decimal_mode.html#A
     * http://www.6502.org/tutorials/vflag.html#b
     *
     */
    public class SBC : IOperation
    {
        public static IDictionary<byte, IAddressingMode> OpcodeMap { get; } =
            new Dictionary<byte, IAddressingMode>
            {
                {0xE0 + 0x01, IndexedIndirectX.Instance}, // Indexed indirect (d, x)
                {0xE0 + 0x05, ZeroPage.Instance}, // Zero page d
                {0xE0 + 0x09, Immediate.Instance}, // Immediate #i
                {0xE0 + 0x0D, Absolute.Instance}, // Absolute a
                {0xE0 + 0x11, IndirectIndexedY.Instance}, // Indirect Indexed (d), y
                {0xE0 + 0x15, ZeroPageIndexedX.Instance}, // Zero page X d, x
                {0xE0 + 0x19, AbsoluteIndexedY.Instance}, // Absolute, Y a, y
                {0xE0 + 0x1D, AbsoluteIndexedX.Instance} // Absolute, X a, x
            };

        public SBC()
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
                ? ArithmeticHelpers.SubtractDecimal(state.Registers.A, value, state.Status.Carry)
                : ArithmeticHelpers.SubtractBinary(state.Registers.A, value, state.Status.Carry);

            state.Registers.A = arithmeticResult.Result;
            state.Status.Carry = arithmeticResult.Carried;
            state.Status.Overflow = arithmeticResult.Overflowed;
            state.Status.ZeroResult = arithmeticResult.Zeroed;
            state.Status.NegativeResult = arithmeticResult.Negative;
        }
    }
}
