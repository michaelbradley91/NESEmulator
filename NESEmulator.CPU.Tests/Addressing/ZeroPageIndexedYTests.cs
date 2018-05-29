using FluentAssertions;
using NESEmulator.CPU.Addressing;
using NUnit.Framework;

namespace NESEmulator.CPU.Tests.Addressing
{
    [TestFixture]
    public class ZeroPageIndexedYTests
    {
        private State _state;
        private ZeroPageIndexedY _zeroPageIndexedY;

        [SetUp]
        public void SetUp()
        {
            _state = new State();
            _zeroPageIndexedY = new ZeroPageIndexedY();
        }

        [Test]
        public void WhenTheIndexIsZero_GetsTheCorrectAddress()
        {
            _state.Registers.PC = 0x15F0;
            _state.Registers.Y = 0x00;
            _state.Memory[0x15F1] = 0x12;

            var (address, canSkipCycle) = _zeroPageIndexedY.GetAddress(_state);

            address.Should().Be(0x12);
            canSkipCycle.Should().BeFalse();
        }

        [Test]
        public void WhenTheIndexPlusTheBaseAddressDoesNotExceed255_GetsTheCorrectAddress()
        {
            _state.Registers.PC = 0x15F0;
            _state.Registers.Y = 0x80;
            _state.Memory[0x15F1] = 0x12;

            var (address, canSkipCycle) = _zeroPageIndexedY.GetAddress(_state);

            address.Should().Be(0x92);
            canSkipCycle.Should().BeFalse();
        }

        [Test]
        public void WhenTheIndexPlusTheBaseAddressDoesExceed255_GetsTheCorrectAddress_WrappingAroundTheResult()
        {
            _state.Registers.PC = 0x15F0;
            _state.Registers.Y = 0xF0;
            _state.Memory[0x15F1] = 0x12;

            var (address, canSkipCycle) = _zeroPageIndexedY.GetAddress(_state);

            address.Should().Be(0x02);
            canSkipCycle.Should().BeFalse();
        }

        [Test]
        public void WhenTheIndexPlusTheBaseAddressEquals255_GetsTheCorrectAddress()
        {
            _state.Registers.PC = 0x15F0;
            _state.Registers.Y = 0xED;
            _state.Memory[0x15F1] = 0x12;

            var (address, canSkipCycle) = _zeroPageIndexedY.GetAddress(_state);

            address.Should().Be(0xFF);
            canSkipCycle.Should().BeFalse();
        }
    }
}
