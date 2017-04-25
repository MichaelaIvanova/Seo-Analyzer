using HtmlAgilityPack;

namespace Analyzer.BusinessLogic.Models
{
    public class ExternalSourceInfoModel
    {
        public HtmlDocument Document { get; set; }

        public Status Status { get; set; }

        public string StatusMsg { get; set; }
    }
}
