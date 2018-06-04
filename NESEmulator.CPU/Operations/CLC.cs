using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Operations.Bases;

namespace NESEmulator.CPU.Operations
{
    /**
     * Sets the carry flag to 0 only.
     */
    public class CLC : IOperation
    {
        public byte[] Opcodes { get; } = {0x00 + 0x18};

        public void Execute(State state)
        {
            state.ClockCycle += Implied.StandardCpuCycles;
            state.Status.Carry = false;
        }
    }
}
