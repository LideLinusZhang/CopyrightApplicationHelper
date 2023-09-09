using HelperConsole.Configuration.RuleModel;

namespace HelperConsole.Configuration
{
    internal interface IConfigurationReader
    {
        public IAsyncEnumerable<LanguageRule> ReadFromStream(Stream stream);
    }
}
