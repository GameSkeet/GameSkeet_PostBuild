using System;
using System.IO;

using LZ4;

namespace GameSkeet_PostBuild
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) return;

            #region Backend thingy

            string PathToBuild = args[0];

            if (!File.Exists(PathToBuild + "\\GameSkeet.dll"))
                throw new ApplicationException("No file found!");

            byte[] data = File.ReadAllBytes(PathToBuild + "\\GameSkeet.dll");
            byte[] compressed = LZ4Codec.Encode(data, 0, data.Length);

            string finalData =
$@"{{
    ""BuildDate"": {Math.Round(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds)},
    ""FileData"": ""{Convert.ToBase64String(compressed)}""
}}";

            File.WriteAllText(PathToBuild + "\\Output_data.json", finalData);

            #endregion

            #region Build increment

            if (!File.Exists("Build.h"))
                File.WriteAllText("Build.h", "\"0\"");

            uint build = uint.Parse(File.ReadAllText("Build.h").Replace("\"", ""));
            build++;

            File.WriteAllText("Build.h", $"\"{build}\"");

            #endregion
        }
    }
}
