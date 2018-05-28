namespace NESEmulator.CPU.Addressing
{
    /**
     * See chapters 5 and 6 in the Synertek manual in this repo for very high levels of detail
     * on the addressing modes.
     */
    public interface IAddressingMode
    {
        /**
         * This is the number of CPU cycles typically.
         *
         * required by this addressing mode. It includes any additional cycle
         * requird by crossing a page boundary. If this could be skipped is indicated
         * by GetAddress.
         */
        int StandardCpuCycles { get; }

        /**
         * Get the "real" address according to this addressing mode.
         *
         * The first byte for addressing will be at pc + 1, and the second byte for addressing,
         * if needed, will be at pc + 2.
         *
         * If a page boundary is crossed by the address operation, the boolean
         * flag returned will be true. This indicates that one cycle could be deducted
         * from the number of cycles required by the operation if the operation
         * can support this. (LDA can but STA cannot. See STA for an explanation)
         */
        (ushort, bool) GetAddress(State state);
    }
}
