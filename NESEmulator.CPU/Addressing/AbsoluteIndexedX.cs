using System;

namespace NESEmulator.CPU.Addressing
{
    /**
     * Absolute indexed addressing crosses a page boundary if the high order byte changes
     * when the index register is added to the base address.
     *
     * In hardware, this requires another signal to process with the ADDER.
     */
    public class AbsoluteIndexedX : IAddressingMode
    {
        public (byte, bool) Get(State state)
        {
            var baseLocation = state.Memory[state.Registers.PC + 1] + 256 * state.Memory[state.Registers.PC + 2];
            var finalLocation = baseLocation + state.Registers.X;

            return (state.Memory[finalLocation], baseLocation / 256 != finalLocation / 256);
        }
    }
}
