using Analyzer.Models;
using Analyzer.BusinessLogic.Models;
using System.Threading.Tasks;
using System.Web.Mvc;
using Analyzer.Helpers;
using Analyzer.BusinessLogic.Services.Contracts;
using Analyzer.Helpers.Contracts;

namespace Analyzer.Controllers
{
    public class WordCounterController : Controller
    {
        private IWordCountService wordService;
        private IJsonResponseFormatHelper responseFormatHelper;
        public WordCounterController(IWordCountService wordService, IJsonResponseFormatHelper responseFormatHelper)
        {
            this.wordService = wordService;
            this.responseFormatHelper = responseFormatHelper;
        }

        [HttpPost]
        public async Task<ActionResult> CountWords(WordCounterViewModel model)
        {
            var result = new WordCounterResultViewModel();
            result.ResponseStatus = Status.NotSpecified;
            result.ResponseStatusMsg = string.Empty;
            if (!string.IsNullOrEmpty(model.Text))
            {
                result.WordsFrequencies = wordService.GetWordsFrequenciesFromText(model.Text);
            }
            else if (!string.IsNullOrEmpty(model.ExternlLink))
            {
                result.ExternalSourceUrl = model.ExternlLink;
                var source = await wordService.GetRemoteData(model.ExternlLink);
                result.ResponseStatus = source.Status;
                result.ResponseStatusMsg = source.StatusMsg;
                if (result.ResponseStatus == Status.Success)
                {
                    result.WordsFrequencies = wordService.GetWordsFrequenciesFromExternalSource(source.Document);
                    if (model.CountWordsInMeta)
                    {
                        result.MetaKeywordsFrequencies = wordService.GetMetaKeywordsFrequencies(source.Document, result.WordsFrequencies);
                    }

                    if (model.IncludeExternalLinks)
                    {
                        result.ExternalLinks = wordService.GetExternalLinks(model.ExternlLink, source.Document);
                    }
                }
            }
            
            var jsonResult = this.responseFormatHelper.GetJsonParsedModel(result);

            return this.Json(jsonResult);
        }

    }

}

