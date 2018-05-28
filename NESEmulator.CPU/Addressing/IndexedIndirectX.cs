namespace NESEmulator.CPU.Addressing
{
    public class IndexedIndirectX : IAddressingMode
    {
        public (ushort, bool) GetAddress(State state)
        {
            // Note that the indexed indirect address always targets a zero page address
            // so it wraps.
            var targetAddress = (state.Memory[state.Registers.PC + 1] + state.Registers.X) % 256;
            var lowOrderByte = state.Memory[targetAddress];

            // In the event the base target address is 255, do not increment
            // the high order byte of the address, but load the next part from 0000
            // See http://atariage.com/forums/topic/72382-6502-indirect-addressing-ff-behavior/
            var highOrderByte = state.Memory[(targetAddress + 1) % 256];

            return ((ushort)(lowOrderByte + 256 * highOrderByte), false);
        }
    }
}
