using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC_2020
{
    public static class MultiLineInputReader
    {
        public static async Task<List<T>> ReadInput<T>(string fileName)
        {
            var fileLines = await System.IO.File.ReadAllLinesAsync(fileName);
            var listInRightType = fileLines
                .Select(l => (T)Convert.ChangeType(l, typeof(T)))
                .ToList();
            return listInRightType;
        }

        public static async Task<T> ReadJsonInput<T>(string fileName) where T : class
        {
            var fileData = await System.IO.File.ReadAllTextAsync(fileName);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(fileData);
        }
    }
}