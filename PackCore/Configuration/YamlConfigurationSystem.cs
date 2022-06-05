using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PackCore.Configuration
{
    public class YamlConfigurationSystem : FileConfigurationSystem
    {
        public override void readFile(string fileUri)
        {
            var deserilzer = new DeserializerBuilder()
                  .WithNamingConvention(UnderscoredNamingConvention.Instance)
                  .Build();
            string[] data = File.ReadAllLines(fileUri);
            StringBuilder builder = new StringBuilder();
            foreach (string s in data)
            {
                builder.Append(s);
            }

           this.data = deserilzer.Deserialize<Dictionary<string, Object>>(builder.ToString());

        }
    }
}
