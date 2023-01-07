using System;
using System.Collections.Generic;
using System.Linq;

namespace StatsNET
{
    public static class Statistics
    {
        static readonly object locker = new object();

        public static KeyValuePair<string, double>[] Values
        {
            get
            {
                lock (locker) return values.ToArray();
            }
        }

        static readonly SortedDictionary<string, double> values = new SortedDictionary<string, double>();

        public static void IncreaseValue(string key, double amount = 1)
        {
            KeyValuePair<string, double>[] values;
            lock (locker)
            {
                if (!Statistics.values.ContainsKey(key)) Statistics.values.Add(key, amount);
                else Statistics.values[key] += amount;
                values = Statistics.values.ToArray();
            }
            Updated?.Invoke(values);
        }

        public static event Action<KeyValuePair<string, double>[]> Updated;

        public static double GetValue(string id)
        {
            lock (locker)
            {
                if (values.ContainsKey(id)) return values[id];
                else return 0;
            }
        }

        public static void Clear()
        {
            lock (locker) values.Clear();
        }
    }
}
