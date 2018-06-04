using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Operations.Bases;

namespace NESEmulator.CPU.Operations
{
    /**
     * Sets the interrupt disable flag to 0 only.
     */
    public class CLI : IOperation
    {
        public byte[] Opcodes { get; } = {0x40 + 0x18};

        public void Execute(State state)
        {
            state.ClockCycle += Implied.StandardCpuCycles;
            state.Status.InterruptDisable = false;
        }
    }
}
