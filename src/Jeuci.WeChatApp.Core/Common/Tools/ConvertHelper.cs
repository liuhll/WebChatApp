using System;

namespace Jeuci.WeChatApp.Common.Tools
{
    public static class ConvertHelper
    {
        public static T StringToEnum<T>(string values,bool isIgnoreCaseSize = false)
        {
            return (T)Enum.Parse(typeof(T), isIgnoreCaseSize ? values.ToLower() : values);
        }
    }
}