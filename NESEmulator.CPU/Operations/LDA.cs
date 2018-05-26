namespace NESEmulator.CPU.Operations
{
    public class LDA : IOperation
    {
        public byte[] Opcodes { get; } = {
            0xA0 + 0x01, // Indexed indirect (d, x)
            0xA0 + 0x05, // Zero page d
            0xA0 + 0x09, // Immediate #i
            0xA0 + 0x0D, // Absolute a
            0xA0 + 0x11, // Indirect Indexed (d), y
            0xA0 + 0x15, // Zero page X d, x
            0xA0 + 0x19, // Absolute, Y a, y
            0xA0 + 0x1D  // Absolute, X a, x
        };

        public void Execute(State state)
        {
            /*
             * Addressing modes to support are:
             * Immediate
             * Absolute
             * Zero Page
             * Absolute, X
             * Absolute, Y
             * Zero Page, X
             * Indexed Indirect
             * Indirect Indexed
             */
        }
    }
}
