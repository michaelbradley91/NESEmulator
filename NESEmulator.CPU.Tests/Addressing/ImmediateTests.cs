using FluentAssertions;
using NESEmulator.CPU.Addressing;
using NUnit.Framework;

namespace NESEmulator.CPU.Tests.Addressing
{
    [TestFixture]
    public class ImmediateTests
    {
        private State _state;
        private Immediate _immediate;

        [SetUp]
        public void SetUp()
        {
            _state = new State();
            _immediate = new Immediate();
        }

        [Test]
        public void GetsTheCorrectAddress()
        {
            // Arrange
            _state.Registers.PC = 0x0020;

            // Act
            var (address, canSkipCycle) = _immediate.GetAddress(_state);

            // Assert - in immediate addressing, the location of the value is just right after the PC
            address.Should().Be(0x0021);
            canSkipCycle.Should().BeFalse();
        }
    }
}
