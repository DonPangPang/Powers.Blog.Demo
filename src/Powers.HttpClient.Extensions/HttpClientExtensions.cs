using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Powers.HttpClient.Extensions.Attributes;
using Powers.HttpClient.Extensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Powers.HttpClient.Extensions
{
    public static class HttpClientExtensions
    {
        private static IHttpClientFactory _httpClientFactory = null!;

        public static void Configure(IHttpClientFactory httpClientFactory)
        {
            if (httpClientFactory is null)
            {
                throw new ArgumentNullException(nameof(httpClientFactory));
            }
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 使用HttpClient
        /// </summary>
        /// <param name="app"> </param>
        /// <returns> </returns>
        public static IApplicationBuilder UseHttpClient(this IApplicationBuilder app)
        {
            using var scoped = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            _httpClientFactory = scoped.ServiceProvider.GetService<IHttpClientFactory>()!;

            Configure(_httpClientFactory!);

            return app;
        }

        /// <summary>
        /// 使用HttpClient
        /// </summary>
        /// <param name="app"> </param>
        /// <returns> </returns>
        public static WebAssemblyHost UseHttpClient(this WebAssemblyHost app)
        {
            using var scoped = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            _httpClientFactory = scoped.ServiceProvider.GetService<IHttpClientFactory>()!;

            Configure(_httpClientFactory!);

            return app;
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <typeparam name="T"> 返回的类型 </typeparam>
        /// <param name="url">        Url地址 </param>
        /// <param name="parameters"> 请求参数 </param>
        /// <param name="token">      token </param>
        /// <returns> </returns>
        public static async Task<T?> GetAsync<T>(this string url, Dictionary<string, dynamic>? parameters = null, string token = "")
        {
            var client = _httpClientFactory.CreateClient();
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

            if (parameters is not null && parameters.Any())
            {
                url = $"{url}?{string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"))}";
            }

            var response = await client.GetFromJsonAsync<T?>(url);

            return response!;
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <typeparam name="T"> 返回类型 </typeparam>
        /// <param name="clientName"> HttpClient </param>
        /// <param name="route">      路由 </param>
        /// <param name="parameters"> 参数 </param>
        /// <param name="token">      token </param>
        /// <returns> </returns>
        public static async Task<T?> GetAsync<T>(this string clientName, string route,
            Dictionary<string, dynamic>? parameters = null,
            string token = "")
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);

            if (!string.IsNullOrWhiteSpace(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            httpClient.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
            if (parameters is not null && parameters.Any())
            {
                route = $"{route}?{string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"))}";
            }

            var response = await httpClient.GetFromJsonAsync<T?>(route);

            return response!;
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <typeparam name="T"> 返回类型 </typeparam>
        /// <param name="uri">        url </param>
        /// <param name="parameters"> 参数 </param>
        /// <param name="token">      token </param>
        /// <returns> </returns>
        [Obsolete("暂时弃用该方法", false)]
        public static async Task<T?> GetAsync<T>(this Uri uri,
            Dictionary<string, dynamic>? parameters = null,
            string token = "")
        {
            var url = uri.AbsoluteUri;

            var httpClient = _httpClientFactory.CreateClient();

            if (!string.IsNullOrWhiteSpace(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            httpClient.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
            if (parameters is not null && parameters.Any())
            {
                url = $"{url}?{string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"))}";
            }

            var response = await httpClient.GetFromJsonAsync<T?>(url);

            return response!;
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <typeparam name="T"> 返回类型 </typeparam>
        /// <param name="url">   Url地址 </param>
        /// <param name="body">  请求体 </param>
        /// <param name="token"> token </param>
        /// <returns> </returns>
        public static async Task<T?> PostAsync<T>(this string url, object body, string token = "")
        {
            var client = _httpClientFactory.CreateClient();

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
            var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, Application.Json);

            var response = await client.PostAsJsonAsync(url, bodyJson);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <typeparam name="T"> 返回类型 </typeparam>
        /// <param name="clientName"> HttpClient </param>
        /// <param name="route">      路由 </param>
        /// <param name="body">       请求体 </param>
        /// <param name="token">      token </param>
        /// <returns> </returns>
        public static async Task<T?> PostAsync<T>(this string clientName, string route, object body, string token = "")
        {
            var client = _httpClientFactory.CreateClient(clientName);

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
            var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, Application.Json);

            var response = await client.PostAsJsonAsync(route, bodyJson);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        /// <summary>
        /// PUT
        /// </summary>
        /// <typeparam name="T"> 返回类型 </typeparam>
        /// <param name="url">   Url地址 </param>
        /// <param name="body">  请求体 </param>
        /// <param name="token"> token </param>
        /// <returns> </returns>
        public static async Task<T?> PutAsync<T>(this string url, object body, string token = "")
        {
            var client = _httpClientFactory.CreateClient();

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
            var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, Application.Json);

            var response = await client.PutAsJsonAsync(url, bodyJson);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        /// <summary>
        /// PUT
        /// </summary>
        /// <typeparam name="T"> 返回类型 </typeparam>
        /// <param name="clientName"> HttpClient </param>
        /// <param name="route">      路由 </param>
        /// <param name="body">       请求体 </param>
        /// <param name="token">      token </param>
        /// <returns> </returns>
        public static async Task<T?> PutAsync<T>(this string clientName, string route, object body, string token = "")
        {
            var client = _httpClientFactory.CreateClient(clientName);

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
            var bodyJson = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, Application.Json);

            var response = await client.PutAsJsonAsync(route, bodyJson);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        /// <summary>
        /// DELETE
        /// </summary>
        /// <typeparam name="T"> 返回类型 </typeparam>
        /// <param name="url">   Url地址 </param>
        /// <param name="token"> token </param>
        /// <returns> </returns>
        public static async Task<T?> DeleteAsync<T>(this string url, string token = "")
        {
            var client = _httpClientFactory.CreateClient();

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetFromJsonAsync<T?>(url);

            return response!;
        }

        /// <summary>
        /// DELETE
        /// </summary>
        /// <typeparam name="T"> 返回类型 </typeparam>
        /// <param name="clientName"> HttpClient </param>
        /// <param name="route">      路由 </param>
        /// <param name="token">      token </param>
        /// <returns> </returns>
        public static async Task<T?> DeleteAsync<T>(this string clientName, string route, string token = "")
        {
            var client = _httpClientFactory.CreateClient(clientName);

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }

            client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetFromJsonAsync<T?>(route);

            return response!;
        }
    }
}