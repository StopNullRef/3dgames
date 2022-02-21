using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace Project.SD
{
    [Serializable]
    public class StaticDataModule
    {
        public List<SDSystemMessage> sdSystemMessages = new List<SDSystemMessage>();

        public void Initialize()
        {
            var loader = new StaticDataLoader();
            loader.Load<SDSystemMessage>(out sdSystemMessages);
        }

        private class StaticDataLoader
        {
            private string path;

            public StaticDataLoader()
            {
                path = $"{Application.dataPath}/StaticData/Json";
            }

            public void Load<T>(out List<T> data) where T : StaticDataBase
            {
                var fileName = typeof(T).Name.Remove(0, "SD".Length);

                var json = File.ReadAllText($"{path}/{fileName}.json");

                data = JsonConvert.DeserializeObject<List<T>>(json);
            }
        }
    }
}
