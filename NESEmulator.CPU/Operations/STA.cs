using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NESEmulator.CPU.Addressing;

namespace NESEmulator.CPU.Operations
{
    public class STA : IOperation
    {
        public static IDictionary<byte, IAddressingMode> OpcodeMap { get; } =
            new Dictionary<byte, IAddressingMode>
            {
                {0x80 + 0x01, IndexedIndirectX.Instance}, // Indexed indirect (d, x)
                {0x80 + 0x05, ZeroPage.Instance}, // Zero page d
                {0x80 + 0x0D, Absolute.Instance}, // Absolute a
                {0x80 + 0x11, IndirectIndexedY.Instance}, // Indirect Indexed (d), y
                {0x80 + 0x15, ZeroPageIndexedX.Instance}, // Zero page X d, x
                {0x80 + 0x19, AbsoluteIndexedY.Instance}, // Absolute, Y a, y
                {0x80 + 0x1D, AbsoluteIndexedX.Instance} // Absolute, X a, x
            };

        public STA()
        {
            Opcodes = OpcodeMap.Keys.ToArray();
        }

        public byte[] Opcodes { get; }

        public void Execute(State state)
        {
            var addressingMode = OpcodeMap[state.OpCode];
            var (targetAddress, _) = addressingMode.GetAddress(state);

            state.Memory[targetAddress] = state.Registers.A;

            /*
             * It is not possible to skip a cycle when a page boundary is not crossed
             * as the processor cannot risk writing to memory early.
             *
             * Consider the cast of absolute indexed X addressing:
             * (X = 01, A = 02)
             * STA $01FF, X
             *
             * If the processor adds A, before the carry is processed the
             * address will appear to be $0100. If A is written to this now, in the hope
             * of skipping a cycle, we would destroy whatever was written to $0100 before.
             *
             * After the carry is processed, the address is now $0200. This is the correct
             * address and should be written to.
             *
             * LDA by contrast can skip a cycle. This is because writing to register A
             * in the hopes of skipping a cycle is not dangerous, as A will be written
             * to again in the event the carry matters (the previous value in A was going
             * to be destroyed anyway).
             */
            state.ClockCycle += addressingMode.StandardCpuCycles;
        }
    }
}
