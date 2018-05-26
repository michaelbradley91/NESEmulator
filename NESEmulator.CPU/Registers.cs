namespace NESEmulator.CPU
{
    public class Registers
    {
        // The accumulator
        public byte A { get; set; }

        // The X index register
        public byte X { get; set; }

        // The Y index register
        public byte Y { get; set; }

        // The stack pointer
        public byte SP { get; set; }

        // The program counter
        public ushort PC { get; set; }

        // The processor status register
        public byte P { get; set; }
    }
}
