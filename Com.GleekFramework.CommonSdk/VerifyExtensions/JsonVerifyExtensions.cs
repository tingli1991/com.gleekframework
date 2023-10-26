namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// JSON验证拓展类
    /// </summary>
    public static partial class JsonVerifyExtensions
    {
        /// <summary>
        /// 检查字符串是否是JSON格式
        /// </summary>
        /// <param name="jsonValue">JSON字符串</param>
        /// <returns></returns>
        public static bool IsJson(this string jsonValue)
        {
            return IsJson(jsonValue, out int errIndex);
        }

        /// <summary>
        /// 检查字符串是否以JSON字符开头
        /// </summary>
        /// <param name="jsonValue">JSON字符串</param>
        /// <returns></returns>
        public static bool IsJsonStart(this string jsonValue)
        {
            return IsJsonStart(ref jsonValue);
        }

        /// <summary>
        /// 检查字符串是否以JSON字符开头
        /// </summary>
        /// <param name="jsonValue">JSON字符串</param>
        /// <returns></returns>
        private static bool IsJsonStart(ref string jsonValue)
        {
            if (!string.IsNullOrEmpty(jsonValue))
            {
                jsonValue = jsonValue.Trim('\r', '\n', ' ');
                if (jsonValue.Length > 1)
                {
                    char s = jsonValue[0];
                    char e = jsonValue[jsonValue.Length - 1];
                    return s == '{' && e == '}' || s == '[' && e == ']';
                }
            }
            return false;
        }

        /// <summary>
        /// 检查字符串是否是JSON格式
        /// </summary>
        /// <param name="jsonValue"></param>
        /// <param name="errIndex"></param>
        /// <returns></returns>
        private static bool IsJson(string jsonValue, out int errIndex)
        {
            errIndex = 0;
            if (IsJsonStart(ref jsonValue))
            {
                char charStr;
                var jsonCharState = new JsonCharState();
                for (int index = 0; index < jsonValue.Length; index++)
                {
                    charStr = jsonValue[index];
                    if (JsonCharState.SetCharState(charStr, ref jsonCharState) && jsonCharState.childrenStart)//设置关键符号状态
                    {
                        var subJsonValue = jsonValue.Substring(index);
                        var length = GetValueLength(subJsonValue, true, out int err);
                        jsonCharState.childrenStart = false;
                        if (err > 0)
                        {
                            errIndex = index + err;
                            return false;
                        }
                        index = index + length - 1;
                    }

                    if (jsonCharState.isError)
                    {
                        errIndex = index;
                        return false;
                    }
                }
                return !jsonCharState.arrayStart && !jsonCharState.jsonStart;
            }
            return false;
        }

        /// <summary>
        /// 获取值的长度（当Json值嵌套以"{"或"["开头时）
        /// </summary>
        /// <param name="jsonValue"></param>
        /// <param name="breakOnErr"></param>
        /// <param name="errIndex"></param>
        /// <returns></returns>
        private static int GetValueLength(string jsonValue, bool breakOnErr, out int errIndex)
        {
            errIndex = 0;
            int len = 0;
            if (!string.IsNullOrEmpty(jsonValue))
            {
                char charStr;
                var jsonCharState = new JsonCharState();
                for (int index = 0; index < jsonValue.Length; index++)
                {
                    charStr = jsonValue[index];
                    if (!JsonCharState.SetCharState(charStr, ref jsonCharState))//设置关键符号状态。
                    {
                        if (!jsonCharState.jsonStart && !jsonCharState.arrayStart)//json结束，又不是数组，则退出
                        {
                            break;
                        }
                    }
                    else if (jsonCharState.childrenStart)//正常字符，值状态下。
                    {
                        var length = GetValueLength(jsonValue.Substring(index), breakOnErr, out errIndex);//递归子值，返回一个长度
                        jsonCharState.childrenStart = false;
                        jsonCharState.valueStart = 0;
                        index = index + length - 1;
                    }
                    if (breakOnErr && jsonCharState.isError)
                    {
                        errIndex = index;
                        return index;
                    }
                    if (!jsonCharState.jsonStart && !jsonCharState.arrayStart)//记录当前结束位置。
                    {
                        len = index + 1;//长度比索引+1
                        break;
                    }
                }
            }
            return len;
        }
    }

    /// <summary>
    /// 字符状态
    /// </summary>
    internal class JsonCharState
    {
        /// <summary>
        /// 以 "{"开始了
        /// </summary>
        internal bool jsonStart = false;

        /// <summary>
        /// 可以设置字典值了
        /// </summary>
        internal bool setDicValue = false;

        /// <summary>
        /// 以"\"转义符号开始了
        /// </summary>
        internal bool escapeChar = false;

        /// <summary>
        /// 数组开始【仅第一开头才算】，值嵌套的以【childrenStart】来标识
        /// 以"[" 符号开始了
        /// </summary>
        internal bool arrayStart = false;

        /// <summary>
        /// 【0 初始状态，或 遇到“,”逗号】；【1 遇到“：”冒号】
        /// </summary>
        internal int state = 0;

        /// <summary>
        /// 【-1 取值结束】【0 未开始】【1 无引号开始】【2 单引号开始】【3 双引号开始】
        /// </summary>
        internal int keyStart = 0;

        /// <summary>
        /// 【-1 取值结束】【0 未开始】【1 无引号开始】【2 单引号开始】【3 双引号开始】
        /// </summary>
        internal int valueStart = 0;

        /// <summary>
        /// 是否语法错误
        /// </summary>
        internal bool isError = false;

        /// <summary>
        /// 子级嵌套开始了
        /// </summary>
        internal bool childrenStart = false;

        /// <summary>
        /// 只当成一级处理（因为GetLength会递归到每一个子项处理）
        /// </summary>
        /// <param name="charStr"></param>
        internal void CheckIsError(char charStr)
        {
            if (keyStart > 1 || valueStart > 1)
            {
                return;
            }
            //示例 ["aa",{"bbbb":123,"fff","ddd"}] 
            switch (charStr)
            {
                case '{'://[{ "[{A}]":[{"[{B}]":3,"m":"C"}]}]
                    isError = jsonStart && state == 0;//重复开始错误 同时不是值处理。
                    break;
                case '}':
                    isError = !jsonStart || keyStart != 0 && state == 0;//重复结束错误 或者 提前结束{"aa"}。正常的有{}
                    break;
                case '[':
                    isError = arrayStart && state == 0;//重复开始错误
                    break;
                case ']':
                    isError = !arrayStart || jsonStart;//重复开始错误 或者 Json 未结束
                    break;
                case '"':
                case '\'':
                    isError = !(jsonStart || arrayStart); //json 或数组开始
                    if (!isError)
                    {
                        //重复开始 [""",{"" "}]
                        isError = state == 0 && keyStart == -1 || state == 1 && valueStart == -1;
                    }
                    if (!isError && arrayStart && !jsonStart && charStr == '\'')//['aa',{}]
                    {
                        isError = true;
                    }
                    break;
                case ':':
                    isError = !jsonStart || state == 1;//重复出现
                    break;
                case ',':
                    isError = !(jsonStart || arrayStart); //json 或数组开始
                    if (!isError)
                    {
                        if (jsonStart)
                        {
                            isError = state == 0 || state == 1 && valueStart > 1;//重复出现
                        }
                        else if (arrayStart)//["aa,] [,]  [{},{}]
                        {
                            isError = keyStart == 0 && !setDicValue;
                        }
                    }
                    break;
                case ' ':
                case '\r':
                case '\n'://[ "a",\r\n{} ]
                case '\0':
                case '\t':
                    break;
                default: //值开头。。
                    isError = !jsonStart && !arrayStart || state == 0 && keyStart == -1 || valueStart == -1 && state == 1;//
                    break;
            }
        }

        /// <summary>
        /// 设置字符状态(返回true则为关键词，返回false则当为普通字符处理）
        /// </summary>
        /// <param name="charStr"></param>
        /// <param name="jsonCharState"></param>
        /// <returns></returns>
        public static bool SetCharState(char charStr, ref JsonCharState jsonCharState)
        {
            jsonCharState.CheckIsError(charStr);
            switch (charStr)
            {
                case '{'://[{ "[{A}]":[{"[{B}]":3,"m":"C"}]}]  大括号开始
                    if (jsonCharState.keyStart <= 0 && jsonCharState.valueStart <= 0)
                    {
                        jsonCharState.keyStart = 0;
                        jsonCharState.valueStart = 0;
                        if (jsonCharState.jsonStart && jsonCharState.state == 1)
                        {
                            jsonCharState.childrenStart = true;
                        }
                        else
                        {
                            jsonCharState.state = 0;
                        }
                        jsonCharState.jsonStart = true;//开始
                        return true;
                    }
                    break;
                case '}': //大括号结束
                    if (jsonCharState.keyStart <= 0 && jsonCharState.valueStart < 2 && jsonCharState.jsonStart)
                    {
                        jsonCharState.jsonStart = false;//正常结束
                        jsonCharState.state = 0;
                        jsonCharState.keyStart = 0;
                        jsonCharState.valueStart = 0;
                        jsonCharState.setDicValue = true;
                        return true;
                    }
                    break;
                case '['://中括号开始
                    if (!jsonCharState.jsonStart)
                    {
                        jsonCharState.arrayStart = true;
                        return true;
                    }
                    else if (jsonCharState.jsonStart && jsonCharState.state == 1)
                    {
                        jsonCharState.childrenStart = true;
                        return true;
                    }
                    break;
                case ']'://中括号结束
                    if (jsonCharState.arrayStart && !jsonCharState.jsonStart && jsonCharState.keyStart <= 2 && jsonCharState.valueStart <= 0)//[{},333]//这样结束
                    {
                        jsonCharState.keyStart = 0;
                        jsonCharState.valueStart = 0;
                        jsonCharState.arrayStart = false;
                        return true;
                    }
                    break;
                case '"':
                case '\''://引号
                    if (jsonCharState.jsonStart || jsonCharState.arrayStart)
                    {
                        if (jsonCharState.state == 0)//key阶段,有可能是数组["aa",{}]
                        {
                            if (jsonCharState.keyStart <= 0)
                            {
                                jsonCharState.keyStart = charStr == '"' ? 3 : 2;
                                return true;
                            }
                            else if (jsonCharState.keyStart == 2 && charStr == '\'' || jsonCharState.keyStart == 3 && charStr == '"')
                            {
                                if (!jsonCharState.escapeChar)
                                {
                                    jsonCharState.keyStart = -1;
                                    return true;
                                }
                                else
                                {
                                    jsonCharState.escapeChar = false;
                                }
                            }
                        }
                        else if (jsonCharState.state == 1 && jsonCharState.jsonStart)//值阶段必须是Json开始了
                        {
                            if (jsonCharState.valueStart <= 0)
                            {
                                jsonCharState.valueStart = charStr == '"' ? 3 : 2;
                                return true;
                            }
                            else if (jsonCharState.valueStart == 2 && charStr == '\'' || jsonCharState.valueStart == 3 && charStr == '"')
                            {
                                if (!jsonCharState.escapeChar)
                                {
                                    jsonCharState.valueStart = -1;
                                    return true;
                                }
                                else
                                {
                                    jsonCharState.escapeChar = false;
                                }
                            }

                        }
                    }
                    break;
                case ':'://冒号
                    if (jsonCharState.jsonStart && jsonCharState.keyStart < 2 && jsonCharState.valueStart < 2 && jsonCharState.state == 0)
                    {
                        if (jsonCharState.keyStart == 1)
                        {
                            jsonCharState.keyStart = -1;
                        }
                        jsonCharState.state = 1;
                        return true;
                    }
                    break;
                case ','://逗号 //["aa",{aa:12,}]
                    if (jsonCharState.jsonStart)
                    {
                        if (jsonCharState.keyStart < 2 && jsonCharState.valueStart < 2 && jsonCharState.state == 1)
                        {
                            jsonCharState.state = 0;
                            jsonCharState.keyStart = 0;
                            jsonCharState.valueStart = 0;
                            jsonCharState.setDicValue = true;
                            return true;
                        }
                    }
                    else if (jsonCharState.arrayStart && jsonCharState.keyStart <= 2)
                    {
                        jsonCharState.keyStart = 0;
                        return true;
                    }
                    break;
                case ' ':
                case '\r':
                case '\n'://[ "a",\r\n{} ]
                case '\0':
                case '\t':
                    if (jsonCharState.keyStart <= 0 && jsonCharState.valueStart <= 0) //cs.jsonStart && 
                    {
                        return true;//跳过空格
                    }
                    break;
                default: //值开头
                    if (charStr == '\\') //转义符号
                    {
                        if (jsonCharState.escapeChar)
                        {
                            jsonCharState.escapeChar = false;
                        }
                        else
                        {
                            jsonCharState.escapeChar = true;
                            return true;
                        }
                    }
                    else
                    {
                        jsonCharState.escapeChar = false;
                    }
                    if (jsonCharState.jsonStart || jsonCharState.arrayStart) //Json或数组开始了
                    {
                        if (jsonCharState.keyStart <= 0 && jsonCharState.state == 0)
                        {
                            jsonCharState.keyStart = 1;//无引号的
                        }
                        else if (jsonCharState.valueStart <= 0 && jsonCharState.state == 1 && jsonCharState.jsonStart)//只有Json开始才有值
                        {
                            jsonCharState.valueStart = 1;//无引号的
                        }
                    }
                    break;
            }
            return false;
        }
    }
}