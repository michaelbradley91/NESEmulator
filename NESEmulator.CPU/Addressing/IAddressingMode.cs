namespace NESEmulator.CPU.Addressing
{
    /**
     * See chapters 5 and 6 in the Synertek manual in this repo for very high levels of detail
     * on the addressing modes.
     */
    public interface IAddressingMode
    {
        /**
         * Get the "real" address according to this addressing mode.
         *
         * The first byte for addressing will be at pc + 1, and the second byte for addressing,
         * if needed, will be at pc + 2.
         *
         * If a page boundary is crossed by the address operation, the boolean
         * flag returned will be true. This indicates that one cycle should be added
         * to the number of cycles required by the operation.
         */
        (ushort, bool) GetAddress(State state);

        /*
         * 3 byte instruction requires 3 clock cycles
         * 2 byte instruction requires 2 clock cycles
         */
    }
}
