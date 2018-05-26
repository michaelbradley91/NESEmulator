namespace NESEmulator.CPU.Operations
{
    
    public interface IOperation
    {
        /**
         * The opcodes corresponding to this operation.
         *
         * Many operations have a range of addressing modes and each addressing
         * mode constitutes a different opcode.
         */
        byte[] Opcodes { get; }

        /**
         * Execute this opcode based on the current state of the processor,
         * and update the state as appropriate.
         */
        void Execute(State state);
    }
}
