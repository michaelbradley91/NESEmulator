using System.Collections.Generic;
using System.Linq;
using NESEmulator.CPU.Addressing;

namespace NESEmulator.CPU.Operations.Bases
{
    public abstract class BranchOperation : IOperation
    {
        private readonly IDictionary<byte, IAddressingMode> _opcodeMap;

        protected BranchOperation(IDictionary<byte, IAddressingMode> opcodeMap)
        {
            _opcodeMap = opcodeMap;
            Opcodes = opcodeMap.Keys.ToArray();
        }

        public byte[] Opcodes { get; }

        public void Execute(State state)
        {
            var addressingMode = _opcodeMap[state.OpCode];
            var (targetAddress, canSkipCycle) = addressingMode.GetAddress(state);

            var value = state.Memory[targetAddress];

            if (BranchCondition(state))
            {
                state.ClockCycle += addressingMode.StandardCpuCycles - (canSkipCycle ? 1 : 0);
                state.Registers.PC = value;

                // The relative addressing mode has already ensured we are pointed at the next instruction we intend to actually execute.
                state.SkipNextPCIncrement = true;
            }
            else
            {
                // No branch taken
                state.ClockCycle += addressingMode.StandardCpuCycles - 2;
            }
        }

        protected abstract bool BranchCondition(State state);
    }
}
