using Powers.HttpClient.Extensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Powers.HttpClient.Extensions.Extensions
{
    public static class UtilExtensions
    {
        public static bool IsUrl(this string str)
        {
            try
            {
                string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
                return Regex.IsMatch(str, Url);
            }
            catch
            {
                return false;
            }
        }

        public static bool Validate<T>(this T entity) where T : class
        {
            Type type = entity.GetType();
            foreach (var item in type.GetProperties())
            {
                if (item.IsDefined(typeof(AbstractCustomAttribute), true))//此处是重点
                {
                    //此处是重点
                    foreach (AbstractCustomAttribute attribute in item.GetCustomAttributes(typeof(AbstractCustomAttribute), true))
                    {
                        if (attribute == null)
                        {
                            throw new Exception("StringLengthAttribute not instantiate");
                        }
                        if (!attribute.Validate(item.GetValue(entity)))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}