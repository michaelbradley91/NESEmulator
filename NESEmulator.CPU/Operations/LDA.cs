using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NESEmulator.CPU.Addressing;

namespace NESEmulator.CPU.Operations
{
    public class LDA : IOperation
    {
        public static IDictionary<byte, IAddressingMode> OpcodeMap { get; } =
            new Dictionary<byte, IAddressingMode>
            {
                {0xA0 + 0x01, IndexedIndirectX.Instance}, // Indexed indirect (d, x)
                {0xA0 + 0x05, ZeroPage.Instance}, // Zero page d
                {0xA0 + 0x09, Immediate.Instance}, // Immediate #i
                {0xA0 + 0x0D, Absolute.Instance}, // Absolute a
                {0xA0 + 0x11, IndirectIndexedY.Instance}, // Indirect Indexed (d), y
                {0xA0 + 0x15, ZeroPageIndexedX.Instance}, // Zero page X d, x
                {0xA0 + 0x19, AbsoluteIndexedY.Instance}, // Absolute, Y a, y
                {0xA0 + 0x1D, AbsoluteIndexedX.Instance} // Absolute, X a, x
            };

        public LDA()
        {
            Opcodes = OpcodeMap.Keys.ToArray();
        }

        public byte[] Opcodes { get; }

        public void Execute(State state)
        {
            var addressingMode = OpcodeMap[state.OpCode];
            var (targetAddress, canSkipCycle) = addressingMode.GetAddress(state);

            state.Registers.A = state.Memory[targetAddress];

            // Is the 7th bit of A set?
            state.Status.NegativeResult = state.Registers.A >= 128;

            // Is the value of A zero?
            state.Status.ZeroResult = state.Registers.A == 0;

            /*
             * If no page boundary was crossed, the processor's early write
             * of the value in memory to register A is correct, and so no additional
             * step is required to process the carry flag.
             */ 
            state.ClockCycle += addressingMode.StandardCpuCycles - (canSkipCycle ? 1 : 0);
        }
    }
}
