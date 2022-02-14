using Powers.HttpClient.Extensions.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Powers.HttpClient.Extensions.Attributes
{
    public class UrlAttribute : AbstractCustomAttribute
    {
        public string? ErrorMessage { get; set; }

        public override bool Validate(object value)
        {
            if (value is string url)
            {
                if (url.IsUrl())
                {
                    return true;
                }
            }

            throw new InvalidOperationException(ErrorMessage);
        }
    }
}