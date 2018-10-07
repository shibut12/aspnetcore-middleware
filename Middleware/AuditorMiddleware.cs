using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore_middleware.Middleware
{
    public class AuditorMiddleware
    {
        private readonly RequestDelegate _next;
        private const int ReadChunkBufferLength = 4096;

        public AuditorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            var injectedRequestStream = new MemoryStream();
            var injectedResponseStream = new MemoryStream();
            var requestLog = $"REQUEST HttpMethod: {context.Request.Method}, Path: {context.Request.Path}";
            var responseLog = $"REQUEST HttpMethod: {context.Request.Method}, Path: {context.Request.Path}";

            string request = "";
            string response = "";

            using (var bodyReader = new StreamReader(context.Request.Body))
            {
                var bodyAsText = bodyReader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(bodyAsText) == false)
                {
                    requestLog += $", Body : {bodyAsText}";
                    request = bodyAsText;
                }

                var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText);
                injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                injectedRequestStream.Seek(0, SeekOrigin.Begin);
                context.Request.Body = injectedRequestStream;
            }

            Console.WriteLine($"Before data {requestLog}");


            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _next(context);

                response = await FormatResponse(context.Response);
                responseLog += $", Body : {response}";
                
                await responseBody.CopyToAsync(originalBodyStream);
            }

            Console.WriteLine($"After data {responseLog}");

            if(!string.IsNullOrEmpty(request) && !string.IsNullOrEmpty(response))
            {
                dynamic jRequest = JsonConvert.DeserializeObject(request);
                dynamic jResponse = JsonConvert.DeserializeObject(response);
                Console.WriteLine($"Request- Name {jRequest["name"]}");
                Console.WriteLine($"Response- Name {jResponse["name"]}");
            }
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }
    }
}
