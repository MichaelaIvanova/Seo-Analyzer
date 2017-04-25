using System.Collections.Generic;
using System.Threading.Tasks;
using Analyzer.BusinessLogic.Models;
using HtmlAgilityPack;

namespace Analyzer.BusinessLogic.Services.Contracts
{
    public interface IWordCountService
    {
        IList<string> GetExternalLinks(string url, HtmlDocument document);
        IDictionary<string, int> GetMetaKeywordsFrequencies(HtmlDocument document, IDictionary<string, int> wordsCountDictionary);
        Task<ExternalSourceInfoModel> GetRemoteData(string url);
        IDictionary<string, int> GetWordsFrequenciesFromExternalSource(HtmlDocument document);
        IDictionary<string, int> GetWordsFrequenciesFromText(string text);
    }
}