using Analyzer.Models;

namespace Analyzer.Helpers.Contracts
{
    public interface IJsonResponseFormatHelper
    {
        string GetJsonParsedModel(WordCounterResultViewModel inputModel);
    }
}