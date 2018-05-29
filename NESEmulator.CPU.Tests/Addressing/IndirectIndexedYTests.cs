using FluentAssertions;
using NESEmulator.CPU.Addressing;
using NUnit.Framework;

namespace NESEmulator.CPU.Tests.Addressing
{
    [TestFixture]
    public class IndirectIndexedYTests
    {
        private State _state;
        private IndirectIndexedY _indirectIndexedY;

        [SetUp]
        public void SetUp()
        {
            _state = new State();
            _indirectIndexedY = new IndirectIndexedY();
        }

        [Test]
        public void WhenTheIndexIsZero_ResolvesTheCorrectAddress_AndIndicatesACycleCanBeSkipped()
        {
            _state.Registers.PC = 0x0072;
            _state.Registers.Y = 0;
            _state.Memory[0x0073] = 0xB4;
            _state.Memory[0x00B4] = 0xC2;
            _state.Memory[0x00B5] = 0x8D;

            var (address, canSkipCycle) = _indirectIndexedY.GetAddress(_state);

            address.Should().Be(0x8DC2);
            canSkipCycle.Should().BeTrue();
        }

        [Test]
        public void WhenTheIndexDoesNotCrossAPageBoundary_ResolvesTheCorrectAddress_AndIndicatesACycleCanBeSkipped()
        {
            _state.Registers.PC = 0x0072;
            _state.Registers.Y = 0x05;
            _state.Memory[0x0073] = 0xB4;
            _state.Memory[0x00B4] = 0xC2;
            _state.Memory[0x00B5] = 0x8D;

            var (address, canSkipCycle) = _indirectIndexedY.GetAddress(_state);

            address.Should().Be(0x8DC7);
            canSkipCycle.Should().BeTrue();
        }

        [Test]
        public void WhenTheIndexDoesCrossAPageBoundary_ResolvesTheCorrectAddress_AndIndicatesACycleCannotBeSkipped()
        {
            _state.Registers.PC = 0x0072;
            _state.Registers.Y = 0xF0;
            _state.Memory[0x0073] = 0xB4;
            _state.Memory[0x00B4] = 0xC2;
            _state.Memory[0x00B5] = 0x8D;

            var (address, canSkipCycle) = _indirectIndexedY.GetAddress(_state);

            address.Should().Be(0x8EB2);
            canSkipCycle.Should().BeFalse();
        }

        [Test]
        public void WhenTheBaseAddressEquals255_ResolvesTheCorrectAddress_AfterWrappingTheHighOrderByte()
        {
            _state.Registers.PC = 0x0072;
            _state.Registers.Y = 0xF0;
            _state.Memory[0x0073] = 0xFF;
            _state.Memory[0x00FF] = 0xC2;
            _state.Memory[0x0000] = 0x8D;

            var (address, canSkipCycle) = _indirectIndexedY.GetAddress(_state);

            address.Should().Be(0x8EB2);
            canSkipCycle.Should().BeFalse();
        }
    }
}
