using System;

namespace NESEmulator.CPU.Addressing
{
    public class Relative : IAddressingMode
    {
        public (ushort, bool) GetAddress(State state)
        {
            var offset = state.Memory[state.Registers.PC + 1];

            // We want to add the relative offset from where the PC ends up, so
            // it is already add the next instruction (there is no need to further increment it)
            var baseLocation = state.Registers.PC + 2;
            var newLocation = baseLocation + (sbyte) offset;

            return ((ushort)newLocation, baseLocation / 256 != newLocation / 256);
        }
    }
}
