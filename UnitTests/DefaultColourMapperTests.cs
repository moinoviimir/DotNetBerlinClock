using BerlinClock.Classes;
using BerlinClock.Classes.ColourMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BerlinClock.UnitTests
{
    [TestClass]
    public class DefaultColourMapperTests
    {
        [TestClass]
        public class MapToLampStringTests
        {
            private DefaultColourMapper _sut;

            [TestInitialize]
            public void Setup()
            {
                _sut = new DefaultColourMapper();
            }

            private static readonly LampTimeModel SanityCheckSource = new LampTimeModel(
                false,
                new[] { true, true, false, false },
                new[] { true, true, true, true },
                new[] { true, true, true, true, true, true, true, true, false, false, false, false },
                new[] { false, false, false, false }
                );

            private static readonly string SanityCheckResult = string.Join(Environment.NewLine, new[] {
                "O",
                "RROO",
                "RRRR",
                "YYRYYRYYOOOO",
                "OOOO"
                });

            [TestMethod]
            public void ColoursAreMappedProperly()
            {
                _sut.MapToLampString(SanityCheckSource).Should().Be(SanityCheckResult);
            }
        }
    }
}
