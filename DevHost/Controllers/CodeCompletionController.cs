using Microsoft.AspNetCore.Mvc;
using MonacoRoslynCompletionProvider;
using MonacoRoslynCompletionProvider.Api;

namespace DevHost.Controllers;

[ApiController]
[Route("completion")]
public class CodeCompletionController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public CodeCompletionController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Route("complete")]
    public async Task<ActionResult<TabCompletionResult[]>> complete(TabCompletionRequest request)
    {
        try
        {
            // var tabCompletionRequest = JsonSerializer.Deserialize<TabCompletionRequest>(text);
            TabCompletionResult[] results = await CompletitionRequestHandler.Handle(request);
            // await JsonSerializer.SerializeAsync(result, tabCompletionResults);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route("signature")]
    public async Task<ActionResult<SignatureHelpResult>> complete(SignatureHelpRequest request)
    {
        try
        {
            // var tabCompletionRequest = JsonSerializer.Deserialize<TabCompletionRequest>(text);
            SignatureHelpResult result = await CompletitionRequestHandler.Handle(request);
            // await JsonSerializer.SerializeAsync(result, tabCompletionResults);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route("hover")]
    public async Task<ActionResult<HoverInfoResult>> complete(HoverInfoRequest request)
    {
        try
        {
            // var tabCompletionRequest = JsonSerializer.Deserialize<TabCompletionRequest>(text);
            HoverInfoResult result = await CompletitionRequestHandler.Handle(request);
            // await JsonSerializer.SerializeAsync(result, tabCompletionResults);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route("codeCheck")]
    public async Task<ActionResult<CodeCheckResult[]>> complete(CodeCheckRequest request)
    {
        try
        {
            // var tabCompletionRequest = JsonSerializer.Deserialize<TabCompletionRequest>(text);
            CodeCheckResult[] results = await CompletitionRequestHandler.Handle(request);
            // await JsonSerializer.SerializeAsync(result, tabCompletionResults);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
