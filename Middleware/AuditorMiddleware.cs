using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace aspnetcore_middleware.Middleware
{
    public class AuditorMiddleware
    {
        private readonly RequestDelegate _next;

        public AuditorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("Before");

            var injectedRequestStream = new MemoryStream();
            var requestLog = $"REQUEST HttpMethod: {context.Request.Method}, Path: {context.Request.Path}";

            using (var bodyReader = new StreamReader(context.Request.Body))
            {
                var bodyAsText = bodyReader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(bodyAsText) == false)
                {
                    requestLog += $", Body : {bodyAsText}";
                }

                var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText);
                injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                injectedRequestStream.Seek(0, SeekOrigin.Begin);
                context.Request.Body = injectedRequestStream;
            }

            Console.WriteLine($"data {requestLog}");

            await _next(context);

            Console.WriteLine("After");
        }
    }
}
