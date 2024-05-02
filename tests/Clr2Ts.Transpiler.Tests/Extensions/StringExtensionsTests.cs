﻿using Clr2Ts.Transpiler.Extensions;
using Xunit;

namespace Clr2Ts.Transpiler.Tests.Extensions
{
    /// <summary>
    /// Tests for extension methods for instances of <see cref="string"/>.
    /// </summary>
    public class StringExtensionsTests
    {
        /// <summary>
        /// ToCamelCase should normalize null or whitespace to an empty string.
        /// </summary>
        [Fact]
        public void StringExtensions_ToCamelCase_NormalizesEmptyInput()
        {
            Assert.Equal(string.Empty, ((string)null!).ToCamelCase());
            Assert.Equal(string.Empty, string.Empty.ToCamelCase());
            Assert.Equal(string.Empty, "   ".ToCamelCase());
        }

        /// <summary>
        /// ToCamelCase should make the first letter lowercase,
        /// assuming the input was in PascalCase format.
        /// </summary>
        [Fact]
        public void StringExtensions_ToCamelCase_MakesFirstLetterLowerCase()
        {
            Assert.Equal("x", "X".ToCamelCase());
            Assert.Equal("someProperty", "SomeProperty".ToCamelCase());
        }

        /// <summary>
        /// ToCamelCase should detect patterns with a screaming caps prefix
        /// (or all uppercase names), like "ID" (=> "id"), "NR" (=> "nr")
        /// or "IDSomething" (=> "idSomething")
        /// </summary>
        [Fact]
        public void StringExtensions_ToCamelCase_HandlesUpperCasePrefix()
        {
            Assert.Equal("id", "ID".ToCamelCase());
            Assert.Equal("nr", "NR".ToCamelCase());
            Assert.Equal("idSomething", "IDSomething".ToCamelCase());
            Assert.Equal("nrSomethingElse", "NRSomethingElse".ToCamelCase());
            Assert.Equal("alluppercase", "ALLUPPERCASE".ToCamelCase());
            Assert.Equal("nId", "NId".ToCamelCase());
        }

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
            Assert.Equal(string.Empty, ((string)null!).AddIndentation());
        }

        /// <summary>
        /// AddIndentation should indent all lines by the specified
        /// number of tabs.
        /// </summary>
        [Fact]
        public void StringExtensions_AddIndentation_IndentsLines()
        {
            var expectedIndentation = "\t\t";
            var expected = string.Join(
                Environment.NewLine,
                new[] {
                    $"{expectedIndentation}Line 1",
                    $"{expectedIndentation}Line 2",
                    $"{expectedIndentation}Line 3"
                }
            );

            var actual = string.Join(
                                   Environment.NewLine,
                                   new[] {
                                       "Line 1",
                                       "Line 2",
                                       "Line 3"
                                   }
                               )
                               .AddIndentation(2);

            Assert.Equal(actual, expected);
        }

        /// <summary>
        /// AddIndentation should not touch empty lines (not insert tabs).
        /// </summary>
        [Fact]
        public void StringExtensions_AddIndentation_KeepsEmptyLines()
        {
            var expectedIndentation = "\t\t";
            var expected = string.Join(
                Environment.NewLine,
                new[] {
                    $"{expectedIndentation}Line 1",
                    $"",
                    $"{expectedIndentation}Line 3"
                }
            );

            var actual = string.Join(
                                   Environment.NewLine,
                                   new[] {
                                       "Line 1",
                                       "",
                                       "Line 3"
                                   }
                               )
                               .AddIndentation(2);

            Assert.Equal(actual, expected);
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
            var expected = @"""Hello"", ""World""!";

            var context = new Dictionary<string, object> {
                { "Tuple1", Tuple.Create("Hello") },
                { "Tuple2", Tuple.Create("", "World") }
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
            var expected = @"{{\{""Hello"", {\{ ""World""!";

            var context = new Dictionary<string, object> {
                { "Tuple1", Tuple.Create("Hello") },
                { "Tuple2", Tuple.Create("", "World") }
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
            var expected = @"""Hello"", ""World""!";

            var context = new Dictionary<string, object> {
                { "Tuple1", Tuple.Create("Hello") },
                { "Tuple2", Tuple.Create("", "World") }
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
            var expected = @"""Hello"", ""World""!";

            var context = new Dictionary<string, object> {
                { "Tuple1", Tuple.Create(Tuple.Create("Hello")) },
                { "Tuple2", Tuple.Create("", Tuple.Create("World")) }
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
            var expected = @"""Hello"", null!";

            var context = new Dictionary<string, object> {
                { "Tuple1", Tuple.Create(Tuple.Create("Hello")) },
                { "Tuple2", Tuple.Create("", (object)null!) }
            };

            Assert.Equal(expected, input.FormatWith(context));
        }

        /// <summary>
        /// FormatWith allows lookups of dictionary values with the dot notation.
        /// </summary>
        [Fact]
        public void StringExtensions_FormatWith_AllowsDictionaryKeyLookups()
        {
            // Includes a test case where the value in the dictionary is another object.
            var input = "{ Tuple1.Item1.SomeKey }, { Tuple1.Item1.OtherKey.Item1 }!";
            var expected = @"""Hello"", ""World""!";

            var context = new Dictionary<string, object> {
                {
                    "Tuple1", Tuple.Create(
                        new Dictionary<string, object> {
                            { "SomeKey", "Hello" },
                            { "OtherKey", Tuple.Create("World") }
                        }
                    )
                }
            };

            Assert.Equal(expected, input.FormatWith(context));
        }

        [Fact]
        public void StringExtensions_FormatWith_SupportsPlaceholderFormatting_Camelcase()
        {
            var input = "{ Tuple1.Item1.Item1 : camelCase }, { Tuple2.Item2.Item1 : camelCase}!";
            var expected = @"""helloHello"", ""worldWorld""!";

            var context = new Dictionary<string, object> {
                { "Tuple1", Tuple.Create(Tuple.Create("HelloHello")) },
                { "Tuple2", Tuple.Create("", Tuple.Create("WorldWorld")) }
            };

            Assert.Equal(expected, input.FormatWith(context));
        }

        /// <summary>
        /// FormatWith allows specifying method names to invoke them
        /// (only parameterless for now).
        /// </summary>
        [Fact]
        public void StringExtensions_FormatWith_AllowMethodCall()
        {
            var input = "{ MyString.GetType.Name }";
            var expected = @"""String""";

            var context = new Dictionary<string, object> { { "MyString", "Hello, World!" } };

            Assert.Equal(expected, input.FormatWith(context));
        }

        /// <summary>
        /// FormatWith offers custom operators to include in the
        /// resolved path (here: UnderlyingTypeIfNullable).
        /// </summary>
        [Fact]
        public void StringExtensions_FormatWith_CustomOperator_UnderlyingTypeIfNullable()
        {
            var input = "{ NullableInt.GetType.UnderlyingTypeIfNullable.Name }, { NonNullableInt.GetType.UnderlyingTypeIfNullable.Name }";
            var expected = @"""Int32"", ""Int32""";

            var context = new Dictionary<string, object> {
                { "NullableInt", (int?)5 },
                { "NonNullableInt", 5 }
            };

            Assert.Equal(expected, input.FormatWith(context));
        }
    }
}