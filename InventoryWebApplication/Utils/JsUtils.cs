using System.Collections.Generic;
using Newtonsoft.Json;

namespace InventoryWebApplication.Utils
{
    public static class JsUtils
    {
        /// <summary>
        ///     Builds a JavaScript array containing the data of a dictionary to be converted to a chart
        /// </summary>
        /// <param name="keysLabel">The label of the keys column</param>
        /// <param name="valuesLabel">The label of the values column</param>
        /// <param name="dict">A dictionary containing the table data</param>
        /// <typeparam name="TValue">Type of the dictionary values</typeparam>
        /// <returns>A string with the data as nested arrays in the JavaScript format</returns>
        public static string BuildChartData<TValue>(string keysLabel, string valuesLabel,
            IEnumerable<KeyValuePair<string, TValue>> dict)
        {
            List<object[]> data = new() {new object[] {keysLabel, valuesLabel}};

            foreach ((string key, TValue value) in dict)
                data.Add(new object[] {key, value});

            string content = EscapeSingleQuotes(JsonConvert.SerializeObject(data));
            return $"JSON.parse('{content}')";
        }

        /// <summary>
        ///     Builds a JavaScript array containing the data of a dictionary to be converted to a chart
        /// </summary>
        /// <param name="keysLabel">The label of the keys column</param>
        /// <param name="values1Label">The label of the values1 column</param>
        /// <param name="values2Label">The label of the values2 column</param>
        /// <param name="dict">A dictionary containing the table data</param>
        /// <typeparam name="TValue1">Type of one of the dictionary values</typeparam>
        /// <typeparam name="TValue2">Type of one of the dictionary values</typeparam>
        /// <returns>>A string with the data as nested arrays in the JavaScript format</returns>
        public static string BuildChartData<TValue1, TValue2>(string keysLabel, string values1Label,
            string values2Label, IEnumerable<KeyValuePair<string, (TValue1, TValue2)>> dict)
        {
            List<object[]> data = new() {new object[] {keysLabel, values1Label, values2Label}};

            foreach ((string key, (TValue1 item1, TValue2 item2)) in dict)
                data.Add(new object[] {key, item1, item2});

            string content = EscapeSingleQuotes(JsonConvert.SerializeObject(data));
            return $"JSON.parse('{content}')";
        }

        private static string EscapeSingleQuotes(string str) => str.Replace("'", "\\'");
    }
}