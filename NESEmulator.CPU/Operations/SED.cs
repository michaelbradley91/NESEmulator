using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Operations.Bases;

namespace NESEmulator.CPU.Operations
{
    /**
     * Sets the disable flag to 1 only.
     */
    public class SED : IOperation
    {
        public byte[] Opcodes { get; } = {0xE0 + 0x18};

        public void Execute(State state)
        {
            state.ClockCycle += Implied.StandardCpuCycles;
            state.Status.DecimalMode = true;
        }
    }
}
