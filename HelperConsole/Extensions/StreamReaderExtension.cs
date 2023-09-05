namespace HelperConsole.Extensions
{
    internal static class StreamReaderExtension
    {
        public static async IAsyncEnumerable<string> ReadAllLinesAsync(this StreamReader reader)
        {
            while (!reader.EndOfStream)
                yield return await reader.ReadLineAsync();
        }
    }
}
