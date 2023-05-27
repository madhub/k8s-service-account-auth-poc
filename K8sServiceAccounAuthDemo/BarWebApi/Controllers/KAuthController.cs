using k8s;
using Microsoft.AspNetCore.Mvc;

namespace FooWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KAuthController : ControllerBase
{
    private readonly Kubernetes client;
    private readonly ILogger<KAuthController> logger;

    public KAuthController(Kubernetes kubernetes, ILogger<KAuthController> logger)
    {
        this.client = kubernetes;
        this.logger = logger;
    }
    [HttpGet(Name = "Auth")]
    public ActionResult Get([FromHeader(Name = "X-Client-Id")] string clientToken)
    {
        logger.LogInformation("Received X-Client-Id : {id}, authenticating the token ....", clientToken);
        var tokenReview = new k8s.Models.V1TokenReview()
        {
            Spec = new k8s.Models.V1TokenReviewSpec()
            { Token = clientToken }
        };

        try
        {
            var result = client.AuthenticationV1.CreateTokenReview(tokenReview);
            if (result.Status?.Authenticated.HasValue == true)
            {
                string podname = string.Empty;
                if (result.Status.User.Extra.TryGetValue("authentication.kubernetes.io/pod-name", out var values))
                {
                    podname = string.Join(",", values);
                }
                var authDetails = new AuthenticatedUser(result.Status.Audiences, result.Status.User.Username, podname);
                logger.LogInformation("Authentication success: {info}", authDetails);
                return Ok(authDetails);
            }
            else
            {
                logger.LogError("Authentication failed ");
            }

        }
        catch (Exception exp)
        {

            logger.LogError("Authentication failed ", exp);
        }


        return Ok("Authentication failed");
    }

}

