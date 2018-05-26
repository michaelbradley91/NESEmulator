namespace NESEmulator.CPU.Addressing
{
    public class ZeroPage : IAddressingMode
    {
        public byte Get(State state)
        {
            // Expected 3 cycles.
            var location = state.Memory[state.Registers.PC + 1];
            return state.Memory[location];
        }
    }
}
