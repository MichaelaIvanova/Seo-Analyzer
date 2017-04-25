using Analyzer.BusinessLogic.Helpers.Contracts;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Analyzer.BusinessLogic.Helpers
{
    public class HtmlDocumentHelper : IHtmlDocumentHelper
    {
        private IEnumerable<char> invalidChars = new List<char> { '@', '(', ')' };

        public bool IsValid(HtmlDocument document)
        {
            bool IsValid = false;
            if (document.DocumentNode.SelectSingleNode("html/head") != null
                && document.DocumentNode.SelectSingleNode("html/body") != null)
            {
                IsValid = true;
            }

            return IsValid;

        }

        public void SanitizeContent(HtmlDocument document)
        {
            document.DocumentNode.Descendants()
                                 .Where(n => n.NodeType == HtmlAgilityPack.HtmlNodeType.Comment)
                                 .ToList()
                                 .ForEach(n => n.Remove());
        }

        public List<string> GetExternalLinks(string url, HtmlDocument document)
        {
            var externalLinks = new List<string>();
            var uri = new Uri(url);
            var domain = uri.Host;
            var body = document.DocumentNode.SelectSingleNode("//body");
            var links = body.SelectNodes("//a").Select(h => h.GetAttributeValue("href", "#"));
            foreach (var anchor in links)
            {
                if (!anchor.Contains(domain) && !invalidChars.Any(anchor.Contains) && anchor.IndexOf('/') != 0 && anchor.IndexOf('#') != 0)
                {
                    externalLinks.Add(anchor);
                }
            }

            return externalLinks;
        }
    }
}
