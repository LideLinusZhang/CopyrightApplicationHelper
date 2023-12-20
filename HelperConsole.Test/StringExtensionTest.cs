using CopyrightHelper.Library.Extensions;

namespace CopyrightHelper.Library.Test
{
    [TestClass]
    public class StringExtensionTest
    {
        [TestMethod]
        public void IndexOfAny_NoneMatched_ReturnNegativeOneAndSetMatchedToNull()
        {
            // Arrange
            List<string> anyOf = new() { "a", "b", "c" };
            string s = "ddddddd";

            // Act
            string matched;
            int index = s.IndexOfAny(anyOf, out matched);

            // Assert
            Assert.AreEqual(-1, index);
            Assert.IsNull(matched);
        }

        [TestMethod]
        public void IndexOfAny_Matched_ReturnIndexAndSetMatchedToFirstMatchedPattern()
        {
            // Arrange
            List<string> anyOf = new() { "d", "dd" };
            string s = "aaaaaddd";

            // Act
            string matched;
            int index = s.IndexOfAny(anyOf, out matched);

            // Assert
            Assert.AreEqual(5, index);
            Assert.AreEqual("d" ,matched);
        }
    }
}
