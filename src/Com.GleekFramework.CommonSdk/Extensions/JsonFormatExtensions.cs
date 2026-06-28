using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// JSON格式化拓展类
    /// </summary>
    public static partial class JsonFormatExtensions
    {
        /// <summary>
        /// JSON格式化(压缩+格式化+缩进)
        /// </summary>
        /// <param name="jsonValue">JSONN字符串</param>
        /// <returns></returns>
        public static string Format(this string jsonValue)
        {
            if (string.IsNullOrEmpty(jsonValue))
            {
                return "";
            }

            jsonValue = jsonValue.JsonFormat();//格式化JSON
            jsonValue = jsonValue.JsonIndent();//缩进JSON
            return jsonValue;
        }

        /// <summary>
        /// Json转义
        /// </summary>
        /// <param name="jsonValue">Json格式化</param>
        /// <returns></returns>
        public static string JsonEscape(this string jsonValue)
        {
            if (string.IsNullOrEmpty(jsonValue))
            {
                return "";
            }

            jsonValue = jsonValue.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            return JsonConvert.SerializeObject(jsonValue).TrimStart('\"').TrimEnd('\"');
        }

        /// <summary>
        /// 压缩并转义
        /// </summary>
        /// <param name="obj">实例对象</param>
        /// <returns></returns>
        public static string JsonCompressAndEscape(this object obj)
        {
            if (obj == null)
            {
                return "";
            }

            var jsonValue = JsonConvert.SerializeObject(obj);
            return jsonValue.JsonCompressAndEscape();
        }

        /// <summary>
        /// 压缩并转义
        /// </summary>
        /// <param name="jsonValue">Json数据</param>
        /// <returns></returns>
        public static string JsonCompressAndEscape(this string jsonValue)
        {
            if (string.IsNullOrEmpty(jsonValue))
            {
                return "";
            }

            jsonValue = jsonValue.JsonCompress();
            jsonValue = jsonValue.JsonEscape();
            return jsonValue;
        }

        /// <summary>
        /// 压缩JSON
        /// </summary>
        /// <param name="jsonValue">json字符串</param>
        /// <returns></returns>
        public static string JsonCompress(this string jsonValue)
        {
            if (string.IsNullOrEmpty(jsonValue))
            {
                return "";
            }

            var jsonBuilder = new StringBuilder();
            using StringReader reader = new StringReader(jsonValue);
            int ch = -1;
            int lastch = -1;
            bool isQuoteStart = false;
            while ((ch = reader.Read()) > -1)
            {
                if ((char)lastch != '\\' && (char)ch == '\"')
                {
                    if (!isQuoteStart)
                    {
                        isQuoteStart = true;
                    }
                    else
                    {
                        isQuoteStart = false;
                    }
                }
                if (!char.IsWhiteSpace((char)ch) || isQuoteStart)
                {
                    jsonBuilder.Append((char)ch);
                }
                lastch = ch;
            }
            return jsonBuilder.ToString();
        }

        /// <summary>
        /// JSON格式化
        /// </summary>
        /// <param name="jsonValue">JSONN字符串</param>
        /// <returns></returns>
        public static string JsonFormat(this string jsonValue)
        {
            if (string.IsNullOrEmpty(jsonValue))
            {
                return "";
            }

            var builderJson = new StringBuilder();
            var strCompress = jsonValue.JsonCompress();//压缩
            using StringReader reader = new StringReader(strCompress);
            using StringWriter writer = new StringWriter(builderJson);
            int ch = -1;
            int lastch = -1;
            bool isQuoteStart = false;
            while ((ch = reader.Read()) > -1)
            {
                var temp = new StringBuilder();
                switch ((char)ch)
                {
                    case '{':
                        if (isQuoteStart)
                        {
                            temp.Append('{');
                        }
                        else
                        {
                            temp.Append('{');
                            if ((char)reader.Peek() != '}')
                            {
                                temp.Append(Environment.NewLine);
                            }
                        }
                        break;
                    case '}':
                        if (isQuoteStart)
                        {
                            temp.Append('}');
                        }
                        else
                        {
                            if ((char)lastch != '{' && (char)lastch != '}')
                            {
                                temp.Append(Environment.NewLine);
                            }
                            temp.Append('}');
                            if ((char)reader.Peek() != ',')
                            {
                                temp.Append(Environment.NewLine);
                            }
                        }
                        break;
                    case '[':
                        if (isQuoteStart)
                        {
                            temp.Append('[');
                        }
                        else
                        {
                            temp.Append('[');
                            if ((char)reader.Peek() != ']')
                            {
                                temp.Append(Environment.NewLine);
                            }
                        }
                        break;
                    case ']':
                        if (isQuoteStart)
                        {
                            temp.Append(']');
                        }
                        else
                        {
                            if ((char)lastch != '[' && (char)lastch != ']')
                            {
                                temp.Append(Environment.NewLine);
                            }
                            temp.Append(']');
                            if ((char)reader.Peek() != ',' && (char)reader.Peek() != '}')
                            {
                                temp.Append(Environment.NewLine);
                            }
                        }
                        break;
                    case '\"':
                        if ((char)lastch != '\\')
                        {
                            if (!isQuoteStart)
                            {
                                isQuoteStart = true;
                            }
                            else
                            {
                                isQuoteStart = false;
                            }
                        }
                        temp.Append("\"");
                        break;
                    case ':':
                        if (isQuoteStart)
                        {
                            temp.Append(':');
                        }
                        else
                        {
                            temp.Append(':');
                            temp.Append(" ");
                        }
                        break;
                    case ',':
                        if (isQuoteStart)
                        {
                            temp.Append(',');
                        }
                        else
                        {
                            temp.Append(',');
                            temp.Append(Environment.NewLine);
                        }
                        break;
                    case ' ':
                        if (isQuoteStart)
                        {
                            temp.Append(" ");
                        }
                        else
                        {
                            temp.Append("");
                            temp.Append(Environment.NewLine);
                        }
                        break;
                    default:
                        temp.Append((char)ch);
                        break;
                }
                writer.Write(temp.ToString());
                lastch = ch;
            }
            return builderJson.ToString();
        }

        /// <summary>
        /// JSON缩进
        /// </summary>
        /// <param name="jsonValue">JSONN字符串</param>
        /// <returns></returns>
        public static string JsonIndent(this string jsonValue)
        {
            if (string.IsNullOrEmpty(jsonValue))
            {
                return "";
            }

            var builderJson = new StringBuilder();
            using StringReader reader = new StringReader(jsonValue);
            using StringWriter writer = new StringWriter(builderJson);
            string str = null;
            int nspace = 0;
            string space = "\t";
            bool bEndMid = false;
            while ((str = reader.ReadLine()) != null)
            {
                if (str.Length == 0)
                {
                    continue;
                }

                if (str.EndsWith("},"))
                {
                    nspace--;
                }

                var temp = new StringBuilder();
                if (!bEndMid)
                {
                    for (int i = 0; i < (str.EndsWith("],") || (str.EndsWith("}") && !str.EndsWith("{}")) || str.EndsWith("]") ? nspace - 1 : nspace); i++)
                    {
                        temp.Append(space);
                    }
                }

                temp.Append(str);
                if (str.EndsWith("["))
                {
                    writer.Write(temp);
                    bEndMid = true;
                }
                else
                {
                    writer.WriteLine(temp);
                    bEndMid = false;
                }
                if (!(str.EndsWith("{}") || str.EndsWith("[]")))
                {
                    if (str.StartsWith("{") || str.EndsWith("{") ||
                        str.EndsWith("["))
                    {
                        nspace++;
                    }
                    if (str.EndsWith("}") || str.EndsWith("]"))
                    {
                        nspace--;
                    }
                }
            }
            return builderJson.ToString();
        }
    }
}