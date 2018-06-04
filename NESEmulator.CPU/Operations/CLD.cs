using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Operations.Bases;

namespace NESEmulator.CPU.Operations
{
    /**
     * Sets the disable flag to 0 only.
     */
    public class CLD : IOperation
    {
        public byte[] Opcodes { get; } = {0xC0 + 0x18};

        public void Execute(State state)
        {
            state.ClockCycle += Implied.StandardCpuCycles;
            state.Status.DecimalMode = false;
        }
    }
}
