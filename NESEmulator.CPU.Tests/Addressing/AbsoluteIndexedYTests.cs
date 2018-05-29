using FluentAssertions;
using NESEmulator.CPU.Addressing;
using NUnit.Framework;

namespace NESEmulator.CPU.Tests.Addressing
{
    [TestFixture]
    public class AbsoluteIndexedYTests
    {
        private State _state;
        private AbsoluteIndexedY _absoluteIndexedY;

        [SetUp]
        public void SetUp()
        {
            _state = new State();
            _absoluteIndexedY = new AbsoluteIndexedY();
        }

        [Test]
        public void WhenTheIndexIsZero_GetsTheCorrectAddress_AndIndicatesACycleCanBeSkipped()
        {
            // Arrange
            _state.Registers.PC = 0x0020;
            _state.Memory[0x0021] = 0x40;
            _state.Memory[0x0022] = 0xF0;

            // Act
            var (address, canSkipCycle) = _absoluteIndexedY.GetAddress(_state);

            // Assert
            address.Should().Be(0xF040);
            canSkipCycle.Should().BeTrue();
        }

        [Test]
        public void WhenTheIndexDoesNotCrossAPageBoundary_GetsTheCorrectAddress_AndIndicatesACycleCanBeSkipped()
        {
            // Arrange
            _state.Registers.PC = 0x0020;
            _state.Registers.Y = 0x40;
            _state.Memory[0x0021] = 0x40;
            _state.Memory[0x0022] = 0xF0;

            // Act
            var (address, canSkipCycle) = _absoluteIndexedY.GetAddress(_state);

            // Assert
            address.Should().Be(0xF080);
            canSkipCycle.Should().BeTrue();
        }

        [Test]
        public void WhenTheIndexDoesCrossAPageBoundary_GetsTheCorrectAddress_AndIndicatesACycleCannotBeSkipped()
        {
            // Arrange
            _state.Registers.PC = 0x0020;
            _state.Registers.Y = 0xF0;
            _state.Memory[0x0021] = 0x40;
            _state.Memory[0x0022] = 0xF0;

            // Act
            var (address, canSkipCycle) = _absoluteIndexedY.GetAddress(_state);

            // Assert
            address.Should().Be(0xF130);
            canSkipCycle.Should().BeFalse();
        }
    }
}
