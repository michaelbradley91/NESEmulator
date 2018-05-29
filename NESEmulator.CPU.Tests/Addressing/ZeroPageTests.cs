using FluentAssertions;
using NESEmulator.CPU.Addressing;
using NUnit.Framework;

namespace NESEmulator.CPU.Tests.Addressing
{
    [TestFixture]
    public class ZeroPageTests
    {
        private State _state;
        private ZeroPage _zeroPage;

        [SetUp]
        public void SetUp()
        {
            _state = new State();
            _zeroPage = new ZeroPage();
        }

        [Test]
        public void GetsTheCorrectAddress()
        {
            _state.Registers.PC = 0x15F0;
            _state.Memory[0x15F1] = 0x12;

            var (address, canSkipCycle) = _zeroPage.GetAddress(_state);

            address.Should().Be(0x12);
            canSkipCycle.Should().BeFalse();
        }
    }
}
