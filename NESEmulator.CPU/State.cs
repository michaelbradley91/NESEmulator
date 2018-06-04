namespace NESEmulator.CPU
{
    // TODO conaider allowing a longer stack for unofficial opcodes
    public class State
    {
        /**
         * The 6502 processor is split into 256 pages,
         * each containing 256 bytes (so all locations in a single page
         * can be determined by one byte).
         *
         * The "zero page" of memory is the first 256 bytes (0 - 255).
         *
         * Zero page addressing is where only a single byte is used in an address,
         * which is faster to load. Note that zero page indexed addressing wraps
         * around in the first byte, so always points to the zero page.
         */
        public byte[] Memory { get; } = new byte[65536];

        /**
         * The number of clock cycles that have passed in the CPU.
         *
         * Different operations require different numbers of clock cycles,
         * and it is important to keep track of this to emulate the correct
         * CPU speed.
         *
         * Operations can take longer if a page boundary is crossed.
         * See memory for details on the page structure. A page boundary is crossed
         * if the program counter's high order byte has to be modified.
         *
         * Note that, for the most part, the addressing mode determines the number of cycles.
         * However, the processor parallelises looking at the next instruction while processing
         * the previous instruction when it can. This can cause some operations
         * with the same addressing mode to take a different number of cycles.
         *
         * This site contains an excellent record of the number of cycles required
         * by each operation (TIM). http://www.6502.org/tutorials/6502opcodes.html
         */
        public long ClockCycle { get; set; }

        /**
         * This should be set to true by an instruction
         * that has already adjusted the PC such as a branch operation.
         *
         * This can also be set to true by an operation to effectively halt execution.
         */
        public bool SkipNextPCIncrement { get; set; }

        public Registers Registers { get; } = new Registers();

        public ProcessorStatus Status { get; } = new ProcessorStatus();

        public byte OpCode => Memory[Registers.PC];
    }
}
