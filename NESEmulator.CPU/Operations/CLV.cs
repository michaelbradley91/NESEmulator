using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Operations.Bases;

namespace NESEmulator.CPU.Operations
{
    /**
     * Sets the overflow flag to 0 only.
     */
    public class CLV : IOperation
    {
        public byte[] Opcodes { get; } = {0xA0 + 0x18};

        public void Execute(State state)
        {
            state.ClockCycle += Implied.StandardCpuCycles;
            state.Status.Overflow = false;
        }
    }
}
