using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientQueueMessageSender
{
    public static class ObjectSerializer
    {
        public static string DeSerializeText(this byte[] arrBytes)
        {
            return Encoding.Default.GetString(arrBytes);
        }

        public static string SerializeToJSON(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(obj);
        }

        public static object DeSerializeFromJSON(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        public static byte[] SerializeToByteArray(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            var json = SerializeToJSON(obj);
            return Encoding.ASCII.GetBytes(json);
        }

        public static object DeSerializeFromByteArray(this byte[] arrBytes, Type type)
        {
            var json = Encoding.Default.GetString(arrBytes);
            return DeSerializeFromJSON(json, type);
        } 
    }
}
