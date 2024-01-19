using System.Configuration;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.TextToImage;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

string _endpoint = configuration.GetSection("AppSettings")["endpoint"];
string _apiKey = configuration.GetSection("AppSettings")["api-key"];
string _dalleDeployment = configuration.GetSection("AppSettings")["dalle-deployment"];
string _gptDeployment = configuration.GetSection("AppSettings")["gpt-deployment"];

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<Kernel>();


builder.Services
    .AddAzureOpenAITextToImage(
        _dalleDeployment
        , _endpoint
        , _apiKey);
builder.Services
    .AddAzureOpenAIChatCompletion(
        _gptDeployment
        , _endpoint
        , _apiKey);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
