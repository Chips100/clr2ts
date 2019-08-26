using Clr2Ts.Transpiler.Extensions;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// FormatWith should throw an <see cref="ArgumentNullException"/> when
        /// the context parameter is null.
        /// </summary>
        [Fact]
        public void StringExtensions_FormatWith_ThrowsArgumentNullException_WhenContextIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => "".FormatWith(null));
        }

        /// <summary>
        /// FormatWith should keep empty strings without any changes.
        /// </summary>
        [Fact]
        public void StringExtensions_FormatWith_KeepsEmptyString()
        {
            Assert.Equal(string.Empty, string.Empty.FormatWith(new Dictionary<string, object>()));
        }

        /// <summary>
        /// FormatWith should find template placeholders marked by curly braces
        /// and replace them with values from the provided context.
        /// </summary>
        [Fact]
        public void StringExtensions_FormatWith_InsertsPlaceholdersFromContext()
        {
            var input = "{Tuple1.Item1}, {Tuple2.Item2}!";
            var expected = "Hello, World!";

            var context = new Dictionary<string, object>
            {
                { "Tuple1", Tuple.Create("Hello") },
                { "Tuple2", Tuple.Create("", "World") },
            };

            Assert.Equal(expected, input.FormatWith(context));
        }

        /// <summary>
        /// FormatWith should not match curly braces that have been escaped
        /// by a preceding backslash.
        /// </summary>
        [Fact]
        public void StringExtensions_FormatWith_KeepsEscapedMarkers()
        {
            var input = @"\{\{\\{{Tuple1.Item1}, \{\\{ {Tuple2.Item2}!";
            var expected = @"{{\{Hello, {\{ World!";

            var context = new Dictionary<string, object>
            {
                { "Tuple1", Tuple.Create("Hello") },
                { "Tuple2", Tuple.Create("", "World") },
            };

            Assert.Equal(expected, input.FormatWith(context));
        }

        /// <summary>
        /// FormatWith should allow whitespaces in the placeholders.
        /// </summary>
        [Fact]
        public void StringExtensions_FormatWith_AllowsWhitespaceInPlaceholders()
        {
            var input = "{ Tuple1.Item1 }, { Tuple2.Item2 }!";
            var expected = "Hello, World!";

            var context = new Dictionary<string, object>
            {
                { "Tuple1", Tuple.Create("Hello") },
                { "Tuple2", Tuple.Create("", "World") },
            };

            Assert.Equal(expected, input.FormatWith(context));
        }

        /// <summary>
        /// FormatWith supports multiple levels of depth in the placeholder paths.
        /// </summary>
        [Fact]
        public void StringExtensions_FormatWith_AllowsDeepPaths()
        {
            var input = "{ Tuple1.Item1.Item1 }, { Tuple2.Item2.Item1 }!";
            var expected = "Hello, World!";

            var context = new Dictionary<string, object>
            {
                { "Tuple1", Tuple.Create(Tuple.Create("Hello")) },
                { "Tuple2", Tuple.Create("", Tuple.Create("World")) },
            };

            Assert.Equal(expected, input.FormatWith(context));
        }

        /// <summary>
        /// FormatWith stops evaluating the path when encountering null values.
        /// </summary>
        [Fact]
        public void StringExtensions_FormatWith_AllowsNullValues()
        {
            var input = "{ Tuple1.Item1.Item1 }, { Tuple2.Item2.Item1 }!";
            var expected = "Hello, !";

            var context = new Dictionary<string, object>
            {
                { "Tuple1", Tuple.Create(Tuple.Create("Hello")) },
                { "Tuple2", Tuple.Create("", (object)null) },
            };

            Assert.Equal(expected, input.FormatWith(context));
        }
    }
}