using Analyzer.Models;
using Analyzer.BusinessLogic.Models;
using Newtonsoft.Json;
using System.Linq;
using Analyzer.Helpers.Contracts;

namespace Analyzer.Helpers
{
    public class JsonResponseFormatHelper : IJsonResponseFormatHelper
    {
        public string GetJsonParsedModel(WordCounterResultViewModel inputModel)
        {
            var formatModel = new FriendlyJsonFormatModel();
            formatModel.ExternalSourceUrl = inputModel.ExternalSourceUrl;
            formatModel.ResponseStatus = inputModel.ResponseStatus.ToString();
            formatModel.ResponseStatusMsg = inputModel.ResponseStatusMsg;
            if (inputModel.WordsFrequencies != null)
            {
                formatModel.WordsFrequencies = inputModel.WordsFrequencies.Select(c => new DictEntry { Key = c.Key, Value = c.Value }).ToList();
            }

            if (inputModel.MetaKeywordsFrequencies != null)
            {
                formatModel.MetaKeywordsFrequencies = inputModel.MetaKeywordsFrequencies.Select(c => new DictEntry { Key = c.Key, Value = c.Value }).ToList();
            }

            if (inputModel.ExternalLinks != null)
            {
                formatModel.ExternalLinks = inputModel.ExternalLinks.Select(c => new DictEntry { Key = c }).ToList();
            }

            var jsonResult = JsonConvert.SerializeObject(formatModel);

            return jsonResult;
        }
    }
}
