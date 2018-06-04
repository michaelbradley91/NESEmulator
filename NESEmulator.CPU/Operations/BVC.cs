using System.Collections.Generic;
using NESEmulator.CPU.Addressing;
using NESEmulator.CPU.Operations.Bases;

namespace NESEmulator.CPU.Operations
{
    /**
     * Branches to a relative location from the PC + 2 if the overflow flag is not set. (Branch on oVerflow Clear).
     * That is, PC = PC + 2 + (value provided signed). See the Relative addressing mode for details.
     *
     * No flags are set by this operation.
     */
    public class BVC : BranchOperation
    {
        public static IDictionary<byte, IAddressingMode> OpcodeMap { get; } =
            new Dictionary<byte, IAddressingMode>
            {
                {0x40 + 0x10, Relative.Instance}, // Relative *+d
            };

        public BVC() : base(OpcodeMap) { }

        protected override bool BranchCondition(State state)
        {
            return state.Status.Overflow;
        }
    }
}
