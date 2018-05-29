namespace NESEmulator.CPU.Addressing
{
    public class Absolute : IAddressingMode
    {
        public static Absolute Instance = new Absolute();

        /**
         * Jump absolute only can take 3 cycles.
         */
        public int StandardCpuCycles { get; } = 4;

        public (ushort, bool) GetAddress(State state)
        {
            var location = state.Memory[state.Registers.PC + 1] + 256 * state.Memory[state.Registers.PC + 2];
            return ((ushort)location, false);
        }
    }
}
