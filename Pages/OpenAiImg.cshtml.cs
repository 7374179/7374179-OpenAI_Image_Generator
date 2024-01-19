using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.TextToImage;
using System.Threading.Tasks;


namespace MyApp.Namespace
{
    public class OpenAiImgModel : PageModel
    {
        private readonly ITextToImageService _textToImageService;
        private readonly Kernel _kernel;

        public OpenAiImgModel(ITextToImageService textToImageService, Kernel kernel)
        {
            _textToImageService = textToImageService;
            _kernel = kernel;
        }

        [BindProperty]
        public string Phrase { get; set; }
        public string ImageUrl { get; private set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrEmpty(Phrase))
            {
                var prompt = @"Think about an artificial object that represents {{$input}}.";
                var executionSettings = new OpenAIPromptExecutionSettings
                {
                    MaxTokens = 256,
                    Temperature = 1
                };

                var getImgFunction = _kernel.CreateFunctionFromPrompt(prompt, executionSettings);
                var imageDescResult = await _kernel.InvokeAsync(getImgFunction, new() { ["input"] = Phrase });
                var imageDesc = imageDescResult.ToString();
                ImageUrl = await _textToImageService.GenerateImageAsync(imageDesc.Trim(), 1024, 1024);
            }

            return Page();
        }
    }
}
