namespace NESEmulator.CPU.Addressing
{
    /**
     * Absolute indexed addressing crosses a page boundary if the high order byte changes
     * when the index register is added to the base address.
     *
     * In hardware, this requires another signal to process with the ADDER.
     *
     * Note that the PC has a dedicated 16 bit incrementer to ensure it takes one cycle
     * to increment PC: http://visual6502.org/wiki/index.php?title=6502_increment_PC_control
     */
    public class AbsoluteIndexedY : IAddressingMode
    {
        public static AbsoluteIndexedY Instance = new AbsoluteIndexedY();

        public int StandardCpuCycles { get; } = 5;

        public (ushort, bool) GetAddress(State state)
        {
            var baseLocation = state.Memory[state.Registers.PC + 1] + 256 * state.Memory[state.Registers.PC + 2];
            var finalLocation = baseLocation + state.Registers.Y;

            return ((ushort)finalLocation, baseLocation / 256 == finalLocation / 256);
        }
    }
}
