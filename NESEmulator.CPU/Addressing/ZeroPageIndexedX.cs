using System;
using System.Collections.Generic;
using System.Text;

namespace NESEmulator.CPU.Addressing
{
    public class ZeroPageIndexedX : IAddressingMode
    {
        public byte Get(State state)
        {
            // See page 81 of the Synertek manual. Zero page indexed wraps around and so stays
            // in page zero.
            var location = (state.Memory[state.Registers.PC + 1] + state.Registers.X) % 256;
            return state.Memory[location];
        }
    }
}
