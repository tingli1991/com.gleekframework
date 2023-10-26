using ProtoBuf;
using System.IO;
using System.Linq;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// Protobuf序列化拓展类
    /// </summary>
    public static partial class ProtobufExtensions
    {
        /// <summary>
        ///序列化成二进制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] SerializeBinary<T>(this T value)
        {
            if (value == null)
            {
                return new byte[0];
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, value);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 反序列化为数据模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(this byte[] bytes)
        {
            var result = default(T);
            if (bytes == null || !bytes.Any())
            {
                return result;
            }

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Serializer.Deserialize<T>(ms);
            }
        }
    }
}