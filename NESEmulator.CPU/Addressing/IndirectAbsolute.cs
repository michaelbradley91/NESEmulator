namespace NESEmulator.CPU.Addressing
{
    /**
     * Expected to be used by JMP only.
     */
    public class IndirectAbsolute : IAddressingMode
    {
        public static IndirectAbsolute Instance = new IndirectAbsolute();

        public int StandardCpuCycles { get; } = 5;

        public (ushort, bool) GetAddress(State state)
        {
            /*
             * There is no carry when computing the absolute indirect address
             * of a jump. Therefore, if the first byte is at the end of a page
             * the second byte will be taken from the start of it (not the
             * first byte of the next page)
             *
             * This is nicely explained here: http://www.6502.org/tutorials/6502opcodes.html#BCC
             */
            var indirectLocation = state.Memory[state.Registers.PC + 1] + 256 * state.Memory[state.Registers.PC + 2];
            var highOrderByteOfIndirectLocation = indirectLocation / 256;
            var lowOrderByteOfIndirectLocation = indirectLocation % 256;
            var lowOrderByte = state.Memory[indirectLocation];
            var highOrderByte = state.Memory[(lowOrderByteOfIndirectLocation + 1) % 256 + highOrderByteOfIndirectLocation * 256];
            var targetAddress = lowOrderByte + highOrderByte * 256;

            return ((ushort)targetAddress, false);
        }
    }
}
