using Clr2Ts.Transpiler.Extensions;
using System;
using Xunit;

namespace Clr2Ts.Transpiler.Tests.Extensions
{
    /// <summary>
    /// Tests for extension methods for instances of <see cref="string"/>.
    /// </summary>
    public class StringExtensionsTests
    {
        /// <summary>
        /// AddIndentation should throw an <see cref="ArgumentOutOfRangeException"/>
        /// when the provided level is negative.
        /// </summary>
        [Fact]
        public void StringExtensions_AddIndentation_ThrowsArgumentException_WhenLevelNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => "Test".AddIndentation(-1));
        }

        /// <summary>
        /// AddIndentation should not alter the string for an indentation level of zero.
        /// </summary>
        [Fact]
        public void StringExtensions_AddIndentation_ReturnsOriginal_WhenLevelZero()
        {
            Assert.Equal("Some Test Text", "Some Test Text".AddIndentation(0));
        }

        /// <summary>
        /// AddIndentation should normalize empty cases (null, empty, whitespace) to <see cref="string.Empty"/>.
        /// </summary>
        [Fact]
        public void StringExtensions_AddIndentation_NormalizesNullValues()
        {
            Assert.Equal(string.Empty, ((string)null).AddIndentation());
        }
        
        /// <summary>
        /// AddIndentation should indent all lines by the specified
        /// number of tabs.
        /// </summary>
        [Fact]
        public void StringExtensions_AddIndentation_IndentsLines()
        {
            var expectedIndentation = "\t\t";
            Assert.Equal($@"{expectedIndentation}Line 1
{expectedIndentation}Line 2
{expectedIndentation}Line 3", @"Line 1
Line 2
Line 3".AddIndentation(2));
        }

        /// <summary>
        /// AddIndentation should not touch empty lines (not insert tabs).
        /// </summary>
        [Fact]
        public void StringExtensions_AddIndentation_KeepsEmptyLines()
        {
            var expectedIndentation = "\t\t";
            Assert.Equal($@"{expectedIndentation}Line 1

{expectedIndentation}Line 3", @"Line 1

Line 3".AddIndentation(2));
        }
    }
}