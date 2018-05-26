namespace NESEmulator.CPU.Addressing
{
    public class Immediate : IAddressingMode
    {
        public byte Get(State state)
        {
            // Expected 2 cycles
            return state.Memory[state.Registers.PC + 1];
        }
    }
}
