using System;

namespace NESEmulator.CPU.Addressing
{
    public class Relative : IAddressingMode
    {
        public static Relative Instance = new Relative();

        /**
         * This is the number of cycles if the branch is taken and there
         * is a page boundary crossing. If a page boundary is NOT crossed, deduct 1.
         * If the branch is not taken, deduct 2.
         */
        public int StandardCpuCycles { get; } = 4;

        public (ushort, bool) GetAddress(State state)
        {
            var offset = state.Memory[state.Registers.PC + 1];

            // We want to add the relative offset from where the PC ends up, so
            // it is already add the next instruction (there is no need to further increment it)
            var baseLocation = state.Registers.PC + 2;
            var newLocation = baseLocation + (sbyte) offset;

            return ((ushort)newLocation, baseLocation / 256 == newLocation / 256);
        }
    }
}
