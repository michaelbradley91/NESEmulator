using System.Collections.Generic;
using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Operations.Bases;

namespace NESEmulator.CPU.Operations
{
    /**
     * Branches to a relative location from the PC + 2 if the zero flag is set. (Branch on EQual).
     * That is, PC = PC + 2 + (value provided signed). See the Relative addressing mode for details.
     *
     * No flags are set by this operation.
     */
    public class BEQ : BranchOperation
    {
        public static IDictionary<byte, IAddressingMode> OpcodeMap { get; } =
            new Dictionary<byte, IAddressingMode>
            {
                {0xE0 + 0x10, Relative.Instance}, // Relative *+d
            };

        public BEQ() : base(OpcodeMap) { }

        protected override bool BranchCondition(State state)
        {
            return state.Status.ZeroResult;
        }
    }
}
