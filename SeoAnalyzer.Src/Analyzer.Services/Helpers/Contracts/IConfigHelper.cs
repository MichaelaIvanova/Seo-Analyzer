namespace Analyzer.BusinessLogic.Helpers.Contracts
{
    public interface IConfigHelper
    {
        string GetValue(string paramName);
        int GetValueId(string paramName);
    }
}