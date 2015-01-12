using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CCC.UTILS.Libs
{
    public class JsonTranslator
    {
         public static string Serialize<T>(T t) 
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
           
            serializer.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            
            ms.Close();

            //Replace Json Date String                                         
            string pattern = @"\\/Date\((\d+)\+\d+\)\\/";
            MatchEvaluator Evaluator = new MatchEvaluator(JsonDateToDateString);
            Regex regex = new Regex(pattern);
            jsonString = regex.Replace(jsonString, Evaluator);
             
            return jsonString;
        }

        public static T Deserialize<T>(string jsonString)
        {
            //Convert "yyyy-MM-dd HH:mm:ss" String as "\/Date(1319266795390+0800)\/"
            string pattern = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator Evaluator = new MatchEvaluator(DateStringToJsonDate);
            Regex regex = new Regex(pattern);
            jsonString = regex.Replace(jsonString, Evaluator);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

            T obj = (T)serializer.ReadObject(ms);
            return obj;
        }


        private static string JsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime date= new DateTime(1970, 1, 1);
            date = date.AddMilliseconds(long.Parse(m.Groups[1].Value));
            date = date.ToLocalTime();
            result = date.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }
        
        private static string DateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime date = DateTime.Parse(m.Groups[0].Value);
            date = date.ToUniversalTime();
            TimeSpan ts = date - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }
   
    }
}
