using Analyzer.BusinessLogic.Helpers.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Caching;

namespace Analyzer.BusinessLogic.Helpers
{
    public class FileHelper : IFileHelper
    {
        public List<string> LoadData(string path)
        {
            var data = new List<string>();
            try
            {
                using (StreamReader r = new StreamReader(path))
                {
                    var words = r.ReadToEnd();
                    var cdata = JsonConvert.DeserializeObject<string[]>(words);
                    data.AddRange(cdata);
                }
            }
            catch (Exception ex)
            {
                //TODO logger
            }

            return data;
        }

        public  HashSet<string> GetUniqueResultsFromJson(string path)
        {
            List<string> uniqueWords;
            ObjectCache cache = MemoryCache.Default;
            var cachedItem = cache["unique" + path];
            if (cachedItem == null)
            {
                uniqueWords = LoadData(path);
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10);
                cache.Set("unique" + path, uniqueWords, policy);
            }
            else
            {
                uniqueWords = cachedItem as List<string>;
            }

            var result = new HashSet<string>(uniqueWords);

            return result;
        }
    }
}
