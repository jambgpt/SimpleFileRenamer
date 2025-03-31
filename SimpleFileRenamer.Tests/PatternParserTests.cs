using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleFileRenamer.Core;
using SimpleFileRenamer.Models;

namespace SimpleFileRenamer.Tests
{
    [TestClass]
    public class PatternParserTests
    {
        private PatternParser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new PatternParser();
        }

        [TestMethod]
        public void ValidatePattern_NullPattern_ReturnsInvalid()
        {
            // Arrange & Act
            var result = _parser.ValidatePattern(null);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Pattern is null", result.ErrorMessage);
        }

        [TestMethod]
        public void ValidatePattern_EmptyPattern_ReturnsInvalid()
        {
            // Arrange
            var pattern = new RenamePattern();

            // Act
            var result = _parser.ValidatePattern(pattern);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Pattern does not make any changes", result.ErrorMessage);
        }

        [TestMethod]
        public void ValidatePattern_ValidPrefix_ReturnsValid()
        {
            // Arrange
            var pattern = new RenamePattern { Prefix = "Test_" };

            // Act
            var result = _parser.ValidatePattern(pattern);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.ErrorMessage);
        }

        [TestMethod]
        public void ValidatePattern_ValidSuffix_ReturnsValid()
        {
            // Arrange
            var pattern = new RenamePattern { Suffix = "_Test" };

            // Act
            var result = _parser.ValidatePattern(pattern);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.ErrorMessage);
        }

        [TestMethod]
        public void ValidatePattern_ValidFindReplace_ReturnsValid()
        {
            // Arrange
            var pattern = new RenamePattern { FindText = "Old", ReplaceText = "New" };

            // Act
            var result = _parser.ValidatePattern(pattern);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.ErrorMessage);
        }

        [TestMethod]
        public void ValidatePattern_SameFindReplace_ReturnsInvalid()
        {
            // Arrange
            var pattern = new RenamePattern { FindText = "Same", ReplaceText = "Same" };

            // Act
            var result = _parser.ValidatePattern(pattern);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Pattern does not make any changes", result.ErrorMessage);
        }

        [TestMethod]
        public void ValidatePattern_ValidRegex_ReturnsValid()
        {
            // Arrange
            var pattern = new RenamePattern { FindText = "[0-9]+", ReplaceText = "Num", UseRegex = true };

            // Act
            var result = _parser.ValidatePattern(pattern);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.ErrorMessage);
        }

        [TestMethod]
        public void ValidatePattern_InvalidRegex_ReturnsInvalid()
        {
            // Arrange
            var pattern = new RenamePattern { FindText = "[0-9", ReplaceText = "Num", UseRegex = true };

            // Act
            var result = _parser.ValidatePattern(pattern);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Invalid regular expression"));
        }

        [TestMethod]
        public void ValidatePattern_ValidSequence_ReturnsValid()
        {
            // Arrange
            var pattern = new RenamePattern
            {
                UseSequence = true,
                SequenceStart = 1,
                SequenceIncrement = 1,
                SequenceFormat = "000"
            };

            // Act
            var result = _parser.ValidatePattern(pattern);

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.ErrorMessage);
        }

        [TestMethod]
        public void ValidatePattern_InvalidSequenceFormat_ReturnsInvalid()
        {
            // Arrange
            var pattern = new RenamePattern
            {
                UseSequence = true,
                SequenceStart = 1,
                SequenceIncrement = 1,
                SequenceFormat = "invalid"
            };

            // Act
            var result = _parser.ValidatePattern(pattern);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("Invalid sequence format"));
        }
    }
}
