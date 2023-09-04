namespace HelperConsole.Extensions
{
    internal static class StringExtension
    {
        public static int IndexOfAny(this string s, List<string> anyOf) => s.IndexOfAny(anyOf, out _);

        public static int IndexOfAny(this string s, List<string> anyOf, out string matched) 
        {
            int lastIndex = -1;
            matched = null;

            foreach (string pattern in anyOf) 
            {
                if (lastIndex == -1)
                    lastIndex = s.IndexOf(pattern);
                else
                {
                    int index = s.IndexOf(pattern, 0, lastIndex + 1);

                    if (index >= 0 && index < lastIndex)
                    {
                        lastIndex = index;
                        matched = pattern;
                    }
                }
            }

            return lastIndex;
        }
    }
}
