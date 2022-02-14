using System;
using System.Collections.Generic;
using System.Text;

namespace Powers.HttpClient.Extensions.Attributes
{
    /// <summary>
    /// </summary>
    public abstract class AbstractCustomAttribute : Attribute
    {
        /// <summary>
        /// 定义校验抽象方法
        /// </summary>
        /// <param name="value"> 需要校验的值 </param>
        /// <returns> </returns>
        public abstract bool Validate(object value);
    }
}