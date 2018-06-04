using System.Collections.Generic;
using System.Linq;
using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Operations.Bases;

namespace NESEmulator.CPU.Operations
{
    /**
     * ORs exclusive the value in the accumulator (A) register with the value pointed at in memory,
     * and stores the result in the accumulator.
     *
     * Flags:
     * Unless otherwise stated, all flags mentioned are set to 0 if the condition is not met.
     * If the flag is not mentioned, its value is not changed.
     *
     * The zero flag is set if the accumulator is set to zero.
     * The negative flag is set if the accumulator has bit 7 set to 1.
     *
     */
    public class EOR : IOperation
    {
        public static IDictionary<byte, IAddressingMode> OpcodeMap { get; } =
            new Dictionary<byte, IAddressingMode>
            {
                {0x40 + 0x01, IndexedIndirectX.Instance}, // Indexed indirect (d, x)
                {0x40 + 0x05, ZeroPage.Instance}, // Zero page d
                {0x40 + 0x09, Immediate.Instance}, // Immediate #i
                {0x40 + 0x0D, Absolute.Instance}, // Absolute a
                {0x40 + 0x11, IndirectIndexedY.Instance}, // Indirect Indexed (d), y
                {0x40 + 0x15, ZeroPageIndexedX.Instance}, // Zero page X d, x
                {0x40 + 0x19, AbsoluteIndexedY.Instance}, // Absolute, Y a, y
                {0x40 + 0x1D, AbsoluteIndexedX.Instance} // Absolute, X a, x
            };

        public EOR()
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

            var result = (byte)(value ^ state.Registers.A);

            state.Registers.A = result;
            state.Status.ZeroResult = result == 0;
            state.Status.NegativeResult = result >= 128;
        }
    }
}
