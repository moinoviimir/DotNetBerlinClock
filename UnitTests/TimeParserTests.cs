using BerlinClock.Classes;
using BerlinClock.Classes.TimeParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FluentAssertions;

namespace BerlinClock.UnitTests
{
    [TestClass]
    public class TimeParserTests
    {
        [TestClass]
        public class ParseTests
        {
            private TimeParser _sut;

            [TestInitialize]
            public void Initialize()
            {
                _sut = new TimeParser();
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void WillThrow_OnGarbageInput()
            {
                _sut.Parse("hello world");
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void WillThrow_OnNegativeTimestamp()
            {
                _sut.Parse("-01:00:00");
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void WillThrow_OnTooManyMinutesTimestamp()
            {
                _sut.Parse("01:61:00");
            }

            private static readonly LampTimeModel TestResult = new LampTimeModel(
                false,
                new[] { true, false, false, false },
                new[] { true, false, false, false },
                new[] { true, true, false, false, false, false, false, false, false, false, false },
                new[] { true, false, false, false }
            );

            [TestMethod]
            public void ReturnsResult_OnValidTimestamp()
            {
                _sut.Parse("06:11:01").Should().BeEquivalentTo(TestResult);
            }
        }
    }
}
