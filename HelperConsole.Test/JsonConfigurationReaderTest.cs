using CopyrightHelper.Library.Configuration;
using CopyrightHelper.Library.Configuration.RuleModel;
using System.Text;

namespace CopyrightHelper.Library.Test
{
    [TestClass]
    public class JsonConfigurationReaderTest
    {
        [TestMethod]
        public void ReadFromStream_JsonWithCorrectFormat_ReturnDeserializedRule()
        {
            // Arrange
            string jsonConfiguration = "[{\"Name\":\"C++\",\"ExtensionNames\":[\".cxx\", \".cpp\", \".cc\"],\"LineCommentSymbols\":[\"//\"],\"BlockCommentRules\":[{\"StartSymbol\":\"/*\",\"EndSymbol\":\"*/\"}]}]";
            var stream = new MemoryStream(Encoding.Default.GetBytes(jsonConfiguration));
            var reader = new JsonConfigurationReader();

            // Act
            var result = reader.ReadFromStream(stream).ToBlockingEnumerable().ToList();

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("C++", result.First().Name);
            CollectionAssert.AreEquivalent(new string[] { ".cpp", ".cxx", ".cc" }, result.First().ExtensionNames);
            CollectionAssert.AreEquivalent(new string[] { "//" }, result.First().LineCommentSymbols);
            CollectionAssert.AreEquivalent(new BlockCommentRule[] { new () { StartSymbol = "/*", EndSymbol="*/" } }, result.First().BlockCommentRules);
        }
    }
}