using Analyzer.BusinessLogic.Models;
using Analyzer.BusinessLogic.Services;
using System.Collections.Generic;

namespace Analyzer.Models
{
    public class WordCounterResultViewModel
    {
        public IDictionary<string,int> WordsFrequencies { get; set; }

        public IDictionary<string, int> MetaKeywordsFrequencies { get; set; }

        public string ExternalSourceUrl { get; set; }

        public Status ResponseStatus { get; set; }

        public string ResponseStatusMsg { get; set; }

        public IEnumerable<string> ExternalLinks { get; set; }
    }

   
}