using FluentAssertions;
using NESEmulator.CPU.Addressing;
using NUnit.Framework;

namespace NESEmulator.CPU.Tests.Addressing
{
    [TestFixture]
    public class IndirectAbsoluteTests
    {
        private State _state;
        private IndirectAbsolute _indirectAbsolute;

        [SetUp]
        public void SetUp()
        {
            _state = new State();
            _indirectAbsolute = new IndirectAbsolute();
        }

        [Test]
        public void WhenTheIndirectAddressDoesNotCrossAPageBoundary_ResolvesTheCorrectAddress()
        {
            _state.Registers.PC = 0x0045;
            _state.Memory[0x0046] = 0xD1;
            _state.Memory[0x0047] = 0x45;
            _state.Memory[0x45D1] = 0xC8;
            _state.Memory[0x45D2] = 0xF3;

            var (address, canSkipCycle) = _indirectAbsolute.GetAddress(_state);

            address.Should().Be(0xF3C8);
            canSkipCycle.Should().BeFalse();
        }

        [Test]
        public void WhenTheIndirectAddressDoesCrossAPageBoundary_ResolvesTheCorrectAddress_AfterWrappingForTheHighOrderByte()
        {
            _state.Registers.PC = 0x0045;
            _state.Memory[0x0046] = 0xFF;
            _state.Memory[0x0047] = 0x45;
            _state.Memory[0x45FF] = 0xC8;
            _state.Memory[0x4500] = 0xF3;

            var (address, canSkipCycle) = _indirectAbsolute.GetAddress(_state);

            address.Should().Be(0xF3C8);
            canSkipCycle.Should().BeFalse();
        }
    }
}
