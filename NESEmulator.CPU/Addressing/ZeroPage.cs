namespace NESEmulator.CPU.Addressing
{
    public class ZeroPage : IAddressingMode
    {
        public static ZeroPage Instance = new ZeroPage();

        public int StandardCpuCycles { get; } = 3;

        public (ushort, bool) GetAddress(State state)
        {
            // Expected 3 cycles.
            var location = state.Memory[state.Registers.PC + 1];
            return (location, false);
        }
    }
}
