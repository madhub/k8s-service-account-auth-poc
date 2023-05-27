using k8s;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace FooWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DemoController : ControllerBase
{
    private readonly Kubernetes client;
    private readonly ILogger<DemoController> logger;
    private readonly IConfiguration configuration;
    static HttpClient httpClient = new HttpClient();

    public DemoController(Kubernetes kubernetes, ILogger<DemoController> logger,IConfiguration configuration)
    {
        this.client = kubernetes;
        this.logger = logger;
        this.configuration = configuration;
    }
    /// <summary>
    /// Invoke BarWebapi service with service account token 
    /// BarWebapi authenticate the token & returns username & pod details
    /// </summary>
    /// <returns></returns>
    [HttpPost(Name = "invoke")]
    [Route("invoke")]
    public ActionResult Invoke()
    {
        var barEndpoint = configuration.GetValue<string>("barendpoint");
        try
        {
            String token = string.Empty;
            if ( KubernetesClientConfiguration.IsInCluster())
            {
                logger.LogInformation("Reading service account token" );
                token = System.IO.File.ReadAllText("/var/run/secrets/kubernetes.io/serviceaccount/token");
                logger.LogDebug("Token Content: {token}", token);
            }
            else
            {
                // hardcoded token for testing
                token = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ilo1NUQ5dDVVYlJZVEtZWENuR2lteWI3ZDYxX09Ub240am54S2JRMnVhTG8ifQ.eyJhdWQiOlsiaHR0cHM6Ly9rdWJlcm5ldGVzLmRlZmF1bHQuc3ZjLmNsdXN0ZXIubG9jYWwiXSwiZXhwIjoxNzE2NzEwMDI5LCJpYXQiOjE2ODUxNzQ\r\nwMjksImlzcyI6Imh0dHBzOi8va3ViZXJuZXRlcy5kZWZhdWx0LnN2Yy5jbHVzdGVyLmxvY2FsIiwia3ViZXJuZXRlcy5pbyI6eyJuYW1lc3BhY2UiOiJtYWRodS14cGxvciIsInBvZCI6eyJuYW1lIjoic2lnbmFscnNlcnZpY2UtZGVwbG95bWVudC02ZGM3NjhmOGQ2LTZia\r\n3NyIiwidWlkIjoiZTRkYjc5ZWQtNTFhYy00YmMzLWE1ZjEtNzhjN2FiZjdhNDg1In0sInNlcnZpY2VhY2NvdW50Ijp7Im5hbWUiOiJkZWZhdWx0IiwidWlkIjoiOTQ0YWU1YWYtODgyMi00ZjZmLWE1NTUtMGZkNGRkY2ExOTYwIn0sIndhcm5hZnRlciI6MTY4NTE3NzYzNn0\r\nsIm5iZiI6MTY4NTE3NDAyOSwic3ViIjoic3lzdGVtOnNlcnZpY2VhY2NvdW50Om1hZGh1LXhwbG9yOmRlZmF1bHQifQ.DW5POF9RfBeoRrBWDd-E54mt_9T6gn1rYqHRScfsMa6_WYJxaHdAeLreCGd12cDm0bLngKElndOt1-B-u1FOV16jM_01Z6P5D7KcSHJQnCYUSZY3hS\r\nH1ZR7eTwHfx7rPyW-bp0ra2xx2uehkFqugOKYwu-FCc0IiAYpoLjBc0dfeBDwDLgz_zCBYs23ly2vFB_0ER8D4A73j72AOCUPas-Fz1LSVt1cOZntg99WmB8ho5nDCvgNlk8W6LjbzvObXlhoFbjyV9IJ5p9qc8_2aTMWG-W5F0WmippQApda66Ny76IaNJzmEOsJFSb3_fCRE\r\neqr_m0VOq-ggDpqYuEeBDg";
            }

            logger.LogInformation("Sending request to: {barendpoint}", barEndpoint);
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get,barEndpoint );
            requestMessage.Headers.TryAddWithoutValidation("X-Client-Id", token);
            var result = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();
            logger.LogInformation("Sent request successfully: {barendpoint}", barEndpoint);
            return Ok(result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
        catch (Exception exp)
        {

            logger.LogError("Authentication failed ", exp);
        }
        


        return Ok("Authentication failed");
    }

}



