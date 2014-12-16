using LyncBillingBase.DataAccess;
using LyncBillingBase.DataAttributes;
using LyncBillingBase.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LyncBillingBase.Helpers
{
    public class RandomObjectsGenerator<T> where T: class,new()
    {
        private static  Random rand = new Random();

        private static List<long> randomLongNumbersList = new List<long>();
        private static List<Int32> randomIntNumbersList = new List<Int32>();
        private static List<DateTime> randomDateTimeList = new List<DateTime>();
        
        // Randomization bounds 
        private int minLongRandBound = 1;
        private long maxLongRandBound = long.MaxValue;

        private int minIntRandBound = 1;
        private int MaxIntRandBound = Int32.MaxValue;


        private int GetIntNumber()
        {
            byte[] buf = new byte[8];
            
            rand.NextBytes(buf);
           
            int intRand = BitConverter.ToInt32(buf, 0);

            var value = Math.Abs(intRand % (minIntRandBound - MaxIntRandBound)) + minIntRandBound;

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
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            var value = Math.Abs(longRand % (minLongRandBound - maxLongRandBound)) + minLongRandBound;

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
            byte scale = (byte)rand.Next(29);
            bool sign = rand.Next(2) == 1;
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
            int value = rand.Next(100);

            if (value <= 50)
                return true;
            else
                return false;
        }

        private string GetString() 
        {
            return Guid.NewGuid().ToString();
        }

        private DateTime GetDateTime() 
        {
            DateTime startingDate = DateTime.Now.AddYears(-2);

            int range = (DateTime.Today - startingDate).Days;

            DateTime value = startingDate
                .AddDays(rand.Next(range))
                .AddHours(rand.Next(0,24))
                .AddMinutes(rand.Next(0,60))
                .AddSeconds(rand.Next(0,60))
                .AddMilliseconds(rand.Next(0,999));

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
            Byte[] b = new Byte[10];
            
            rand.NextBytes(b);

            return b[rand.Next(0,9)];

        }

        public  static T GenerateRandomObject()
        {

            RandomObjectsGenerator<T> randObjGen = new RandomObjectsGenerator<T>();


            Dictionary<string, Action<T, object>> setters = new Dictionary<string, Action<T, object>>();

            // List of class property infos
            List<PropertyInfo> masterPropertyInfoFields = new List<PropertyInfo>();

            //List of T object data fields (DbColumnAttribute Values), and types.
            List<ObjectPropertyInfoField> masterObjectFields = new List<ObjectPropertyInfoField>();

            //Define what attributes to be read from the class
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            masterPropertyInfoFields = typeof(T).GetProperties(flags)
               .Cast<PropertyInfo>()
               .ToList();

            foreach (var field in masterPropertyInfoFields)
            {
                var propertyInfo = typeof(T).GetProperty(field.Name);
                var propertyName = field.Name;
                setters.Add(propertyName, Invoker.CreateSetter<T>(propertyInfo));
            }

            T obj = new T();

            var typedValueMap = new Dictionary<Type, Delegate>
            {
                {typeof(int),new Func<int>(() => randObjGen.GetIntNumber())},
                {typeof(long),new Func<long>(() => randObjGen.GetLongNumber())},
                {typeof(decimal),new Func<decimal>(() => randObjGen.GetDecimal())},
                {typeof(bool),new Func<bool>(() => randObjGen.GetBool())},
                {typeof(DateTime),new Func<DateTime>(() => randObjGen.GetDateTime())},
                {typeof(string),new Func<string>(() => randObjGen.GetString())},
                {typeof(byte),new Func<byte>(() => randObjGen.GetByte())}
            };

            
            foreach (var setter in setters) 
            {
                Type type = masterPropertyInfoFields.Where(item => item.Name == setter.Key).Select(item => item.PropertyType).FirstOrDefault();

                if (type != null)
                {
                    int y = randObjGen.GetIntNumber();

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
