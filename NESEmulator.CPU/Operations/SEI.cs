using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Operations.Bases;

namespace NESEmulator.CPU.Operations
{
    /**
     * Sets the interrupt disable flag to 1 only.
     */
    public class SEI : IOperation
    {
        public byte[] Opcodes { get; } = {0x60 + 0x18};

        public void Execute(State state)
        {
            state.ClockCycle += Implied.StandardCpuCycles;
            state.Status.InterruptDisable = true;
        }
    }
}
