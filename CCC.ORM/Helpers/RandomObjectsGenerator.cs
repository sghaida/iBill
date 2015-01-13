using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CCC.ORM.DataAccess;

namespace CCC.ORM.Helpers
{
    public class RandomObjectsGenerator<T> where T : class, new()
    {
        private static readonly Random rand = new Random();
        private static readonly List<long> randomLongNumbersList = new List<long>();
        private static readonly List<Int32> randomIntNumbersList = new List<Int32>();
        private static readonly List<DateTime> randomDateTimeList = new List<DateTime>();
        private readonly int MaxIntRandBound = Int32.MaxValue;
        private readonly long maxLongRandBound = long.MaxValue;
        private readonly int minIntRandBound = 1;
        // Randomization bounds 
        private readonly int minLongRandBound = 1;

        private int GetIntNumber()
        {
            var buf = new byte[8];

            rand.NextBytes(buf);

            var intRand = BitConverter.ToInt32(buf, 0);

            var value = Math.Abs(intRand%(minIntRandBound - MaxIntRandBound)) + minIntRandBound;

            if (!randomIntNumbersList.Contains(value))
            {
                randomIntNumbersList.Add(value);
            }
            else
            {
                GetIntNumber();
            }

            return value;
        }

        private long GetLongNumber()
        {
            var buf = new byte[8];
            rand.NextBytes(buf);
            var longRand = BitConverter.ToInt64(buf, 0);

            var value = Math.Abs(longRand%(minLongRandBound - maxLongRandBound)) + minLongRandBound;

            if (!randomLongNumbersList.Contains(value))
            {
                randomLongNumbersList.Add(value);
            }
            else
            {
                GetIntNumber();
            }

            return value;
        }

        private decimal GetDecimal()
        {
            var scale = (byte) rand.Next(29);
            var sign = rand.Next(2) == 1;
            return new decimal
                (
                GetIntNumber(),
                GetIntNumber(),
                GetIntNumber(),
                sign,
                scale
                );
        }

        private bool GetBool()
        {
            var value = rand.Next(256);
            return value >= 128;
        }

        private string GetString()
        {
            return Guid.NewGuid().ToString();
        }

        private DateTime GetDateTime()
        {
            var startingDate = DateTime.Now.AddYears(-2);

            var range = (DateTime.Today - startingDate).Days;

            var value = startingDate
                .AddDays(rand.Next(range))
                .AddHours(rand.Next(0, 24))
                .AddMinutes(rand.Next(0, 60))
                .AddSeconds(rand.Next(0, 60))
                .AddMilliseconds(rand.Next(0, 999));

            if (!randomDateTimeList.Contains(value))
            {
                randomDateTimeList.Add(value);
            }
            else
            {
                GetDateTime();
            }

            return value;
        }

        private byte GetByte()
        {
            var b = new Byte[10];

            rand.NextBytes(b);

            return b[rand.Next(0, 9)];
        }

        public static T GenerateRandomObject()
        {
            var randObjGen = new RandomObjectsGenerator<T>();


            var setters = new Dictionary<string, Action<T, object>>();

            // List of class property infos
            var masterPropertyInfoFields = new List<PropertyInfo>();

            //List of T object data fields (DbColumnAttribute Values), and types.
            var masterObjectFields = new List<ObjectPropertyInfoField>();

            //Define what attributes to be read from the class
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            masterPropertyInfoFields = typeof (T).GetProperties(flags)
                .Cast<PropertyInfo>()
                .ToList();

            foreach (var field in masterPropertyInfoFields)
            {
                var propertyInfo = typeof (T).GetProperty(field.Name);
                var propertyName = field.Name;
                setters.Add(propertyName, Invoker.CreateSetter<T>(propertyInfo));
            }

            var obj = new T();

            var typedValueMap = new Dictionary<Type, Delegate>
            {
                {typeof (int), new Func<int>(() => randObjGen.GetIntNumber())},
                {typeof (long), new Func<long>(() => randObjGen.GetLongNumber())},
                {typeof (decimal), new Func<decimal>(() => randObjGen.GetDecimal())},
                {typeof (bool), new Func<bool>(() => randObjGen.GetBool())},
                {typeof (DateTime), new Func<DateTime>(() => randObjGen.GetDateTime())},
                {typeof (string), new Func<string>(() => randObjGen.GetString())},
                {typeof (byte), new Func<byte>(() => randObjGen.GetByte())}
            };


            foreach (var setter in setters)
            {
                var type =
                    masterPropertyInfoFields.Where(item => item.Name == setter.Key)
                        .Select(item => item.PropertyType)
                        .FirstOrDefault();

                if (type != null)
                {
                    var y = randObjGen.GetIntNumber();

                    if (typedValueMap.ContainsKey(type))
                    {
                        setter.Value(obj, typedValueMap[type].DynamicInvoke(null));
                    }
                }
            }

            return obj;
        }
    }
}