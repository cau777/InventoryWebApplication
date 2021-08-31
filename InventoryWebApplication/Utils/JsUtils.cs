using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryWebApplication.Utils
{
    public static class JsUtils
    {
        /// <summary>
        /// Builds a JavaScript array containing the data of a dictionary to be converted to a chart
        /// </summary>
        /// <param name="keysLabel">The label of the keys column</param>
        /// <param name="valuesLabel">The label of the values column</param>
        /// <param name="dict">A dictionary containing the table data</param>
        /// <param name="keyToString">Function to convert TKey to string</param>
        /// <param name="valueToString">Function to convert TValue to string</param>
        /// <typeparam name="TKey">Type of the dictionary keys</typeparam>
        /// <typeparam name="TValue">Type of the dictionary values</typeparam>
        /// <returns>A string with the data as nested arrays in the JavaScript format</returns>
        public static string BuildChartData<TKey, TValue>(string keysLabel, string valuesLabel,
            IDictionary<TKey, TValue> dict, Func<TKey, string> keyToString, Func<TValue, string> valueToString)
        {
            StringBuilder builder = new();
            builder.Append("['")
                .Append(keysLabel).Append("', '")
                .Append(valuesLabel).Append("'],");

            foreach ((TKey key, TValue value) in dict)
            {
                builder.Append("['")
                    .Append(keyToString(key)).Append("', ")
                    .Append(valueToString(value)).Append("],");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Builds a JavaScript array containing the data of a dictionary to be converted to a chart
        /// </summary>
        /// <param name="keysLabel">The label of the keys column</param>
        /// <param name="values1Label">The label of the values1 column</param>
        /// <param name="values2Label">The label of the values2 column</param>
        /// <param name="dict">A dictionary containing the table data</param>
        /// <param name="keyToString">Function to convert TKey to string</param>
        /// <param name="value1ToString">Function to convert TValue1 to string</param>
        /// <param name="value2ToString">Function to convert TValue2 to string</param>
        /// <typeparam name="TKey">Type of the dictionary keys</typeparam>
        /// <typeparam name="TValue1">Type of one of the dictionary values</typeparam>
        /// <typeparam name="TValue2">Type of one of the dictionary values</typeparam>
        /// <returns>>A string with the data as nested arrays in the JavaScript format</returns>
        public static string BuildChartData<TKey, TValue1, TValue2>(string keysLabel, string values1Label,
            string values2Label, IDictionary<TKey, ValueTuple<TValue1, TValue2>> dict, Func<TKey, string> keyToString,
            Func<TValue1, string> value1ToString, Func<TValue2, string> value2ToString)
        {
            StringBuilder builder = new();
            builder.Append("['")
                .Append(keysLabel).Append("', '")
                .Append(values1Label).Append("', '")
                .Append(values2Label).Append("'],");

            foreach ((TKey key, (TValue1 item1, TValue2 item2)) in dict)
            {
                builder.Append("['")
                    .Append(keyToString(key)).Append("', ")
                    .Append(value1ToString(item1)).Append(", ")
                    .Append(value2ToString(item2)).Append("],");
            }

            return builder.ToString();
        }
    }
}