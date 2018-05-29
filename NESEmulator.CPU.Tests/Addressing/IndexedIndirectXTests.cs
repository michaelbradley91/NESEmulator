using FluentAssertions;
using NESEmulator.CPU.Addressing;
using NUnit.Framework;

namespace NESEmulator.CPU.Tests.Addressing
{
    [TestFixture]
    public class IndexedIndirectXTests
    {
        private State _state;
        private IndexedIndirectX _indexedIndirectX;

        [SetUp]
        public void SetUp()
        {
            _state = new State();
            _indexedIndirectX = new IndexedIndirectX();
        }

        [Test]
        public void WhenTheIndexIsZero_ResolvesTheIndirectAddress()
        {
            // Arrange
            _state.Registers.PC = 0x0020;
            _state.Registers.X = 0x00;
            _state.Memory[0x0021] = 0xC0;
            _state.Memory[0x00C0] = 0x45;
            _state.Memory[0x00C1] = 0xF0;

            // Act
            var (address, canSkipCycle) = _indexedIndirectX.GetAddress(_state);

            // Assert
            address.Should().Be(0xF045);
            canSkipCycle.Should().BeFalse();
        }

        [Test]
        public void WhenTheIndexPlusTheConstantDoesNotExceed255_ResolvesTheIndirectAddress()
        {
            // Arrange
            _state.Registers.PC = 0x0020;
            _state.Registers.X = 0x08;
            _state.Memory[0x0021] = 0xC0;
            _state.Memory[0x00C8] = 0x45;
            _state.Memory[0x00C9] = 0xF0;

            // Act
            var (address, canSkipCycle) = _indexedIndirectX.GetAddress(_state);

            // Assert
            address.Should().Be(0xF045);
            canSkipCycle.Should().BeFalse();
        }

        [Test]
        public void WhenTheIndexPlusTheConstantDoesExceed255_ResolvesTheIndirectAddress_AfterWrappingAround()
        {
            // Arrange
            _state.Registers.PC = 0x0020;
            _state.Registers.X = 0xF8;
            _state.Memory[0x0021] = 0xC0;
            _state.Memory[0x00B8] = 0x45;
            _state.Memory[0x00B9] = 0xF0;

            // Act
            var (address, canSkipCycle) = _indexedIndirectX.GetAddress(_state);

            // Assert
            address.Should().Be(0xF045);
            canSkipCycle.Should().BeFalse();
        }

        [Test]
        public void WhenTheIndexPlusTheConstantEquals255_ResolvesTheIndirectAddress_AfterWrappingAroundForTheHighOrderByte()
        {
            // Arrange
            _state.Registers.PC = 0x0020;
            _state.Registers.X = 0x3F;
            _state.Memory[0x0021] = 0xC0;
            _state.Memory[0x00FF] = 0x45;
            _state.Memory[0x0000] = 0xF0;

            // Act
            var (address, canSkipCycle) = _indexedIndirectX.GetAddress(_state);

            // Assert
            address.Should().Be(0xF045);
            canSkipCycle.Should().BeFalse();
        }
    }
}
