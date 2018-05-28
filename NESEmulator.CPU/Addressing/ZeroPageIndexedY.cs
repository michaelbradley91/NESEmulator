namespace NESEmulator.CPU.Addressing
{
    /**
     * Zero paged indexed Y is only supported by LDX and STX.
     * All other operations must use Zero paged indexed X.
     */
    public class ZeroPageIndexedY : IAddressingMode
    {
        public static ZeroPageIndexedY Instance = new ZeroPageIndexedY();

        public int StandardCpuCycles { get; } = 4;

        public (ushort, bool) GetAddress(State state)
        {
            // See page 81 of the Synertek manual. Zero page indexed wraps around and so stays
            // in page zero.
            var location = (state.Memory[state.Registers.PC + 1] + state.Registers.Y) % 256;
            return ((ushort)location, false);
        }
    }
}
