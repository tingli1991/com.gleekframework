using ProtoBuf;
using System.IO;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// Protobuf序列化拓展类
    /// </summary>
    public static partial class ProtobufExtensions
    {
        /// <summary>
        /// 序列化成二进制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] SerializeBinary<T>(this T value)
        {
            if (value == null)
            {
                return [];
            }

            using MemoryStream ms = new();
            Serializer.Serialize(ms, value);
            return ms.ToArray();
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
            if (bytes.IsNullOrEmpty())
            {
                return result;
            }

            using MemoryStream ms = new(bytes);
            return Serializer.Deserialize<T>(ms);
        }
    }
}