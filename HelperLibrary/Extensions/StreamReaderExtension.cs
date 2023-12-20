using System.Collections.Generic;
using System.IO;

namespace CopyrightHelper.Library.Extensions
{
    public static class StreamReaderExtension
    {
        public static async IAsyncEnumerable<string> ReadAllLinesAsync(this StreamReader reader)
        {
            while (!reader.EndOfStream)
                yield return await reader.ReadLineAsync();
        }
    }
}
