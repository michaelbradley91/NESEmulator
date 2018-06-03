using System;
using System.Diagnostics;
using System.Net.Configuration;
using FluentAssertions;
using NESEmulator.CPU.Helpers;
using NUnit.Framework;

namespace NESEmulator.CPU.Tests.Helpers
{
    [TestFixture]
    public class ArithmeticHelpersTests
    {
        [TestCase(0x00, 0x00, false, 0x00, false, false, true, false)]
        [TestCase(0x79, 0x00, true, 0x80, true, true, false, false)]
        [TestCase(0x24, 0x56, false, 0x80, true, true, false, false)]
        [TestCase(0x93, 0x82, false, 0x75, false, true, false, true)]
        [TestCase(0x89, 0x76, false, 0x65, false, false, false, true)]
        [TestCase(0x89, 0x76, true, 0x66, false, false, true, true)]
        [TestCase(0x80, 0xF0, false, 0xD0, false, true, false, true)]
        [TestCase(0x80, 0xFA, false, 0xE0, true, false, false, true)]
        [TestCase(0x2F, 0x4F, false, 0x74, false, false, false, false)]
        [TestCase(0x6F, 0x00, true, 0x76, false, false, false, false)]
        public void AddDecimal(byte a, byte b, bool inputCarry,
            byte resultValue, bool negative, bool overflow, bool zeroed, bool resultCarry)
        {
            var result = ArithmeticHelpers.AddDecimal(a, b, inputCarry);

            result.Result.Should().Be(resultValue);
            result.Carried.Should().Be(resultCarry);
            result.Overflowed.Should().Be(overflow);
            result.Negative.Should().Be(negative);
            result.Zeroed.Should().Be(zeroed);
        }
    }
}
