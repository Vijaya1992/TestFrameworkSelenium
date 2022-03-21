using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSelenium.Common
{
  public  class ConfigReader
    {       
        private static ConfigReader instance = null;
        private static ConcurrentDictionary<string, object> _commonData = new ConcurrentDictionary<string, object>();
        private static Dictionary<string, object> _config = new Dictionary<string, object>();
        private static string previousFilePath;
        private static string previousKey;
        
        public static ConfigReader GetInstance(string key,string filePath)
        {
            if (previousFilePath == null || !filePath.Equals(previousFilePath)||previousKey==null||!key.Equals(previousKey))
            {
                instance = new ConfigReader(key,filePath);
                previousFilePath = filePath;
                previousKey = key;
            }
            return instance;
        }

        public ConfigReader(string key,string filePath)
        {
            var jsonText = File.ReadAllText(filePath);

            var dict = JsonConvert.DeserializeObject< IDictionary<string, IDictionary<string, object>>>(jsonText);
            foreach (var kv in dict)
            {
                string envName = kv.Key;
                if (envName == key)
                {

                    foreach (var v in kv.Value)
                    {
                        _config.Add(v.Key, v.Value);

                    }
                    break;
                }
                
              
            }
        }
        public string GetConfig(string key)
        {
            return _config[key].ToString();
        }

    }
}
