using System;

namespace NESEmulator.CPU
{
    /// <summary>
    /// The NES CPU is a 6502 processor. An instruction manual detailing
    /// how all of its opcodes work can be found in the repository.
    /// 
    /// A key to understanding how to translate opcodes is to understand
    /// how the assembly instructions work.
    /// 
    /// An incredibly helpful site to compare your implementation against
    /// can be found here: https://skilldrick.github.io/easy6502/
    /// </summary>
    public class CPU
    {
        public State State { get; } = new State();
    }
}
