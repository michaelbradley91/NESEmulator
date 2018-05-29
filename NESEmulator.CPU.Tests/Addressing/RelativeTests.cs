using FluentAssertions;
using NESEmulator.CPU.Addressing;
using NUnit.Framework;

namespace NESEmulator.CPU.Tests.Addressing
{
    [TestFixture]
    public class RelativeTests
    {
        private State _state;
        private Relative _relative;

        [SetUp]
        public void SetUp()
        {
            _state = new State();
            _relative = new Relative();
        }

        [Test]
        public void WhenTheRelativeAddressIsPositive_AndDoesNotCrossAPageBoundary_CalculatesTheCorrectAddress_AndIndicatesACycleCanBeSkipped()
        {
            _state.Registers.PC = 0x0081;
            _state.Memory[0x0082] = 0x1F;

            var (address, canSkipCycle) = _relative.GetAddress(_state);

            address.Should().Be(0x00A2);
            canSkipCycle.Should().BeTrue();
        }

        [Test]
        public void WhenTheRelativeAddressIsPositive_AndDoesCrossAPageBoundary_CalculatesTheCorrectAddress_AndIndicatesACycleCannotBeSkipped()
        {
            _state.Registers.PC = 0x00FD;
            _state.Memory[0x00FE] = 0x01;

            var (address, canSkipCycle) = _relative.GetAddress(_state);

            address.Should().Be(0x0100);
            canSkipCycle.Should().BeFalse();
        }

        [Test]
        public void WhenTheRelativeAddressIsNegative_AndDoesNotCrossAPageBoundary_CalculatesTheCorrectAddress_AndIndicatesACycleCanBeSkipped()
        {
            _state.Registers.PC = 0x00FD;
            _state.Memory[0x00FE] = 0xFF;

            var (address, canSkipCycle) = _relative.GetAddress(_state);

            address.Should().Be(0x00FE);
            canSkipCycle.Should().BeTrue();
        }

        [Test]
        public void WhenTheRelativeAddressIsNegative_AndDoesCrossAPageBoundary_CalculatesTheCorrectAddress_AndIndicatesACycleCannotBeSkipped()
        {
            _state.Registers.PC = 0x0460;
            _state.Memory[0x0461] = 0x80;

            var (address, canSkipCycle) = _relative.GetAddress(_state);

            address.Should().Be(0x03E2);
            canSkipCycle.Should().BeFalse();
        }
    }
}
