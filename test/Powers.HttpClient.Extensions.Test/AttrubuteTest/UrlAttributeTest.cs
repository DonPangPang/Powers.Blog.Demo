using Powers.HttpClient.Extensions.Attributes;
using Powers.HttpClient.Extensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Powers.HttpClient.Extensions.Test.AttrubuteTest
{
    public class UrlAttributeTest
    {
        private readonly ITestOutputHelper _testOutputHelper = null!;

        public UrlAttributeTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test1()
        {
            UrlTestFunc("1111");
            UrlTestFunc("https://www.baidu.com/");
            UrlTestFunc("www.baidu.com");
            UrlTestFunc("baidu.com");
            UrlTestFunc("https://www.baidu");
            UrlTestFunc("https://www");
        }

        private void UrlTestFunc([Url(ErrorMessage = "kkk")] string url)
        {
            url.Validate();
            _testOutputHelper.WriteLine(url.IsUrl().ToString());
        }
    }
}