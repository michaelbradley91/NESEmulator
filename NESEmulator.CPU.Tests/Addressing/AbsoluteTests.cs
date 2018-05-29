using FluentAssertions;
using NESEmulator.CPU.Addressing;
using NUnit.Framework;

namespace NESEmulator.CPU.Tests.Addressing
{
    [TestFixture]
    public class AbsoluteTests
    {
        private State _state;
        private Absolute _absolute;

        [SetUp]
        public void SetUp()
        {
            _state = new State();
            _absolute = new Absolute();
        }

        [Test]
        public void GetsTheCorrectAddress()
        {
            // Arrange
            _state.Registers.PC = 0x0020;
            _state.Memory[0x0021] = 0x40;
            _state.Memory[0x0022] = 0xF0;

            // Act
            var (address, canSkipCycle) = _absolute.GetAddress(_state);

            // Assert
            address.Should().Be(0xF040);
            canSkipCycle.Should().BeFalse();
        }
    }
}
