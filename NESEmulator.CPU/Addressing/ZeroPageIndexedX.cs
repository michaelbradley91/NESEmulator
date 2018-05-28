namespace NESEmulator.CPU.Addressing
{
    public class ZeroPageIndexedX : IAddressingMode
    {
        public (ushort, bool) GetAddress(State state)
        {
            // See page 81 of the Synertek manual. Zero page indexed wraps around and so stays
            // in page zero.
            var location = (state.Memory[state.Registers.PC + 1] + state.Registers.X) % 256;
            return ((ushort)location, false);
        }
    }
}
