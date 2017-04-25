using Analyzer.BusinessLogic.Helpers.Contracts;
using System.Configuration;

namespace Analyzer.BusinessLogic.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public string GetValue(string paramName)
        {
            return ConfigurationManager.AppSettings[paramName];
        }

        public int GetValueId(string paramName)
        {
            return int.Parse(GetValue(paramName));
        }
    }
}
