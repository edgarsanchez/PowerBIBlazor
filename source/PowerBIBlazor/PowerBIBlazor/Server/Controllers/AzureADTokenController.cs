using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace PowerBIBlazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AzureADTokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AzureADTokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetADToken()
        {
            var tenantId = _configuration["AzureAppInfo:TenantId"];
            var clientId = _configuration["AzureAppInfo:ClientId"];
            var clientSecret = _configuration["AzureAppInfo:ClientSecret"];
            var authorityUri = new Uri($"https://login.microsoftonline.com/{tenantId}");

            var app = ConfidentialClientApplicationBuilder
                        .Create(clientId)
                        .WithClientSecret(clientSecret)
                        .WithAuthority(authorityUri)
                        .Build();

            var powerbiApiDefaultScope = "https://analysis.windows.net/powerbi/api/.default";
            var scopes = new string[] { powerbiApiDefaultScope };

            try
            {
                var authResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();
                return Ok(authResult.AccessToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
