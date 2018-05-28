using System;

namespace NESEmulator.CPU.Operations
{
    public class JMP : IOperation
    {
        public byte[] Opcodes { get; }

        public void Execute(State state)
        {
            /*
             * There is no carry when computing the absolute indirect address
             * of a jump. Therefore, if the first byte is at the end of a page
             * the second byte will be taken from the start of it (not the
             * first byte of the next page)
             *
             * This is nicely explained here: http://www.6502.org/tutorials/6502opcodes.html#BCC
             */
            var indirectLocation = state.Memory[state.Registers.PC + 1] + 256 * state.Memory[state.Registers.PC + 2];
            var highOrderByteOfIndirectLocation = indirectLocation / 256;
            var lowOrderByteOfIndirectLocation = indirectLocation % 256;
            var lowOrderByte = state.Memory[indirectLocation];
            var highOrderByte = state.Memory[(lowOrderByteOfIndirectLocation + 1) % 256 + highOrderByteOfIndirectLocation * 256];
            var targetAddress = lowOrderByte + highOrderByte * 256;

            throw new NotImplementedException();
        }
    }
}
