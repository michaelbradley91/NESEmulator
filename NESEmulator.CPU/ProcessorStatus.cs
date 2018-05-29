namespace NESEmulator.CPU
{
    public class ProcessorStatus
    {
        // C = bit 0
        public bool Carry { get; set; }

        // Z = bit 1
        public bool ZeroResult { get; set; }

        // I = bit 2
        public bool InterruptDisable { get; set; }

        // D = bit 3
        public bool DecimalMode { get; set; }

        // B = bit 4
        public bool Break { get; set; }

        // bit 5 = expansion - no guarantees on its value.
        // It was going to be used in a future expansion of the micro processor

        // V = bit 6
        public bool Overflow { get; set; }

        // N = bit 7
        public bool NegativeResult { get; set; }
    }
}
