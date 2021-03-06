﻿using System.Collections.Generic;
using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Operations.Bases;

namespace NESEmulator.CPU.Operations
{
    /**
     * Branches to a relative location from the PC + 2 if the overflow flag is set. (Branch on oVerflow Set).
     * That is, PC = PC + 2 + (value provided signed). See the Relative addressing mode for details.
     *
     * No flags are set by this operation.
     */
    public class BVS : BranchOperation
    {
        public static IDictionary<byte, IAddressingMode> OpcodeMap { get; } =
            new Dictionary<byte, IAddressingMode>
            {
                {0x60 + 0x10, Relative.Instance}, // Relative *+d
            };

        public BVS() : base(OpcodeMap) { }

        protected override bool BranchCondition(State state)
        {
            return state.Status.Overflow;
        }
    }
}
