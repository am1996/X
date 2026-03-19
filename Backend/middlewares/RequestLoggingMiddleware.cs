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

        string requestBody = "";
        if (request.Method == HttpMethods.Post || request.Method == HttpMethods.Put)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            request.Body.Position = 0;
        }

        var originalBody = context.Response.Body;
        using var memStream = new MemoryStream();
        context.Response.Body = memStream;

        var sw = System.Diagnostics.Stopwatch.StartNew();
        await _next(context);
        sw.Stop();

        memStream.Position = 0;
        string responseBody = await new StreamReader(memStream).ReadToEndAsync();
        memStream.Position = 0;
        await memStream.CopyToAsync(originalBody);
        context.Response.Body = originalBody;

        Log.Information("HTTP {Method} {Path} responded {StatusCode} in {Elapsed:0.000} ms | ReqBody: {ReqBody} | ResBody: {ResBody}",
            request.Method,
            request.Path,
            context.Response.StatusCode,
            sw.Elapsed.TotalMilliseconds,
            requestBody,
            responseBody);
    }
}
