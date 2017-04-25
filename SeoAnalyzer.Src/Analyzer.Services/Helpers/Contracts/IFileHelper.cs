using System.Collections.Generic;

namespace Analyzer.BusinessLogic.Helpers.Contracts
{
    public interface IFileHelper
    {
        HashSet<string> GetUniqueResultsFromJson(string path);
        List<string> LoadData(string path);
    }
}