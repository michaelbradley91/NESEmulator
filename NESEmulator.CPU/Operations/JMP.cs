using System.Collections.Generic;
using System.Linq;
using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Operations.Bases;

namespace NESEmulator.CPU.Operations
{
    /**
     * Sets the program counter to the address specified in the operation. Affects no flags.
     * See IndirectAbsolute for some details around zero page addressing.
     */
    public class JMP : IOperation
    {
        public static IDictionary<byte, IAddressingMode> OpcodeMap { get; } =
            new Dictionary<byte, IAddressingMode>
            {
                {0x40 + 0x0C, Absolute.Instance}, // Absolute a
                {0x60 + 0x0C, IndirectAbsolute.Instance} // Indirect absolute (a)
            };

        public JMP()
        {
            Opcodes = OpcodeMap.Keys.ToArray();
        }

        public byte[] Opcodes { get; }

        public void Execute(State state)
        {
            var addressingMode = OpcodeMap[state.OpCode];
            var (targetAddress, _) = addressingMode.GetAddress(state);

            state.ClockCycle += addressingMode == Absolute.Instance ? 3 : 5;
            state.Registers.PC = targetAddress;
            state.Status.Overflow = false;

            // The address points at the next operation to be executed, so the PC
            // should not increment again
            state.SkipNextPCIncrement = true;
        }
    }
}
