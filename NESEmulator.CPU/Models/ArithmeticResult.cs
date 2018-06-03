namespace NESEmulator.CPU.Models
{
    public class ArithmeticResult
    {
        public ArithmeticResult(bool overflowed, bool carried, bool zeroed, bool negative, byte result)
        {
            Overflowed = overflowed;
            Carried = carried;
            Zeroed = zeroed;
            Negative = negative;
            Result = result;
        }
        
        public bool Overflowed { get; }
        public bool Carried { get; }
        public bool Zeroed { get; }
        public bool Negative { get; }
        public byte Result { get; }
    }
}
