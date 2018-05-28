namespace NESEmulator.CPU.Addressing
{
    public class Immediate : IAddressingMode
    {
        public (ushort, bool) GetAddress(State state)
        {
            // Expected 2 cycles

            // Note that the constant value is in memory within the program code, so it is just offset from the program counter
            return ((ushort)(state.Registers.PC + 1), false);
        }
    }
}
