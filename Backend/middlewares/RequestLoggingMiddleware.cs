using Serilog;
using System.Text;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;

        // 🔹 Read request body (if available)
        string body = "";
        if (request.Method == HttpMethods.Post || request.Method == HttpMethods.Put)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            body = await reader.ReadToEndAsync();
            request.Body.Position = 0; // reset stream
        }

        // 🔹 Continue to next middleware
        var sw = System.Diagnostics.Stopwatch.StartNew();
        await _next(context);
        sw.Stop();

        // 🔹 Log the request info
        Log.Information("HTTP {Method} {Path} responded {StatusCode} in {Elapsed:0.000} ms | Body: {Body}",
            request.Method,
            request.Path,
            context.Response.StatusCode,
            sw.Elapsed.TotalMilliseconds,
            body);
    }
}
