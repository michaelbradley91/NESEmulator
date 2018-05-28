namespace NESEmulator.CPU.Addressing
{
    public class Absolute : IAddressingMode
    {
        public static Absolute Instance = new Absolute();

        public int StandardCpuCycles { get; } = 4;

        public (ushort, bool) GetAddress(State state)
        {
            // Expected 4 cycles - Jump Absolute only exception taking 3 cycles.
            var location = state.Memory[state.Registers.PC + 1] + 256 * state.Memory[state.Registers.PC + 2];
            return ((ushort)location, false);
        }
    }
}
