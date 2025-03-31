using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleFileRenamer.Core;
using SimpleFileRenamer.Models;
using System.IO;
using SequencePosition = SimpleFileRenamer.Models.SequencePosition;

namespace SimpleFileRenamer.Tests
{
    [TestClass]
    public class RenamerLogicTests
    {
        private RenamerLogic _renamerLogic;

        [TestInitialize]
        public void Initialize()
        {
            _renamerLogic = new RenamerLogic();
        }

        [TestMethod]
        public void GenerateNewFileName_NullFile_ReturnsError()
        {
            // Arrange
            FileItem file = null;
            var pattern = new RenamePattern();

            // Act
            var result = _renamerLogic.GenerateNewFileName(file, pattern);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual("File is null", result.ErrorMessage);
        }

        [TestMethod]
        public void GenerateNewFileName_NullPattern_ReturnsOriginalName()
        {
            // Arrange
            var file = new FileItem("test.txt");
            RenamePattern pattern = null;

            // Act
            var result = _renamerLogic.GenerateNewFileName(file, pattern);

            // Assert
            Assert.IsFalse(result.HasError);
            Assert.AreEqual("test.txt", result.NewFileName);
        }

        [TestMethod]
        public void GenerateNewFileName_PrefixOnly_ReturnsNameWithPrefix()
        {
            // Arrange
            var file = new FileItem("test.txt");
            var pattern = new RenamePattern { Prefix = "Pre_" };

            // Act
            var result = _renamerLogic.GenerateNewFileName(file, pattern);

            // Assert
            Assert.IsFalse(result.HasError);
            Assert.AreEqual("Pre_test.txt", result.NewFileName);
        }

        [TestMethod]
        public void GenerateNewFileName_SuffixOnly_ReturnsNameWithSuffix()
        {
            // Arrange
            var file = new FileItem("test.txt");
            var pattern = new RenamePattern { Suffix = "_Suffix" };

            // Act
            var result = _renamerLogic.GenerateNewFileName(file, pattern);

            // Assert
            Assert.IsFalse(result.HasError);
            Assert.AreEqual("test_Suffix.txt", result.NewFileName);
        }

        [TestMethod]
        public void GenerateNewFileName_FindReplace_ReturnsUpdatedName()
        {
            // Arrange
            var file = new FileItem("testFile.txt");
            var pattern = new RenamePattern { FindText = "File", ReplaceText = "Document" };

            // Act
            var result = _renamerLogic.GenerateNewFileName(file, pattern);

            // Assert
            Assert.IsFalse(result.HasError);
            Assert.AreEqual("testDocument.txt", result.NewFileName);
        }

        [TestMethod]
        public void GenerateNewFileName_RegexReplace_ReturnsUpdatedName()
        {
            // Arrange
            var file = new FileItem("test123.txt");
            var pattern = new RenamePattern { FindText = "[0-9]+", ReplaceText = "Numbers", UseRegex = true };

            // Act
            var result = _renamerLogic.GenerateNewFileName(file, pattern);

            // Assert
            Assert.IsFalse(result.HasError);
            Assert.AreEqual("testNumbers.txt", result.NewFileName);
        }

        [TestMethod]
        public void GenerateNewFileName_InvalidRegex_ReturnsError()
        {
            // Arrange
            var file = new FileItem("test123.txt");
            var pattern = new RenamePattern { FindText = "[0-9", ReplaceText = "Numbers", UseRegex = true };

            // Act
            var result = _renamerLogic.GenerateNewFileName(file, pattern);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.ErrorMessage.Contains("Invalid regex pattern"));
        }

        [TestMethod]
        public void GenerateNewFileName_SequencePrefix_ReturnsNameWithSequence()
        {
            // Arrange
            var file = new FileItem("test.txt");
            file.Index = 2; // Index 2 would be the 3rd file (0-based)
            var pattern = new RenamePattern
            {
                UseSequence = true,
                SequenceStart = 10,
                SequenceIncrement = 5,
                SequenceFormat = "000",
                SequencePosition = SequencePosition.Prefix
            };

            // Act
            var result = _renamerLogic.GenerateNewFileName(file, pattern);

            // Assert
            Assert.IsFalse(result.HasError);
            Assert.AreEqual("020test.txt", result.NewFileName); // 10 + (2 * 5) = 20
        }

        [TestMethod]
        public void GenerateNewFileName_SequenceSuffix_ReturnsNameWithSequence()
        {
            // Arrange
            var file = new FileItem("test.txt");
            file.Index = 2; // Index 2 would be the 3rd file (0-based)
            var pattern = new RenamePattern
            {
                UseSequence = true,
                SequenceStart = 10,
                SequenceIncrement = 5,
                SequenceFormat = "000",
                SequencePosition = SequencePosition.Suffix
            };

            // Act
            var result = _renamerLogic.GenerateNewFileName(file, pattern);

            // Assert
            Assert.IsFalse(result.HasError);
            Assert.AreEqual("test020.txt", result.NewFileName); // 10 + (2 * 5) = 20
        }

        [TestMethod]
        public void GenerateNewFileName_SequenceReplace_ReturnsSequenceOnly()
        {
            // Arrange
            var file = new FileItem("test.txt");
            file.Index = 2; // Index 2 would be the 3rd file (0-based)
            var pattern = new RenamePattern
            {
                UseSequence = true,
                SequenceStart = 10,
                SequenceIncrement = 5,
                SequenceFormat = "000",
                SequencePosition = SequencePosition.Replace
            };

            // Act
            var result = _renamerLogic.GenerateNewFileName(file, pattern);

            // Assert
            Assert.IsFalse(result.HasError);
            Assert.AreEqual("020.txt", result.NewFileName); // 10 + (2 * 5) = 20
        }

        [TestMethod]
        public void GenerateNewFileName_CombinedOperations_ReturnsCorrectName()
        {
            // Arrange
            var file = new FileItem("test123.txt");
            file.Index = 1;
            var pattern = new RenamePattern
            {
                Prefix = "Pre_",
                Suffix = "_Suf",
                FindText = "[0-9]+",
                ReplaceText = "Num",
                UseRegex = true,
                UseSequence = true,
                SequenceStart = 5,
                SequenceIncrement = 10,
                SequenceFormat = "00",
                SequencePosition = SequencePosition.Suffix
            };

            // Act
            var result = _renamerLogic.GenerateNewFileName(file, pattern);

            // Assert
            Assert.IsFalse(result.HasError);
            Assert.AreEqual("Pre_testNum15_Suf.txt", result.NewFileName);
        }

        [TestMethod]
        public void GenerateNewFileName_InvalidCharacters_ReturnsError()
        {
            // Arrange
            var file = new FileItem("test.txt");
            var pattern = new RenamePattern { Prefix = "Invalid?" };

            // Act
            var result = _renamerLogic.GenerateNewFileName(file, pattern);

            // Assert
            Assert.IsTrue(result.HasError);
            Assert.AreEqual("Invalid characters in filename", result.ErrorMessage);
        }
    }
}
