using Presence_API.Middleware.Response;
using Presence_API.Services.Chat;
using Presence_API.Services.Memory;
using Presence_API.Services.TextToSpeech;
using OpenAIApi.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IAIService, OpenAIService>();
        builder.Services.AddSingleton<IMemoryService, MemoryService>();
        builder.Services.AddSingleton<IResponseMiddleware, ResponseMiddleware>();
        builder.Services.AddSingleton<IChatService, TwitchChatService>();
        builder.Services.AddSingleton<ITextToSpeechService, MicrosoftSpeechService>();

        var app = builder.Build();
        //Used to initialize the IChatService
        app.Services.GetService<IChatService>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}