using ProtoBuf;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Com.GleekFramework.ObjectSdk
{
    /// <summary>
    /// 文件流拓展类
    /// </summary>
    internal static class StreamExtensions
    {
        /// <summary>
        /// 将字节流转换成文本内容
        /// </summary>
        /// <param name="bytes">字节流</param>
        /// <returns>包含字符串内容的内存流</returns>
        public static string ToContent(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 将文件流转换成文本内容
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns>包含字符串内容的内存流</returns>
        public static async Task<string> ToContentAsync(this Stream stream)
        {
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return ToContent(memoryStream.ToArray());
        }

        /// <summary>
        /// 序列化成二进制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Stream SerializeObject<T>(this T value)
        {
            if (value == null)
            {
                return null;
            }

            MemoryStream ms = new();
            Serializer.Serialize(ms, value);
            return ms;
        }

        /// <summary>
        /// 反序列化为数据模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(this Stream stream)
        {
            var result = default(T);
            if (stream == null || stream.Length <= 0)
            {
                return result;
            }

            // 重置流位置到起始点
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }
            // 尝试反序列化
            return Serializer.Deserialize<T>(stream);
        }

        /// <summary>
        /// 将字符串内容转换为流
        /// </summary>
        /// <param name="filePath">本地文件存放目录</param>
        /// <returns>包含字符串内容的内存流</returns>
        public static Stream OpenStream(this string filePath) => new FileStream(filePath, FileMode.Open);

        /// <summary>
        /// 将字符串内容转换为流
        /// </summary>
        /// <param name="content">字符串内容</param>
        /// <returns>包含字符串内容的内存流</returns>
        public static Stream ToStream(this string content) => new MemoryStream(Encoding.UTF8.GetBytes(content));
    }
}