namespace NESEmulator.CPU.Addressing
{
    public class Immediate : IAddressingMode
    {
        public static Immediate Instance = new Immediate();

        public int StandardCpuCycles { get; } = 2;

        public (ushort, bool) GetAddress(State state)
        {
            // Note that the constant value is in memory within the program code, so it is just offset from the program counter
            return ((ushort)(state.Registers.PC + 1), false);
        }
    }
}
