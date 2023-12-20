using CopyrightHelper.Library.Configuration.RuleModel;
using System.Collections.Generic;
using System.IO;

namespace CopyrightHelper.Library.Configuration
{
    public interface IConfigurationReader
    {
        public IAsyncEnumerable<LanguageRule> ReadFromStream(Stream stream);
    }
}
