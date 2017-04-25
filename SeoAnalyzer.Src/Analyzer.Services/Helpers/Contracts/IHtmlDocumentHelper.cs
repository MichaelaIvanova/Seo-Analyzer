using System.Collections.Generic;
using HtmlAgilityPack;

namespace Analyzer.BusinessLogic.Helpers.Contracts
{
    public interface IHtmlDocumentHelper
    {
        List<string> GetExternalLinks(string url, HtmlDocument document);
        bool IsValid(HtmlDocument document);
        void SanitizeContent(HtmlDocument document);
    }
}