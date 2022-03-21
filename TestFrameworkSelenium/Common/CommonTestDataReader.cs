using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestSelenium.Common
{
    public class CommonTestDataReader
    {
        private static CommonTestDataReader instance = null;
        private static ConcurrentDictionary<string, object> _commonData= new ConcurrentDictionary<string, object>();
        private static string previousFilePath;        

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static CommonTestDataReader GetInstance(string filePath)
        {
            if (previousFilePath == null || !filePath.Equals(previousFilePath))
            {
                instance = new CommonTestDataReader(filePath);
                previousFilePath = filePath;
            }
            return instance;
        }

        public CommonTestDataReader(string filePath)
        {
            var jsonText = File.ReadAllText(filePath);

            var dict = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonText);
            foreach (var kv in dict)
            {
                string key = kv.Key;
                object value = kv.Value;
                _commonData.TryAdd(key, value);
            }

        }

        public string GetProperty(string key)
        {           
            return _commonData[key].ToString();
        }
    

}
}
