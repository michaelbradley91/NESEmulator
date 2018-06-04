﻿using System.Collections.Generic;
using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Operations.Bases;

namespace NESEmulator.CPU.Operations
{
    /**
     * Branches to a relative location from the PC + 2 if the negative flag is not set. (Branch if result PLus).
     * That is, PC = PC + 2 + (value provided signed). See the Relative addressing mode for details.
     *
     * No flags are set by this operation.
     */
    public class BPL : BranchOperation
    {
        public static IDictionary<byte, IAddressingMode> OpcodeMap { get; } =
            new Dictionary<byte, IAddressingMode>
            {
                {0x00 + 0x10, Relative.Instance}, // Relative *+d
            };

        public BPL() : base(OpcodeMap) { }

        protected override bool BranchCondition(State state)
        {
            return !state.Status.NegativeResult;
        }
    }
}
