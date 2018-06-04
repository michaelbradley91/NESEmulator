using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Operations.Bases;

namespace NESEmulator.CPU.Operations
{
    /**
     * Sets the carry flag to 1 only.
     */
    public class SEC : IOperation
    {
        public byte[] Opcodes { get; } = {0x20 + 0x18};

        public void Execute(State state)
        {
            state.ClockCycle += Implied.StandardCpuCycles;
            state.Status.Carry = true;
        }
    }
}
