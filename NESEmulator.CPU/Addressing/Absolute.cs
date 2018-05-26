namespace NESEmulator.CPU.Addressing
{
    public class Absolute : IAddressingMode
    {
        public (byte, bool) Get(State state)
        {
            // Expected 4 cycles - Jump Absolute only exception taking 3 cycles.
            var location = state.Memory[state.Registers.PC + 1] + 256 * state.Memory[state.Registers.PC + 2];
            return (state.Memory[location], false);
        }
    }
}
