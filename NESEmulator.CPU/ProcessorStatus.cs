namespace NESEmulator.CPU
{
    public class ProcessorStatus
    {
        // C = bit 0
        public bool Carry { get; set; }

        // Z = bit 1
        public bool ZeroResult { get; set; }

        // I = bit 2
        // Initialised to 1. See https://stackoverflow.com/questions/16913423/why-is-the-initial-state-of-the-interrupt-flag-of-the-6502-a-1
        // for an explanation.
        public bool InterruptDisable { get; set; } = true;

        // D = bit 3
        public bool DecimalMode { get; set; }

        // B = bit 4
        public bool Break { get; set; }

        // bit 5 = expansion - no guarantees on its value.
        // It was going to be used in a future expansion of the micro processor
        // It is initialised to 1 in the 6502.

        // V = bit 6
        public bool Overflow { get; set; }

        // N = bit 7
        public bool NegativeResult { get; set; }
    }
}
