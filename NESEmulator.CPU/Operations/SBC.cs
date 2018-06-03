using System.Collections.Generic;
using System.Linq;
using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Helpers;
using NESEmulator.CPU.Models;

namespace NESEmulator.CPU.Operations
{
    /**
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

            // TODO actually subtract!
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
