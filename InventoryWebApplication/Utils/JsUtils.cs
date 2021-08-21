using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryWebApplication.Utils
{
    public static class JsUtils
    {
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